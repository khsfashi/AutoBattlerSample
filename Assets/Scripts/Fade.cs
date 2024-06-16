using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 화면 전환을 위한 클래스입니다.
/// </summary>
public class Fade : MonoBehaviour
{
    public Canvas canvas;
    public Image image;
    public GameObject obj;

    private float time = 0f;
    private float fadeTime = 1f;

    public void StartFadeEffect()
    {
        StartCoroutine(FadeEffect());
    }

    IEnumerator FadeEffect()
    {
        time = 0;
        image.gameObject.SetActive(true);
        Color color = image.color;
        while(color.a < 1f)
        {
            time += Time.deltaTime / fadeTime;
            color.a = Mathf.Lerp(0, 1, time);
            image.color = color;
            yield return null;
        }
        time = 0f;
        obj.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        while (color.a > 0f)
        {
            time += Time.deltaTime / fadeTime;
            color.a = Mathf.Lerp(1, 0, time);
            image.color = color;
            yield return null;
        }
        image.gameObject.SetActive(false);
        canvas.gameObject.SetActive(false);
        GameManager.Instance.curState = GameState.Prepare;
        AudioManager.Instance.PlayBgmByIndex((int)GameState.Prepare, 0.3f);
        yield return null;
    }
}
