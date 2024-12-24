using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : MonoBehaviour
{
    public int attackCount = 1;

    BaseUnit _myUnit;
    BaseUnit _targetUnit;

    //스킬쿨타임 현재는 스킬데이터껄로 씀 2024.12.24
    public float skillCoolTime;
    public float attackCoolTime => _myUnit.unitInfo?.AttackCooltime?? 1f;

    bool _haveSkill = true;
    float _lastSkillTime = -Mathf.Infinity;
    float _lastAttackTime = -Mathf.Infinity;

    //public Vector3 firePoint;
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
        if (_skillExecutor.inGameSkillData == SkillDataManager.GetDefaultSkillData())
        {
            _haveSkill = false;
        }
        else
        {
            skillCoolTime = _skillExecutor.inGameSkillData.skillCoolDown;
        }

        //skillCoolTime = _myUnit.unitInfo.SkillCooltime;
        //attackCoolTime = _myUnit.unitInfo.AttackCooltime;
    }

    private void Update()
    {
        
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

    //원거리 투사체 공격
    private void PerformRangedAttack()
    {
        //프리팹에 빈오브젝트로 FirePoint 추가하고 싶으나 다른 사람 코드에서 첫번째 자식으로
        //쓰기때문에 불가
        Vector3 firePoint = transform.position;
        firePoint += new Vector3(0.15f, 0.35f, 0); // 발사 위치 조정

        GameObject attackProjectileGameObject;

        attackProjectileGameObject = ObjectPool.Instance.SpawnFromPool(_myUnit.unitInfo.Name, firePoint);

        //풀에 없을시 기본 투사체 설정
        if (attackProjectileGameObject == null)
        {
            attackProjectileGameObject = ObjectPool.Instance.SpawnFromPool("DefaultProjectile", firePoint);
        }

        Vector2 direction = (_targetUnit.transform.position - firePoint).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        attackProjectileGameObject.transform.rotation = Quaternion.Euler(0, 0, angle);

        var projectileSpriteRenderer = attackProjectileGameObject.GetComponent<SpriteRenderer>();

        if (_myUnit.transform.position.x < _targetUnit.transform.position.x)
        {
            projectileSpriteRenderer.flipX = true;
        }

        var projectileScript = attackProjectileGameObject.GetComponent<DefaultProjectile>();

        projectileScript.SetProjectile(_targetUnit, direction, _myUnit.unitInfo.Attack);
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
        _skillExecutor.ExecuteSkill(_myUnit, _skillExecutor.inGameSkillData);
        Debug.Log($"{_myUnit.ID} 스킬 사용함");
    }

    
}
