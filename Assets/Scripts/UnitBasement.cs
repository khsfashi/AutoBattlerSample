using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ������ �����ϴ� Ŭ������ �ۼ��մϴ�.
/// </summary>
public class UnitBasement : MonoBehaviour
{
    public HPBar barPrefab;
    public SpriteRenderer spriteRenderer;
    public int baseDamage = 2;
    public int baseHP;
    public int baseDefense = 1;
    [Range(1, 5)]
    public float range = 1.5f;

    public float attackSpeed = 1f;
    public float movementSpeed = 1f;
    public int extraDamage = 0;
    public int extraDefense = 0;

    public int GetDamage => baseDamage + extraDamage;
    public int GetDefense => baseDefense + extraDefense;

    public Node CurrentNode => currentNode;
    public Node Destination => destination != null ? destination : null;

    protected Animator animator;
    protected bool HasEnemy => currentTarget != null;
    protected bool IsInRange => currentTarget != null && GetUnitToUnitDistance(this.transform.position, currentTarget.transform.position) <= range;

    protected Team myTeam;
    protected Node currentNode;
    protected UnitBasement currentTarget = null;
    protected bool moving;
    protected Node destination;
    protected bool changeDirExecuted = false;

    protected bool canAttack = true;
    protected bool dead = false;
    protected HPBar hpbar;

    public void Setup(Team team, Node spawnNode)
    {
        myTeam = team;
        if(myTeam == Team.Team2)
        {
            spriteRenderer.flipX = true;
            Color originalColor = spriteRenderer.color;
            Color redColor = new Color(1f, 0f, 0f, 0.25f);
            spriteRenderer.color = Color.Lerp(originalColor, redColor, 0.5f);
        }
        this.currentNode = spawnNode;
        transform.position = currentNode.worldPosition;
        currentNode.SetOccupied(true);
        hpbar = Instantiate(barPrefab, this.transform);
        hpbar.SetUp(this.transform, baseHP);
        animator = GetComponent<Animator>();
        animator.SetBool("isAttack", false);
    }

    public void UnitReset()
    {
        currentNode.SetOccupied(false);
        GameManager.Instance.UnitDead(this);
    }

    public void EncreasePowerByRound(int curRound)
    {
        baseDamage = baseDamage + 1;
        baseHP = baseHP + curRound;
    }

    public void TakeDamage(int amount)
    {
        amount = amount - GetDefense;
        if (amount < 1) amount = 1;
        baseHP -= amount;
        hpbar.UpdateBar(baseHP);
        if(baseHP <= 0 && !dead)
        {
            dead = true;
            currentNode.SetOccupied(false);
            GameManager.Instance.UnitDead(this);
            GameManager.Instance.OnRoundEnd -= OnRoundEnd;
        }
    }

    public Team GetTeam()
    {
        return myTeam;
    }

    public void SetCurrentNode(Node node)
    {
        currentNode = node;
    }
    protected virtual void OnRoundStart() { FindTarget(); }
    protected virtual void OnRoundEnd() { animator.SetBool("isMove", false); animator.SetBool("isAttack", false); }
    protected virtual void OnUnitDied(UnitBasement diedUnit) { }
    protected void Start()
    {
        GameManager.Instance.OnRoundStart += OnRoundStart;
        GameManager.Instance.OnRoundEnd += OnRoundEnd;
        GameManager.Instance.OnUnitDied += OnUnitDied;
    }

    protected virtual float GetUnitToUnitDistance(Vector3 a, Vector3 b){ return Vector3.Distance(a, b); }
    protected void ChangeDir(Node next) 
    {
        if (next == null) return;

        float nextX = next.worldPosition.x;
        float myX = this.transform.position.x;

        if (nextX == myX) return;

        if(nextX < myX) spriteRenderer.flipX = true;
        else if (nextX > myX) spriteRenderer.flipX = false;
    }

