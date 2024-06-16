using UnityEngine;

/// <summary>
/// 근접 광역 공격 유닛 클래스입니다.
/// </summary>
public class UnitAttackSamurai : UnitBasement
{
    public AxeWind axeWind;
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
        axeWind.Attack();
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.AxegirlAttack, 0.5f);
        base.Attack();
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, range);
        foreach (Collider enemy in enemiesInRange)
        {
            UnitBasement enemyUnit = enemy.GetComponent<UnitBasement>();
            if (enemyUnit != null && enemyUnit.GetTeam() != this.myTeam)
            {
                enemyUnit.TakeDamage(GetDamage);
            }
        }
    }
}
