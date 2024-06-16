using UnityEngine;

public class AxeWind : MonoBehaviour
{
    public Animator animator;

    /// <summary>
    /// 광역 근접 유닛의 공격 이펙트 트리거를 제어합니다.
    /// </summary>
    public void Attack()
    {
        if (animator.gameObject.activeSelf)
        {
            animator.SetTrigger("Attack");
        }
    }
}
