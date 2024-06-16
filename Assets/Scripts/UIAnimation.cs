using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ĵ������ �����ϴ� UI Image�� ��������Ʈ �ִϸ��̼��� ���� Ŭ�����Դϴ�.
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
