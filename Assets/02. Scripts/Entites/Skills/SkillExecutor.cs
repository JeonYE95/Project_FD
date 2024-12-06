using Assets.HeroEditor.Common.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillExecutor : MonoBehaviour
{
    SkillData _skillData;

    public SkillExecutor(SkillData skillData)
    {
        this._skillData = skillData;
    }

    public UnitSearchOptions CreateSearchOptionsFromSkill()
    {
        return new UnitSearchOptions()
        {
            Group = _skillData.targetGroup,
            Priority = _skillData.targetPriority
        };
    }

    public void ExecuteSkill(BaseUnit _myUnit, SkillData skillData)
    {

        // UnitSearchOptions 생성
        UnitSearchOptions options = CreateSearchOptionsFromSkill();

        // 타겟 검색
        List<BaseUnit> targets = BattleManager.Instance.GetUnits(_myUnit, options);

        // 타겟이 없으면 스킬 중단
        if (targets.Count == 0)
        {
            Debug.Log("타겟이 없습니다. 스킬 실행을 중단합니다.");
            return;
        }

        // 타겟에 스킬 효과 적용
        foreach (var target in targets)
        {
            ApplySkillEffect(_myUnit, target);
        }

        Debug.Log($"{_myUnit.name}이(가) {_skillData.skillName} 스킬을 실행했습니다.");
    }

    private void ApplySkillEffect(BaseUnit caster, BaseUnit target)
    {
        switch (_skillData.effect)
        {
            case SkillEffect.DefenseBoost:
                // 방어력 증가 효과
                target.defense += (int)_skillData.value;
                StartCoroutine(ResetEffectAfterDuration(() => target.defense -= (int)_skillData.value, _skillData.duration));
                break;

            case SkillEffect.HealAmount:
                // 체력 회복 효과
                target.healthSystem.TakeHealth((int)_skillData.value);
                break;

            case SkillEffect.Damage:
                // 데미지 효과
                target.healthSystem.TakeDamage((int)(caster.attackDamage * _skillData.value));
                break;

            default:
                Debug.LogWarning($"알 수 없는 스킬 효과: {_skillData.effect}");
                break;
        }
    }

    private IEnumerator ResetEffectAfterDuration(System.Action resetAction, float duration)
    {
        yield return new WaitForSeconds(duration);
        resetAction?.Invoke();
    }
}
