using UnityEngine;

public class AxeWind : MonoBehaviour
{
    public Animator animator;

    /// <summary>
    /// ���� ���� ������ ���� ����Ʈ Ʈ���Ÿ� �����մϴ�.
    /// </summary>
    public void Attack()
    {
        if (animator.gameObject.activeSelf)
        {
            animator.SetTrigger("Attack");
        }
    }
}
