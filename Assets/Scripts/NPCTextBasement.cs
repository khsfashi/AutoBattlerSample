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
        npcText.text = "흠....";
    }

    public virtual void Surprised()
    {
        npcText.text = "개발자, 이번 개발이 첫 유니티 개발이었다고?";
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.SkeletonTalk1, 1.5f);
        TextDelay();
    }

    public virtual void Groan()
    {
        CancelInvoke("Normal");
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.SkeletonDefeat, 1.5f);
        npcText.text = "내가 지다니..!!";
    }

    public virtual void Joy()
    {
        npcText.text = "후후...";
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.SkeletonTalk3, 1.5f);
        TextDelay();
    }

    public virtual void Victory()
    {
        CancelInvoke("Normal");
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.SkeletonTalk2, 1.5f);
        npcText.text = "역시 미숙하군!!...";
    }

    private void TextDelay()
    {
        CancelInvoke("Normal");
        Invoke("Normal", 1f);
    }
}
