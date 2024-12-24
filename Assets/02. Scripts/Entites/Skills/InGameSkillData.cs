using Assets.HeroEditor.Common.Scripts.Common;
using GSDatas;
using System;

[System.Serializable]
public class InGameSkillData
{
    public int unitID;
    public int skillID;                // 스킬 고유 ID
    public string skillName;           // 스킬 이름
    public SkillType skillType;        // 스킬 동작 유형 (Buff, Heal, Damage 등)
    public SkillEffect skillEffect;         // 효과 이름 (DefenseBoost, HealAmount 등)
    public float value;                // 효과 값 (스킬의 강도, 배율, 회복량 등)
    public float duration;             // 지속 시간 (스킬 효과가 유지되는 시간, 초 단위)
    public float skillCoolDown;             // 쿨타임 (스킬 재사용 대기 시간, 초 단위)

    // 타겟팅 관련
    public TargetGroup targetGroup;    // 스킬 타겟 그룹 (Self, Ally, Enemy 등)
    public TargetPriority targetPriority; // 타겟 우선순위 (Closest, Farthest, Random 등)
    public int targetCount;            // 타겟 수 (0 = 전체, n = 최대 타겟 수)

    public void SetInGameSkillData(SkillData skillData)
    {
        if (skillData == null)
        {
            return;
        }

        skillID = skillData.skillID;
        unitID = skillData.UnitID;
        skillName = skillData.skillName;
        skillType = EnumExtensions.ToEnum<SkillType>(skillData.SkillType);
        skillEffect = EnumExtensions.ToEnum<SkillEffect>(skillData.SkillEffect);
        value = skillData.value;
        duration = skillData.duration;
        skillCoolDown = skillData.skillCoolDown;
        targetGroup = EnumExtensions.ToEnum<TargetGroup>(skillData.TargetGroup);
        targetPriority = EnumExtensions.ToEnum<TargetPriority>(skillData.TargetPriority);
        targetCount = skillData.targetCount;
    }

}

public enum SkillType
{
    Buff,       // 능력치 증가
    Heal,       // 체력 회복
    Damage      // 공격 스킬
}

public enum SkillEffect
{
    None,
    LifeSteal,
    SkillValue,
    AttackBoost,     // 공격속도
    DefenseBoost,  // 방어력 증가
    MultipleAttacks,     // 여러번 공격
    BasicAttackMultiplier
}


public enum TargetGroup
{
    Self,
    Target,
    Ally,
    Enemy,
    AllAlly,
    AllEnemy,
}

public enum TargetPriority
{
    Closest,
    Farthest,
    LowestHP,
    Random,
    All
}

public enum SpecialAttack
{
    MultipleAttacks,

}

public static class EnumExtensions
{
    public static T ToEnum<T>(this string value) where T : Enum
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value), "Value cannot be null or empty.");
        }

        return (T)Enum.Parse(typeof(T), value, true); // 대소문자 구분 없이 변환
    }
}



