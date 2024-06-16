using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BaseBuff : MonoBehaviour
{
    public string buffType;
    public int buffAmount;
    public float buffDuration;
    public float currentTime;
    public Image icon;

    private WaitForSeconds seconds = new WaitForSeconds(0.01f);

    private void Awake()
    {
        icon = GetComponent<Image>();
    }

    public void Setup(string type, int amount, float duration)
    {
        buffType = type;
        buffAmount = amount;
        buffDuration = duration;
        currentTime = duration;
        icon.fillAmount = 1;
        if(amount > 0) AudioManager.Instance.PlaySfx(AudioManager.Sfx.Buff);
        else AudioManager.Instance.PlaySfx(AudioManager.Sfx.Debuff);
        Execute();
    }

    public void Execute()
    {
        GameManager.Instance.buffs.Add(this);
        GameManager.Instance.ApplyBuff(buffType);
        StartCoroutine(Activation());
    }

    IEnumerator Activation()
    {
        gameObject.SetActive(true);
        while (currentTime > 0)
        {
            currentTime -= 0.01f;
            icon.fillAmount = currentTime / buffDuration;
            yield return seconds;
        }
        icon.fillAmount = 0;
        currentTime = 0;
        DeActivation();
    }

    public void DeActivation()
    {
        GameManager.Instance.buffs.Remove(this);
        GameManager.Instance.ApplyBuff(buffType);
        Destroy(gameObject);
    }
}
