using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class GameManager : Manager<GameManager>
{
    public GameState curState { get; set; } = GameState.Title;
    public float gameTime { get; private set; } = 60.0f;
    public int curRound { get; private set; } = 1;
    public TextMeshProUGUI gameTimeText;
    public TextMeshProUGUI gameRoundText;
    public NPCTextBasement npc;
    public Result result;

    public UnitDatabaseSO unitDatabase;
    public List<BaseBuff> buffs = new List<BaseBuff>();

    public Transform team1Parent;
    public Transform team2Parent;

    public Action OnRoundStart;
    public Action OnRoundEnd;
    public Action<UnitBasement> OnUnitDied;

    private List<UnitBasement> team1Units = new List<UnitBasement>();
    private List<UnitBasement> team2Units = new List<UnitBasement>();

    private int enemyUnitsNum = 3;   //적 유닛의 수를 나타냅니다. 라운드마다 하나씩 늘어납니다.

    private bool isClick;
    private int escapeCount;    // 뒤로가기 키 두번 이상 누를 시 게임 종료되게 하기 위한 변수입니다.

    private new void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        Team2Setup();      
        gameTime = 60.0f;
        curRound = 1;
        isClick = false;
        escapeCount = 0;
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape) && !isClick)
        {
            StartCoroutine(QuitTimer());
            isClick = true;
            escapeCount++;
            if(escapeCount >= 2)
            {
                escapeCount = 0;
                isClick = false;
                Application.Quit();
            }
        }
        if (curState == GameState.Fight)
        {
            if (gameTime > 0f)
            {
                CheckGameEnd();
                gameTime -= Time.deltaTime;
                gameTimeText.text = "Time: " + ((int)gameTime).ToString();
            }
            else
            {
                gameTimeText.text = "Time: 0";
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.Defeat, 2.0f);
                result.Defeat();
                npc.Victory();
                RoundEnd();
            }
        }
    }

    private void Team2Setup()
    {
        for (int i = 0; i < enemyUnitsNum; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, unitDatabase.allUnits.Count);
            UnitBasement newUnit = Instantiate(unitDatabase.allUnits[randomIndex].prefab);
            team2Units.Add(newUnit);

            newUnit.Setup(Team.Team2, TileManager.Instance.GetRandomFreeNode(Team.Team2));
        }
    }

    private void TeamReset()
    {
        if(team1Units.Count > 0)
        {
            foreach(var unit in team1Units)
            {
                unit.CurrentNode.SetOccupied(false);
                if (unit.Destination != null) unit.Destination.SetOccupied(false);
                Destroy(unit.gameObject);
            }
        }
        if (team2Units.Count > 0)
        {
            foreach (var unit in team2Units)
            {
                unit.CurrentNode.SetOccupied(false);
                if (unit.Destination != null) unit.Destination.SetOccupied(false);
                Destroy(unit.gameObject);
            }
        }
        team1Units.Clear();
        team2Units.Clear();

        OnRoundStart = null;
        OnRoundEnd = null;
        OnUnitDied = null;
    }

    private void CheckGameEnd()
    {
        if(team1Units.Count == 0)
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Defeat, 2.0f);
            result.Defeat();
            npc.Victory();
            RoundEnd();
        }
        else if(team2Units.Count == 0)
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Victory, 2.0f);
            result.Victory();
            npc.Groan();
            RoundEnd();
        }
    }

    //이 아래로 public

    public int Buff(string type)
    {
        if(buffs.Count > 0)
        {
            int tmp = 0;
            for(int i = 0; i < buffs.Count; i++)
            {
                if (buffs[i].buffType.Equals(type))
                {
                    tmp += buffs[i].buffAmount;
                }
            }
            return tmp;
        }
        else
        {
            return 0;
        }
    }

    public void ApplyBuff(string type)
    {
        switch (type)
        {
            case "attack":
                foreach(UnitBasement unit in team1Units)
                {
                    unit.extraDamage = Buff("attack");
                }
                break;
            case "defense":
                foreach (UnitBasement unit in team1Units)
                {
                    unit.extraDefense = Buff("defense");
                }
                break;
            case "attackDebuff":
                foreach (UnitBasement unit in team2Units)
                {
                    unit.extraDefense = Buff("attackDebuff");
                }
                break;
            case "defenseDebuff":
                foreach (UnitBasement unit in team2Units)
                {
                    unit.extraDefense = Buff("defenseDebuff");
                }
                break;
        }
    }

    public void OnUnitBought(UnitDatabaseSO.UnitData unitData)
    {
        UnitBasement newUnit = Instantiate(unitData.prefab, team1Parent);
        newUnit.gameObject.name = unitData.name;
        team1Units.Add(newUnit);

        newUnit.Setup(Team.Team1, TileManager.Instance.GetFreeNode(Team.Team1));
    }

    public List<UnitBasement> GetOtherUnits(Team otherTeam)
    {
        if (otherTeam == Team.Team1) return team2Units;
        else return team1Units;
    }

    public void UnitDead(UnitBasement u)
    {
        if (u.GetTeam() == Team.Team2) npc.Surprised();
        else npc.Joy();

        u.CurrentNode.SetOccupied(false);
        if (u.Destination != null) u.Destination.SetOccupied(false);

        PlayerData.Instance.EarnMoney(1);

        team1Units.Remove(u);
        team2Units.Remove(u);
        OnUnitDied?.Invoke(u);
        Destroy(u.gameObject);
    }

    //Round 관련 함수
    public void NextRound()
    {
        AudioManager.Instance.PlayBgmByIndex((int)GameState.Prepare, 0.3f);
        curRound++;
        gameRoundText.text = "Round: " + curRound.ToString();
        PlayerData.Instance.EarnMoney(5 * curRound);
        TeamReset();
        npc.Normal();
        curState = GameState.Prepare;
        gameTime = 60.0f;
        gameTimeText.text = "Time: " + ((int)gameTime).ToString();
        enemyUnitsNum++;
        Team2Setup();
    }

    public void RoundStart()
    {
        if (curState != GameState.Prepare)
            return;
        AudioManager.Instance.PlayBgmByIndex((int)GameState.Fight);
        curState = GameState.Fight;
        for (int i = 0; i < enemyUnitsNum; i++)
        {
            Color currentColor = team2Units[i].spriteRenderer.color;
            Color targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, 1.0f);
            team2Units[i].spriteRenderer.color = targetColor;
        }
        OnRoundStart?.Invoke();
    }

    public void RoundEnd()
    {
        AudioManager.Instance.PlayBgm(false);
        curState = GameState.Result;
        OnRoundEnd?.Invoke();
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator QuitTimer()
    {
        yield return new WaitForSeconds(0.1f);
        isClick = false;
        yield return new WaitForSeconds(0.3f);
        isClick = false;
        escapeCount = 0;
    }
}

public enum Team
{
    Team1,
    Team2
}

public enum GameState
{
    Title,
    Prepare,
    Fight,
    Result
}