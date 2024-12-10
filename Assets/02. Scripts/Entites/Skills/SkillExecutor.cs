using Assets.HeroEditor.Common.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillExecutor : MonoBehaviour
{
    public InGameSkillData _gameSkillData;
    UnitSearchOptions _options;

    private void Start()
    {
        _options = new UnitSearchOptions()
        {
            Group = _gameSkillData.targetGroup,
            Priority = _gameSkillData.targetPriority
        };
    }

    public UnitSearchOptions CreateSearchOptionsFromSkill()
    {
        return new UnitSearchOptions()
        {
            Group = _gameSkillData.targetGroup,
            Priority = _gameSkillData.targetPriority
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

        Debug.Log($"{gameObject.name}이(가) {_gameSkillData.skillName} 스킬을 실행했습니다.");
    }

    private void ApplySkillEffect(BaseUnit caster, BaseUnit target)
    {
        switch (_gameSkillData.skillEffect)
        {
            case SkillEffect.DefenseBoost:
                // 방어력 증가 효과
                target.defense += (int)_gameSkillData.value;
                StartCoroutine(ResetEffectAfterDuration(() => target.defense -= (int)_gameSkillData.value, _gameSkillData.duration));
                break;

            case SkillEffect.HealAmount:
                // 체력 회복 효과
                target.healthSystem.TakeHealth((int)_gameSkillData.value);
                break;

            case SkillEffect.Damage:
                // 데미지 효과
                target.healthSystem.TakeDamage((int)(caster.attackDamage * _gameSkillData.value));
                break;

            default:
                Debug.LogWarning($"알 수 없는 스킬 효과: {_gameSkillData.skillEffect}");
                break;
        }
    }

    private IEnumerator ResetEffectAfterDuration(System.Action resetAction, float duration)
    {
        yield return new WaitForSeconds(duration);
        resetAction?.Invoke();
    }
}
