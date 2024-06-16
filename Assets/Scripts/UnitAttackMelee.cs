using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 근접 공격 유닛 클래스입니다.
/// </summary>
public class UnitAttackMelee : UnitBasement
{
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
            else
            {

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
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.SwordmanAttack, 0.6f);
        base.Attack();
        currentTarget.TakeDamage(GetDamage);
    }
}
