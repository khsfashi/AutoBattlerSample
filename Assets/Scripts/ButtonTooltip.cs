using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTooltip : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isPointerDown = false;
    private float pointerDownTimer = 0;

    public float requiredHoldTime = 0.1f;
    public GameObject tooltip;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        Reset();
    }

    private void Update()
    {
        if (isPointerDown)
        {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer >= requiredHoldTime)
            {
                ShowTooltip();
            }
        }
    }

    private void ShowTooltip()
    {
        tooltip.SetActive(true);
    }

    private void Reset()
    {
        isPointerDown = false;
        pointerDownTimer = 0;
        tooltip.SetActive(false);
    }
}
