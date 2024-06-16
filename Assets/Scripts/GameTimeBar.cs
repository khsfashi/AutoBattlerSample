using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 제한 시간을 나타내는 TimeBar를 구현합니다.
/// </summary>
public class GameTimeBar : MonoBehaviour
{
    public Transform bar;
    public Image rage;

    private float gameTime = 60.0f;
    private float curTime = 60.0f;

    private void Start()
    {
        gameTime = GameManager.Instance.gameTime;
        curTime = gameTime;
    }
    public void UpdateBar()
    {
        curTime = GameManager.Instance.gameTime;
        float newScale = Mathf.Clamp(curTime/gameTime, 0.15f, 1.0f);
        Vector3 scale = bar.transform.localScale;
        scale.x = newScale;
        bar.transform.localScale = scale;
    }

    /// <summary>TimeBar 가운데에 있는 악마의 눈이 점점 붉게 변합니다.</summary>
    public void UpdateRage()
    {
        curTime = GameManager.Instance.gameTime;
        float newAlpha = Mathf.Clamp01(1.0f - curTime / gameTime);
        Color currentColor = rage.color;
        Color targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
        rage.color = targetColor;
    }

    private void Update()
    {
       
        UpdateBar();
        UpdateRage();
        
    }
}
