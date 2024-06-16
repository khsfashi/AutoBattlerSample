using UnityEngine;

public class BuffManager : Manager<BuffManager>
{
    public GameObject buffPrefab;

    private new void Awake()
    {
        base.Awake();
    }

    public void CreateBuff(string type, int amount, float duration, Sprite icon)
    {
        int money = amount < 0 ? 1 : 2;
        if (PlayerData.Instance.Money - money < 0) return;

        PlayerData.Instance.SpendMoney(money);
        GameObject obj = Instantiate(buffPrefab, this.transform);
        obj.GetComponent<BaseBuff>().Setup(type, amount, duration);
        obj.GetComponent<UnityEngine.UI.Image>().sprite = icon;
    }
}
