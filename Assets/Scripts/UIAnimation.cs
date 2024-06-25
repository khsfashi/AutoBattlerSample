using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 캔버스에 존재하는 UI Image의 스프라이트 애니메이션을 위한 클래스입니다.
/// </summary>
public class UIAnimation : MonoBehaviour
{
    public bool LoopAnimation = false;
    public Image image;
    public Sprite[] sprites;
    public float speed = 0.02f;

    private int curSpriteIndex;
    private Coroutine animationCoroutine;
    private bool isDone;

    public void Func_PlayUIAnim()
    {
        isDone = false;
        curSpriteIndex = 0;
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
        image.sprite = sprites[curSpriteIndex++];
        yield return new WaitForSeconds(speed);
        if (image == null)
        {
            yield break;
        }
        if (curSpriteIndex >= sprites.Length)
        {
            if (!LoopAnimation)
            {
                isDone = true;
                yield break;
            }
            curSpriteIndex = 0;
        }
        if (isDone == false)
            animationCoroutine = StartCoroutine(Func_PlayAnimUI());
    }
}
