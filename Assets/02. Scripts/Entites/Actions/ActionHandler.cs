using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : MonoBehaviour
{
    public int attackCount = 1;

    BaseUnit _myUnit;
    BaseUnit _targetUnit;

    public float skillCoolTime;
    public float attackCoolTime;

    bool _haveSkill = true;
    float _lastSkillTime = -Mathf.Infinity;
    float _lastAttackTime = -Mathf.Infinity;

    public Transform firePoint;
    InGameSkillData skillData;
    SkillExecutor _skillExecutor;
    UnitAnimationController _controller;

    private void Awake()
    {
        _myUnit = GetComponent<BaseUnit>();
        _skillExecutor = GetComponent<SkillExecutor>();
        _controller = GetComponent<UnitAnimationController>();
    }

    private void Start()
    {
        firePoint = transform;

        if (_skillExecutor.gameSkillData == SkillDataManager.GetDefaultSkillData())
        {
            _haveSkill = false;
            //Debug.Log($"{gameObject.name} 스킬 업슴");
        }

        skillCoolTime = _myUnit.skillCooltime;
        attackCoolTime = _myUnit.attackCooltime;
    }

    //일단은 상태에서 공격,스킬중 어떤것을 할건지 결정하는데
    //나중에 여기서 통합해서 그냥 스킬쿨 중이면 공격 나가게 할수도 있는데,
    //스킬은 타겟이 범위 안에 있는지 체크 안할거라서 일단 상태에서

    public bool IsAttackCoolTimeComplete()
    {
        return Time.time >= _lastAttackTime + attackCoolTime;
    }

    public bool IsSkillCoolTimeComplete()
    {
        return Time.time >= _lastSkillTime + skillCoolTime;
    }
    public bool ExecuteAction(BaseUnit targetUnit)
    {
        //스킬이 평타 대신 나가는 거라서 평타 쿨도 초기화
        ResetAttackCoolTime();

        if (targetUnit == null || !_myUnit.isLive)
        {
            return false;
        }

        _myUnit.PlayAttackAnimation();

        //액션핸들러가 들고있는 타겟 변경
        this._targetUnit = targetUnit;

        if (IsSkillCoolTimeComplete() && _haveSkill)
        {
            //스킬 사용
            UseSkill();
            ResetSkillCoolTime();
        }
        else
        {
            if (targetUnit.isLive && _myUnit.IsTargetInRange())
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

    public void ResetAttackCoolTime()
    {
        _lastAttackTime = Time.time;
    }

    public void ResetSkillCoolTime()
    {
        _lastSkillTime = Time.time;
    }

    private void DoAttack()
    {
        for (int i = 0; i < attackCount; i++)
        {
            if (_myUnit.isRangedUnit)
            {
                PerformRangedAttack();
            }
            else
            {
                PerformMeleeAttack();
            }
        }
    }

    private void PerformRangedAttack()
    {
        //GameObject attackProjectile = Instantiate(_myUnit.attackProjectile, firePoint.position, Quaternion.identity);
        GameObject attackProjectile = ObjectPool.Instance.SpawnFromPool("DefaultProjectile", firePoint.position);
        Vector2 direction = (_targetUnit.transform.position - firePoint.position).normalized;
        attackProjectile.GetComponent<DefaultProjectile>().Initialize(_targetUnit, direction);
    }

    private void PerformMeleeAttack()
    {
        if (_targetUnit.TryGetComponent(out HealthSystem healthSystem))
        {
            healthSystem.TakeDamage(_myUnit.attackDamage);
        }
    }

    private void UseSkill()
    {
        _skillExecutor.ExecuteSkill(_myUnit, skillData);
        Debug.Log($"{gameObject.name} 스킬 사용함");
    }

    
}
