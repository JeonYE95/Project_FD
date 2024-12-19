using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : MonoBehaviour
{
    public int attackCount = 1;

    BaseUnit _myUnit;
    BaseUnit _targetUnit;

    public float skillCoolTime => _myUnit.unitInfo?.SkillCooltime?? 10f;
    public float attackCoolTime => _myUnit.unitInfo?.AttackCooltime?? 1f;

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

        if (_skillExecutor.inGameSkillData == SkillDataManager.GetDefaultSkillData())
        {
            _haveSkill = false;
        }

        //skillCoolTime = _myUnit.unitInfo.SkillCooltime;
        //attackCoolTime = _myUnit.unitInfo.AttackCooltime;
    }

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

        //공격 애니메이션 재생
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
            healthSystem.TakeDamage(_myUnit.unitInfo.Attack);
        }
    }

    private void UseSkill()
    {
        _skillExecutor.ExecuteSkill(_myUnit, skillData);
        Debug.Log($"{_myUnit.ID} 스킬 사용함");
    }

    
}