    protected virtual void FindTarget()
    {
        if (currentTarget == null)
        {
            var allEnemies = GameManager.Instance.GetOtherUnits(myTeam);

            float minDistance = Mathf.Infinity;
            UnitBasement candidateTarget = null;
            foreach (UnitBasement u in allEnemies)
            {
                float distance = GetUnitToUnitDistance(u.transform.position, this.transform.position);
                if(distance <= range)
                {
                    candidateTarget = u;
                    break;
                }
                else if (distance <= minDistance)
                {
                    minDistance = distance;
                    candidateTarget = u;
                }
            }

            currentTarget = candidateTarget;
        }
    }

    protected void GetInRange()
    {
        if (currentTarget == null) return;

        if (!moving)
        {
            // ���� ��ǥ�� �ϴ� Ÿ���� �ֺ� ��� �� ����ִ� ��带 ã�� �ִܰ�θ� Ž���մϴ�.
            Node candidateDestination = null;
            List<Node> candidates = TileManager.Instance.GetNodesCloseTo(currentTarget.currentNode);
            candidates = candidates.OrderBy(x => GetUnitToUnitDistance(x.worldPosition, this.transform.position)).ToList();
            candidateDestination = FindEmptyNode(candidates);
            if (candidateDestination == null) return;

            var path = TileManager.Instance.GetPath(currentNode, candidateDestination);

            //���� �ִܰ�ΰ� ���� ��� �ڽ� �ֺ� ��� �� ���� Ÿ�ٰ� ���� ����� ����ִ� ���� �̵��մϴ�.
            if (path == null || path.Count <= 1 || path[1].IsOccupied)
            {
                List<Node> myNeighbors = TileManager.Instance.GetNodesCloseTo(currentNode);
                myNeighbors = myNeighbors.OrderBy(x => GetUnitToUnitDistance(x.worldPosition, currentTarget.transform.position)).ToList();
                candidateDestination = FindEmptyNode(myNeighbors);

                path = TileManager.Instance.GetPath(currentNode, candidateDestination);
                if (path == null || path.Count <= 1 || path[1].IsOccupied) return;
                path[1].SetOccupied(true);
                currentNode.SetOccupied(false);
                destination = path[1];
                currentTarget = null;
            }
            //�ִܰ�ΰ� �����Ұ�� �� ��θ� ���� �̵��մϴ�.
            else
            {
                path[1].SetOccupied(true);
                currentNode.SetOccupied(false);
                destination = path[1];
            }
            ChangeDir(destination);
        }
        moving = !MoveForwards();
        if (!moving)
        {
            animator.SetBool("isMove", false);
            currentNode = destination;
        }

        //�̵� ���� ���� �̵����� ���� ��Ÿ� �ȿ� �����ų� ���� ����� ���� �ٲ� �� �����Ƿ� Ž�� ���� null�� �����մϴ�.
        currentTarget = null;
    }

    private Node FindEmptyNode(List<Node> nodes)
    {
        foreach (var node in nodes)
        {
            if (!node.IsOccupied) return node;
        }
        return null;
    }

    protected bool MoveForwards()
    {
        Vector3 direction = destination.worldPosition - this.transform.position;
        animator.SetBool("isMove", true);
        if (direction.sqrMagnitude <= 0.002f)
        {
            transform.position = destination.worldPosition;
            return true;
        }

        this.transform.position += direction.normalized * movementSpeed * Time.deltaTime;
        return false;
    }

    protected virtual void Attack()
    {
        if (!canAttack) return;
        ChangeDir(currentTarget.currentNode);
        float waitBetweenAttack = 1 / attackSpeed;
        StartCoroutine(WaitAttackCoroutine(waitBetweenAttack));
    }

    IEnumerator WaitAttackCoroutine(float waitTime)
    {
        canAttack = false;
        animator.SetBool("isAttack", true);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));
        float clipLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(clipLength);
        animator.SetBool("isAttack", false);
        if(waitTime > clipLength) yield return new WaitForSeconds(waitTime - clipLength);
        canAttack = true;
        
        animator.speed = 1f;
    }
}
