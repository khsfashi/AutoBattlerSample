using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���� ���� Ŭ�����Դϴ�.
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
