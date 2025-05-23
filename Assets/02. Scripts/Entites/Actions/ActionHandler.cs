using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ActionHandler : MonoBehaviour
{
    public int attackCount = 1;

    bool _haveSkill = true;
    float _lastSkillTime = -Mathf.Infinity;
    float _lastAttackTime = -Mathf.Infinity;

    Vector3 firePointAdjust = new Vector3(0.15f, 0.35f, 0);

    public bool isLifeSteal = false;

    GameObject _skillBar;
    RectTransform _skillBarRectTransform;

    BaseUnit _myUnit;
    BaseUnit _targetUnit;

    SkillExecutor _skillExecutor;
    UnitAnimationController _controller;

    //스킬쿨타임 현재는 스킬데이터껄로 씀 2024.12.24
    public float skillCoolTime;
    public float attackCoolTime => _myUnit.unitInfo?.AttackCooltime ?? 1f;

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

        if (_haveSkill)
        {
            CreateSkillBar();
        }
    }

    private void CreateSkillBar()
    {
        GameObject skillBarPrefab = Resources.Load<GameObject>("UI/UIUnitSkillBar");

        if (skillBarPrefab == null)
        {
            Debug.Log("스킬바 못찾음");
            return;
        }

        _skillBar = Instantiate(skillBarPrefab, transform);

        UISkillCoolTimeBar skillBarScript = _skillBar.GetComponent<UISkillCoolTimeBar>();

        if (skillBarScript != null)
        {
            skillBarScript.Initialize(this);
        }

        _skillBarRectTransform = _skillBar.GetComponent<RectTransform>();
        _skillBarRectTransform.anchoredPosition = new Vector2(0, 1.31f);

        //UI를 위한 쿨타임 기록
        _lastAttackTime = Time.time;
    }

    public bool IsAttackCoolTimeComplete()
    {
        return Time.time >= _lastAttackTime + attackCoolTime;
    }

    public bool IsSkillCoolTimeComplete()
    {
        return Time.time >= _lastSkillTime + skillCoolTime;
    }

    public float GetSkillCoolTimeMaxValue()
    {
        return skillCoolTime;
    }

    public float GetSkillCoolTimeValue()
    {
        if (BattleManager.Instance.isBattleRunning && _myUnit.isLive)
        {
            return Mathf.Clamp(Time.time - _lastSkillTime, 0, skillCoolTime);
        }
        else
        {
            return 0;
        }
    }



    public bool ExecuteAction(BaseUnit targetUnit)
    {
        //스킬이 평타 대신 나가는 거라서 평타 쿨도 초기화
        ResetAttackCoolTime();

        if (targetUnit == null || !_myUnit.isLive)
        {
            return false;
        }

        //액션핸들러가 들고있는 타겟 변경
        this._targetUnit = targetUnit;

        if (IsSkillCoolTimeComplete() && _haveSkill)
        {
            //액션 애니메이션 재생
            _myUnit.PlayAttackAnimation();

            //스킬 사용
            UseSkill();
            ResetSkillCoolTime();
        }
        else
        {
            if (targetUnit.isLive && _myUnit.IsTargetInRange())
            {
                //액션 애니메이션 재생
                _myUnit.PlayAttackAnimation();

                if (_myUnit is PlayerUnit)
                {
                    SoundManager.Instance.PlaySFX("Battle/" + UnitChecker.GetUnitType(_myUnit.unitInfo.ID));
                }
                else if (_myUnit is EnemyUnit)
                {
                    SoundManager.Instance.PlaySFX("Battle/monster" + Random.Range(1, 8));
                }

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
        CreateProjectile(_targetUnit);
    }

    private void PerformMeleeAttack()
    {
        int damageDealt;

        if (_targetUnit.TryGetComponent(out HealthSystem healthSystem))
        {
            damageDealt = healthSystem.TakeDamage(_myUnit.unitInfo.Attack);

            if (isLifeSteal)
            {
                LifeSteal(damageDealt);
            }
        }
    }

    private void UseSkill()
    {
        _skillExecutor.ExecuteSkill(_myUnit, _skillExecutor.inGameSkillData);
        //Debug.Log($"{_myUnit.ID} 스킬 사용함");
    }

    public void LifeSteal(int damage)
    {
        _myUnit.healthSystem.TakeHealth((int)(damage * 0.3f));
    }

    public GameObject CreateEffectProjectile(BaseUnit targetUnit, SkillVisualEffectEntry skilleffect)
    {
        GameObject projectile = CreateProjectile(targetUnit);

        var DP = projectile.GetComponent<DefaultProjectile>();

        if (skilleffect.targetEffectTag.Length > 0 )
        {
            DP.SetTargetTriggerEffect(skilleffect.targetEffectTag);
        }

        return projectile;
    }

    public GameObject CreateProjectile(BaseUnit targetUnit)
    {
        //프리팹에 빈오브젝트로 FirePoint 추가하고 싶으나 다른 사람 코드에서 첫번째 자식(GetChild(0)으로 
        //동작하는 코드가 있기 때문에 불가
        Vector3 firePoint = transform.position;
        firePoint += firePointAdjust; // 발사 위치 조정

        //GameObject attackProjectileGO;

        GameObject attackProjectileGO = ObjectPool.Instance.SpawnFromPool(Defines.DefaultProejectileTag, firePoint);

        var projectileScript = attackProjectileGO.GetComponent<DefaultProjectile>();
        var projectileSpriteRenderer = attackProjectileGO.GetComponent<SpriteRenderer>();
        var ProjectileData = BattleManager.Instance.GetProjectileSprite(_myUnit.unitInfo.ID);

        projectileSpriteRenderer.sprite = ProjectileData.sprite;
        projectileSpriteRenderer.color = ProjectileData.color;

        Vector2 direction = (targetUnit.transform.position - firePoint).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        attackProjectileGO.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (_myUnit.transform.position.x < targetUnit.transform.position.x)
        {
            projectileSpriteRenderer.flipX = true;
        }

        projectileScript.SetProjectile(targetUnit, direction, _myUnit.unitInfo.Attack);

        return attackProjectileGO;
    }
}
