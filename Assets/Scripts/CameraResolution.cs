using UnityEngine;

/// <summary>
/// ����� ��⿡ ���� �ػ� ��ȭ�� ���� ���ؼ� �ۼ��� �ڵ��Դϴ�.
/// 2D ī�޶� �����Դϴ�.
/// </summary>
public class CameraResolution : MonoBehaviour
{
    /// <summary> �ش� ������ ��ǥ�� ī�޶� �����մϴ�. </summary>
    public float targetAspectRatio = 16.0f / 9.0f;
    public float orthographicSize = 5;
    private void Awake()
    {
        Camera camera = GetComponent<Camera>();

        // ���� ȭ���� ����
        float currentAspectRatio = (float)Screen.width / Screen.height;

        // ��ǥ ������ ���� Orthographic ũ�� ����
        float orthoSize = orthographicSize * (targetAspectRatio / currentAspectRatio);
        camera.orthographicSize = orthoSize;
    }
}
