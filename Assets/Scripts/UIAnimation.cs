using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 캔버스에 존재하는 UI Image의 스프라이트 애니메이션을 위한 클래스입니다.
/// </summary>
public class UIAnimation : MonoBehaviour
{
    public Image image;
    public Sprite[] sprites;
    public float speed = .02f;
    private int curSpriteIndex;
    Coroutine animationCoroutine;
    bool isDone;

    public void Func_PlayUIAnim()
    {
        isDone = false;
        animationCoroutine = StartCoroutine(Func_PlayAnimUI());
    }

    public void Func_StopUIAnim() 
    {
        if (animationCoroutine != null)
        {
            isDone = true;
            curSpriteIndex = 0;
            StopCoroutine(animationCoroutine);
        }
    }

    IEnumerator Func_PlayAnimUI()
    {
        yield return new WaitForSeconds(speed);
        if (image == null)
        {
            yield break;
        }
        if (curSpriteIndex >= sprites.Length)
        {
            isDone = true;
            curSpriteIndex = 0;
            yield break;
        }
        image.sprite = sprites[curSpriteIndex];
        curSpriteIndex += 1;
        if (isDone == false)
            animationCoroutine = StartCoroutine(Func_PlayAnimUI());
    }
}
