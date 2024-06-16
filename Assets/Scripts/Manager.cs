using UnityEngine;

/// <summary>
/// �̱����� �����ϱ� ���� ���׸� Ŭ������ �����մϴ�.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Manager<T> : MonoBehaviour
    where T : Manager<T>
{
    public static T Instance;

    /// <summary>
    /// �ν��Ͻ��� �����մϴ�.
    /// </summary>
    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

}
