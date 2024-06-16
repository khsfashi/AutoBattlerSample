using UnityEngine;

/// <summary>
/// 게임 유닛인 Archer가 공격 시 발사되는 화살입니다.
/// 화살은 지정 목표에 다다르면 파괴됩니다.
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
