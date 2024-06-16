using UnityEngine;

/// <summary>
/// 모바일 기기에 따른 해상도 변화를 막기 위해서 작성된 코드입니다.
/// 2D 카메라 기준입니다.
/// </summary>
public class CameraResolution : MonoBehaviour
{
    /// <summary> 해당 비율을 목표로 카메라를 조정합니다. </summary>
    public float targetAspectRatio = 16.0f / 9.0f;
    public float orthographicSize = 5;
    private void Awake()
    {
        Camera camera = GetComponent<Camera>();

        // 현재 화면의 비율
        float currentAspectRatio = (float)Screen.width / Screen.height;

        // 목표 비율에 따른 Orthographic 크기 조절
        float orthoSize = orthographicSize * (targetAspectRatio / currentAspectRatio);
        camera.orthographicSize = orthoSize;
    }
}
