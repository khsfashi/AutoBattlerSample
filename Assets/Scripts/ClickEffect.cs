using UnityEngine;

/// <summary>
/// 화면 터치 시 나타나는 터치 이펙트를 위한 클래스입니다.
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
