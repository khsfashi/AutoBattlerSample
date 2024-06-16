using UnityEngine;

public class BuffButton : MonoBehaviour
{
    public string type;
    public int amount;
    public float duration;
    public Sprite icon;

    public void Click()
    {
        if (GameManager.Instance.curState != GameState.Fight) return;
        BuffManager.Instance.CreateBuff(type, amount, duration, icon);
    }
}
