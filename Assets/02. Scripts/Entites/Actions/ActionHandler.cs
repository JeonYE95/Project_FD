using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : MonoBehaviour
{
    protected float lastActionTime = -Mathf.Infinity;
    public float cooldownTime;

    protected float lastSkillTime = -Mathf.Infinity;
    protected float lastAttackTime = -Mathf.Infinity;

    public float skillCollTime;
    public float attackCoolTime;

    protected BaseUnit myUnit;
    protected BaseUnit targetUnit;

    public Transform firePoint;

    private void Awake()
    {
        myUnit = GetComponent<BaseUnit>();
    }

    private void Start()
    {
        firePoint = transform;
    }

    //일단은 상태에서 공격,스킬중 어떤것을 할건지 결정하는데
    //나중에 여기서 통합해서 그냥 스킬쿨 중이면 공격 나가게 할수도 있는데,
    //스킬은 타겟이 범위 안에 있는지 체크 안할거라서 일단 상태에서

    public bool IsCooldownComplete()
    {
        return Time.time >= lastActionTime + cooldownTime;
    }

    public bool IsAttackCoolTimeComplete()
    {
        return Time.time >= lastActionTime + attackCoolTime;
    }

    public bool IsSkillCoolTimeComplete()
    {
        return Time.time >= lastActionTime + skillCollTime;
    }
    public bool ExecuteAction(BaseUnit targetUnit)
    {
        ResetAttackCoolTime();

        if (targetUnit == null)
        {
            return false;
        }

        if (IsSkillCoolTimeComplete())
        {
            //스킬 사용
            UseSkill();
            ResetSkillCoolTime();
        }
        else
        {
            if (targetUnit.isLive && myUnit.IsTargetInRange())
            {
                DoAttack();
                //평타 공격
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public void ResetCooldown()
    {
        lastActionTime = Time.time;
    }

    public void ResetAttackCoolTime()
    {
        lastAttackTime = Time.time;
    }

    public void ResetSkillCoolTime()
    {
        lastSkillTime = Time.time;
    }

    private void DoAttack()
    {
        if (myUnit.isRangedUnit)
        {
            PerformRangedAttack();
        }
        else
        {
            PerformMeleeAttack();
        }
    }

    private void PerformRangedAttack()
    {
        GameObject attackProjectile = Instantiate(myUnit.attackProjectile, firePoint.position, Quaternion.identity);
        Vector2 direction = (targetUnit.transform.position - firePoint.position).normalized;
        attackProjectile.GetComponent<DefaultProjectile>().Initialize(targetUnit, direction);
    }

    private void PerformMeleeAttack()
    {
        if (targetUnit.TryGetComponent(out HealthSystem healthSystem))
        {
            healthSystem.TakeDamage(myUnit.attackDamage);
        }
    }

    private void UseSkill()
    {
        throw new NotImplementedException();
    }

    
}
