using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 궁수 클래스입니다.
/// </summary>
public class UnitAttackArcher : UnitBasement
{
    public Arrow arrow;

    void Update()
    {
        if (GameManager.Instance.curState != GameState.Fight) return;
        if (!dead)
        {
            if (!HasEnemy)
            {
                FindTarget();
                return;
            }

            if (IsInRange && !moving)
            {
                if (canAttack)
                {
                    animator.speed = attackSpeed;
                    Attack();
                }
            }
            else
            {
                GetInRange();
            }
        }
    }

    protected override void Attack()
    {
        Vector3 targetDir = currentTarget.transform.position - this.transform.position;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        GameObject arrowObject =  PoolManager.Instance.GetArrow(currentTarget.transform.position);
        arrowObject.transform.position = this.transform.position;
        arrowObject.transform.rotation = rotation;


        AudioManager.Instance.PlaySfx(AudioManager.Sfx.ArcherAttack, 0.5f);
        base.Attack();
        currentTarget.TakeDamage(GetDamage);
    }

    protected override float GetUnitToUnitDistance(Vector3 a, Vector3 b) { return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); }
}
