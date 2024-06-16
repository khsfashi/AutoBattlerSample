using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCTextBasement : MonoBehaviour
{
    public Image npcImage;
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI npcText;

    public void SetUp(string name, string text)
    {
        npcName.text = name;
        npcText.text = text;
    }
    public virtual void Normal()
    {
        npcText.text = "��....";
    }

    public virtual void Surprised()
    {
        npcText.text = "������, �̹� ������ ù ����Ƽ �����̾��ٰ�?";
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.SkeletonTalk1, 1.5f);
        TextDelay();
    }

    public virtual void Groan()
    {
        CancelInvoke("Normal");
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.SkeletonDefeat, 1.5f);
        npcText.text = "���� ���ٴ�..!!";
    }

    public virtual void Joy()
    {
        npcText.text = "����...";
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.SkeletonTalk3, 1.5f);
        TextDelay();
    }

    public virtual void Victory()
    {
        CancelInvoke("Normal");
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.SkeletonTalk2, 1.5f);
        npcText.text = "���� �̼��ϱ�!!...";
    }

    private void TextDelay()
    {
        CancelInvoke("Normal");
        Invoke("Normal", 1f);
    }
}
