using UnityEngine;

/// <summary>
/// ���� ������ Archer�� ���� �� �߻�Ǵ� ȭ���Դϴ�.
/// ȭ���� ���� ��ǥ�� �ٴٸ��� �ı��˴ϴ�.
/// </summary>
public class Arrow : MonoBehaviour
{
    public float movementSpeed = 2f;
    private Vector3 targetPos;          

    public void Setup(Vector3 target)
    {
        targetPos = target;
    }

    private void Update()
    {
        Vector3 direction = targetPos - transform.position;
        if (direction.sqrMagnitude <= 0.2f)
        {
            gameObject.SetActive(false);
            return;
        }

        transform.position += direction.normalized * movementSpeed * Time.deltaTime;
    }

}
