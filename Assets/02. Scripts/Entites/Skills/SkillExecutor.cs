using Assets.HeroEditor.Common.Scripts.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkillExecutor : MonoBehaviour
{
    ActionHandler _handler;
    UnitSearchOptions _options;
    
    public InGameSkillData inGameSkillData;

    private void Awake()
    {
        _handler = GetComponent<ActionHandler>();

        UnitInfo unitstat = new UnitInfo();
    }
    

    public UnitSearchOptions CreateSearchOptionsFromSkill()
    {
        _options = new UnitSearchOptions()
        {
            Number = inGameSkillData.targetCount,
            Group = inGameSkillData.targetGroup,
            Priority = inGameSkillData.targetPriority
            //인클루트 셀프도 곧 추가
        };

        return _options; // 필요시 반환
    }

    public void ExecuteSkill(BaseUnit _myUnit, InGameSkillData skillData)
    {
        // 타겟 검색
        List<BaseUnit> targets = BattleManager.Instance.GetUnits(_myUnit, _options);

        // 타겟이 없으면 스킬 중단
        if (targets.Count == 0)
        {
            Debug.Log($"{gameObject.name} 의 스킬 사용시 타겟이 없습니다. 스킬 실행을 중단합니다.");
            return;
        }

        //스킬 효과음 출력
        if (skillData.skillType == SkillType.Heal)
        {
            SoundManager.Instance.PlaySFX("Battle/Heal");
        }
        else if (_myUnit is PlayerUnit)
        {
            SoundManager.Instance.PlaySFX("Battle/" + UnitChecker.GetUnitType(_myUnit.unitInfo.ID));
        }
        else if (_myUnit is EnemyUnit)
        {
            SoundManager.Instance.PlaySFX("Battle/monster" + Random.Range(1, 8));
        }

        // 타겟에 스킬 효과 적용
        foreach (var target in targets)
        {
            ApplySkillType(_myUnit, target);
        }

        Debug.Log($"{gameObject.name}이(가) {inGameSkillData.skillName} 스킬을 실행했습니다.");
    }

    private void ApplySkillType(BaseUnit caster, BaseUnit target)
    {
        var visualEffect = BattleManager.Instance.GetSkillEffect(inGameSkillData.skillID);

        /*if (inGameSkillData.skillID == 15)
        {
            Debug.Log("");
        }*/

        if (!visualEffect.haveProjectile)
        {
            PlayVisualEffect(target, visualEffect.targetEffectTag);
        }

        switch (inGameSkillData.skillType)
        {
            case SkillType.Damage:

                if (visualEffect.haveProjectile)
                {
                    GameObject projectile = _handler.CreateEffectProjectile(target, visualEffect);

                    var projectileSprite = projectile.GetComponent<SpriteRenderer>();

                    projectileSprite.sprite = visualEffect.projectileSprite;
                    projectileSprite.color = visualEffect.color;

                }
                else
                {
                    if (inGameSkillData.skillEffect == SkillEffect.SkillValue) // 단순 스킬 데미지
                    {
                        target.healthSystem.TakeDamage((int)inGameSkillData.value);
                    }
                    else if (inGameSkillData.skillEffect == SkillEffect.BasicAttackMultiplier) // 평타 데미지 기반 N배의 데미지
                    {
                        target.healthSystem.TakeDamage((int)(caster.unitInfo.Attack * (float)inGameSkillData.value));
                    }
                }

                break;

            case SkillType.Heal:

                if (inGameSkillData.skillEffect == SkillEffect.SkillValue)
                {
                    target.healthSystem.TakeHealth((int)inGameSkillData.value);
                }

                break;

            case SkillType.Buff:
                ApplySkillEffect(caster, target);
                break;

            default:
                Debug.Log($"{caster.ID} 의 스킬의 SkillType 오류 발생");
                break;
        }
    }

    private void ApplySkillEffect(BaseUnit caster, BaseUnit target)
    {
        float originPropertyValue;

        switch (inGameSkillData.skillEffect)
        {
            case SkillEffect.Stun:

                BattleManager.Instance.ApplyBuff(
                    target,
                    inGameSkillData.skillEffect.ToString(),
                    inGameSkillData.duration,
                    () => SetTargetSten(target),
                    () => SetTargetIdle(target));

                break;

            case SkillEffect.LifeSteal:

                BattleManager.Instance.ApplyBuff(
                    target,
                    inGameSkillData.skillEffect.ToString(),
                    inGameSkillData.duration,
                    () => SetLifeSteal(target, true),
                    () => SetLifeSteal(target, false));

                break;

            case SkillEffect.AttackBoost:

                originPropertyValue = target.unitInfo.AttackCooltime;

                BattleManager.Instance.ApplyBuff(
                    target,
                    inGameSkillData.skillEffect.ToString(),
                    inGameSkillData.duration,
                    () => SetAttackSpeed(target, target.unitInfo.AttackCooltime - (inGameSkillData.value / 100)),
                    () => SetAttackSpeed(target, originPropertyValue));

                break;

            case SkillEffect.DefenseBoost:

                BattleManager.Instance.ApplyBuff(
                    target,
                    inGameSkillData.skillEffect.ToString(),
                    inGameSkillData.duration,
                    () => DefenseBuff(target, inGameSkillData.value),
                    () => DefenseBuff(target, -inGameSkillData.value));

                break;

            case SkillEffect.MultipleAttacks:

                originPropertyValue = target.actionHandler.attackCount;

                BattleManager.Instance.ApplyBuff(
                    target,
                    inGameSkillData.skillEffect.ToString(),
                    inGameSkillData.duration,
                    () => SetAttackCount(target, (int)inGameSkillData.value),
                    () => SetAttackCount(target, (int)originPropertyValue));
                break;

            case SkillEffect.AttackModifier:

                originPropertyValue = target.unitInfo.Attack;

                BattleManager.Instance.ApplyBuff(
                    target,
                    inGameSkillData.skillEffect.ToString(),
                    inGameSkillData.duration,
                    () => SetAttackValue(target, (int)originPropertyValue + (int)inGameSkillData.value),
                    () => SetAttackValue(target, (int)originPropertyValue));
                break;


            default:
                Debug.LogWarning($"알 수 없는 스킬 효과: {inGameSkillData.skillEffect}");
                break;
        }

        Debug.Log($"{caster.ID} 가 {target.ID} 에게 스킬 사용");
    }

    public void PlayVisualEffect(BaseUnit target, string ObjectTag)
    {
        GameObject skillVisualEffect = ObjectPool.Instance.SpawnFromPool(ObjectTag);

        if (skillVisualEffect != null)
        {
            skillVisualEffect.transform.position = target.transform.position;
        }
    }

    public void DefenseBuff(BaseUnit target, float value)
    {
        target.unitInfo.Defense += (int)value;

        if (target.unitInfo.Defense < 0)
        {
            target.unitInfo.Defense = 0;
        }
    }

    public void SetAttackValue(BaseUnit target, int value)
    {
        target.unitInfo.Attack = value;
    }

    public void SetAttackSpeed(BaseUnit target, float value)
    {
        target.unitInfo.AttackCooltime = value;
    }

    public void SetAttackCount(BaseUnit target, int Count)
    {
        target.actionHandler.attackCount = Count;
    }

    public void SetTargetSten(BaseUnit target)
    {
        target.SetStun();
    }

    public void SetTargetIdle(BaseUnit target)
    {
        if (!BattleManager.Instance.IsBattleEnd)
        {
            target.SetIdle();
        }
    }

    public void SetLifeSteal(BaseUnit target, bool value)
    {
        target.actionHandler.isLifeSteal = value;
    }

    public string GetSkillDescription()
    {
        return inGameSkillData.GetSkillDescription();
    }
}
