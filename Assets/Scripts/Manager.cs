using UnityEngine;

/// <summary>
/// 싱글톤을 구현하기 위한 제네릭 클래스를 정의합니다.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Manager<T> : MonoBehaviour
    where T : Manager<T>
{
    public static T Instance;

    /// <summary>
    /// 인스턴스를 생성합니다.
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
