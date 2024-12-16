using Assets.HeroEditor.Common.Scripts.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillExecutor : MonoBehaviour
{
    ActionHandler _handler;
    UnitSearchOptions _options;
    
    public InGameSkillData gameSkillData;

    private void Awake()
    {
        _handler = GetComponent<ActionHandler>();
    }

    private void Start()
    {
        _options = CreateSearchOptionsFromSkill();
    }

    public UnitSearchOptions CreateSearchOptionsFromSkill()
    {
        return new UnitSearchOptions()
        {
            Group = gameSkillData.targetGroup,
            Priority = gameSkillData.targetPriority
        };
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

        // 타겟에 스킬 효과 적용
        foreach (var target in targets)
        {
            ApplySkillEffect(_myUnit, target);
        }

        Debug.Log($"{gameObject.name}이(가) {gameSkillData.skillName} 스킬을 실행했습니다.");
    }

    private void ApplySkillEffect(BaseUnit caster, BaseUnit target)
    {
        switch (gameSkillData.skillEffect)
        {
            case SkillEffect.DefenseBoost:
                // 방어력 증가 효과
                target.unitInfo.Defense += (int)gameSkillData.value;
                StartCoroutine(ResetEffectAfterDuration(() => target.unitInfo.Defense -= (int)gameSkillData.value, gameSkillData.duration));
                break;

            case SkillEffect.HealAmount:
                // 체력 회복 효과
                target.healthSystem.TakeHealth((int)gameSkillData.value);
                break;

            case SkillEffect.Damage:
                // 데미지 효과
                target.healthSystem.TakeDamage((int)(caster.unitInfo.Attack * gameSkillData.value));
                break;

            case SkillEffect.MultipleAttacks:
                _handler.attackCount = (int)gameSkillData.value;
                StartCoroutine(ResetEffectAfterDuration(() => { _handler.attackCount = 1; }, gameSkillData.duration));
                Debug.Log($"멀티플어택 발동 {(int)gameSkillData.value}");
                break;

            case SkillEffect.AttackBoost:
                var originAttackCollTime = _handler.attackCoolTime;
                _handler.attackCoolTime -= (gameSkillData.value / 100) * originAttackCollTime;
                StartCoroutine(ResetEffectAfterDuration(() => { _handler.attackCoolTime = originAttackCollTime; }, gameSkillData.duration));
                break;

            default:
                Debug.LogWarning($"알 수 없는 스킬 효과: {gameSkillData.skillEffect}");
                break;
        }
    }

    private IEnumerator ResetEffectAfterDuration(Action resetAction, float duration)
    {
        yield return new WaitForSeconds(duration);
        resetAction?.Invoke();
    }
}
