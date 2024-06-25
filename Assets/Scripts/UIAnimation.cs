using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ĵ������ �����ϴ� UI Image�� ��������Ʈ �ִϸ��̼��� ���� Ŭ�����Դϴ�.
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
