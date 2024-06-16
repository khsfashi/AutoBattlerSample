using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ������ ������ ��Ÿ���� ���� ī�� Ŭ�����Դϴ�.
/// </summary>
public class UICard : MonoBehaviour
{
    public Image icon;
    public new TextMeshProUGUI name;
    public TextMeshProUGUI cost;

    private UIShop shopRef;
    private UnitDatabaseSO.UnitData myData;

    public void Setup(UnitDatabaseSO.UnitData myData, UIShop shopRef)
    {
        icon.sprite = myData.icon;
        name.text = myData.name;
        cost.text = myData.cost.ToString();

        this.shopRef = shopRef;
        this.myData = myData;
    }

    public void OnClick()
    {
        shopRef.OnCardClick(this, myData);
    }
}
