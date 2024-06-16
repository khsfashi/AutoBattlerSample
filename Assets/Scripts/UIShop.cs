using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 유닛 카드를 생성하고, 그 카드를 플레이어가 구매할 수 있게 하는 상점 클래스입니다.
/// </summary>
public class UIShop : MonoBehaviour
{
    public List<UICard> allCards;
    public TextMeshProUGUI money;
    public int rerollCost = 1;

    private UnitDatabaseSO cachedDb;

    private void Start()
    {
        cachedDb = GameManager.Instance.unitDatabase;
        GenerateCard();
        PlayerData.Instance.OnUpdate += Refresh;
        Refresh();
    }

    private void Update()
    {
        Refresh();
    }

    public void GenerateCard()
    {
        for(int i = 0; i < allCards.Count; i++)
        {
            if (!allCards[i].gameObject.activeSelf)
                allCards[i].gameObject.SetActive(true);

            allCards[i].Setup(cachedDb.allUnits[Random.Range(0, cachedDb.allUnits.Count)], this);
        }
    }

    public void OnCardClick(UICard card, UnitDatabaseSO.UnitData cardData)
    {
        if (GameManager.Instance.curState != GameState.Prepare)
            return;
        if (PlayerData.Instance.CanAfford(cardData.cost))
        {
            PlayerData.Instance.SpendMoney(cardData.cost);
            card.gameObject.SetActive(false);
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.GoldSound);
            GameManager.Instance.OnUnitBought(cardData);
        }
    }

    /// <summary>상점의 유닛 카드를 재생성합니다.</summary>
    public void OnRerollClick()
    {
        if (GameManager.Instance.curState != GameState.Prepare)
            return;
        if (PlayerData.Instance.CanAfford(rerollCost))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.GoldSound);
            PlayerData.Instance.SpendMoney(rerollCost);
            GenerateCard();
        }
           
    }

    void Refresh()
    {
        money.text = PlayerData.Instance.Money.ToString();
    }
}
