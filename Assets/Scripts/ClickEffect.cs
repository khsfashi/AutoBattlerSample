using UnityEngine;

/// <summary>
/// ȭ�� ��ġ �� ��Ÿ���� ��ġ ����Ʈ�� ���� Ŭ�����Դϴ�.
/// </summary>
public class ClickEffect : MonoBehaviour
{
    public UIAnimation uiObject;
    public Canvas canvas;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out Vector2 localPoint);

            uiObject.transform.localPosition = localPoint;

            uiObject.Func_StopUIAnim();
            uiObject.Func_PlayUIAnim();
        }
    }
}
