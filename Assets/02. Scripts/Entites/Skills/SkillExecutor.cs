using Assets.HeroEditor.Common.Scripts.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillExecutor : MonoBehaviour
{
    ActionHandler _handler;
    [SerializeField] UnitSearchOptions _options;
    
    public InGameSkillData gameSkillData;

    private void Awake()
    {
        _handler = GetComponent<ActionHandler>();
    }
    

    public UnitSearchOptions CreateSearchOptionsFromSkill()
    {
        _options = new UnitSearchOptions()
        {
            Number = gameSkillData.targetCount,
            Group = gameSkillData.targetGroup,
            Priority = gameSkillData.targetPriority
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

        

        // 타겟에 스킬 효과 적용
        foreach (var target in targets)
        {
            /*if (gameSkillData.unitID == 1001)
            {
                Debug.Log(target.gameObject.name);
            }*/

            ApplySkillType(_myUnit, target);
        }

        Debug.Log($"{gameObject.name}이(가) {gameSkillData.skillName} 스킬을 실행했습니다.");
    }

    private void ApplySkillType(BaseUnit caster, BaseUnit target)
    {
        switch (gameSkillData.skillType)
        {
            case SkillType.Damage:

                break;

            case SkillType.Heal:

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

        switch (gameSkillData.skillEffect)
        {
            case SkillEffect.AttackBoost:

                originPropertyValue = target.unitInfo.AttackCooltime;

                BattleManager.Instance.ApplyBuff(
                    target,
                    gameSkillData.skillEffect.ToString(),
                    gameSkillData.duration,
                    () => AttackSpeedBuff(target, target.unitInfo.AttackCooltime - (gameSkillData.value / 100)),
                    () => AttackSpeedBuff(target, originPropertyValue));

                break;

            case SkillEffect.DefenseBoost:

                BattleManager.Instance.ApplyBuff(
                    target,
                    gameSkillData.skillEffect.ToString(),
                    gameSkillData.duration,
                    () => DefenseBuff(target, gameSkillData.value),
                    () => DefenseBuff(target, -gameSkillData.value));

                break;

            case SkillEffect.MultipleAttacks:
                /*target.actionHandler.attackCount = (int)gameSkillData.value;
                StartCoroutine(ResetEffectAfterDuration(() => { target.actionHandler.attackCount = 1; }, gameSkillData.duration));
                Debug.Log($"멀티플어택 발동 {(int)gameSkillData.value}");*/
                break;
            

            default:
                Debug.LogWarning($"알 수 없는 스킬 효과: {gameSkillData.skillEffect}");
                break;
        }

        Debug.Log($"{caster.ID} 가 {target.ID} 에게 스킬 사용");
    }

    private IEnumerator ResetEffectAfterDuration(Action resetAction, float duration)
    {
        yield return new WaitForSeconds(duration);
        resetAction?.Invoke();
    }

    public void DefenseBuff(BaseUnit target, float value)
    {
        if (value < 0)
        {
            Debug.Log("리셋");
        }

        target.unitInfo.Defense += (int)value;
    }

    public void AttackSpeedBuff(BaseUnit target, float value)
    {
        target.unitInfo.AttackCooltime = value;
    }
}
