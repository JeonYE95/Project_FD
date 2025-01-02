using Assets.HeroEditor.Common.Scripts.Common;
using GSDatas;
using System;
using System.Diagnostics;

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
        if (skillData.skillID == 20)
        {
            Debug.WriteLine("");
        }

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

    public string GetSkillDescription()
    {
        string discription = "";

        switch(targetGroup)
        {
            case TargetGroup.Self:
                discription += "자신에게";
                break;

            case TargetGroup.Target:
                discription += "목표에게";
                break;

            case TargetGroup.Enemy:
            case TargetGroup.AllEnemy:
                discription += "다수의 적에게";
                break;


            case TargetGroup.Ally:
            case TargetGroup.AllAlly:
                discription += "아군에게";
                break;
        }

        discription += " ";

        if (skillType == SkillType.Damage)
        {
            discription += "데미지";
        }
        else if (skillType == SkillType.Heal)
        {
            discription += "힐";
        }
        else if (skillType == SkillType.Buff)
        {
            switch(skillEffect)
            {
                case SkillEffect.Stun:
                    discription += "스턴";
                    break;

                case SkillEffect.LifeSteal:
                    discription += "흡혈";
                    break;

                case SkillEffect.AttackBoost:
                    discription += "공격속도";
                    break;

                case SkillEffect.DefenseBoost:
                case SkillEffect.DefenseModifier:
                    discription += "방어력 증감";
                    break;

                case SkillEffect.AttackModifier:
                    discription += "공격력 증감";
                    break;

                case SkillEffect.MultipleAttacks:
                    discription += "멀티플 공격";
                    break;
            }
        }

        discription += " 적용";

        return discription;
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
    Stun,                 // 상태이상 : 스턴
    Shield,               // 쉴드 : 도입 안할 가능성 높음
    Freeze,               // 빙결 : 스턴과 동일하나 충돌방지로 미리추가
    LifeSteal,            // 공격 흡혈
    SkillValue,           // 스킬 데미지
    AttackBoost,          // 공격속도
    DefenseBoost,         // 방어력 증가
    AttackModifier,       // 공격력 조정
    DefenseModifier,      // 방어력 조정 dev 올린후 이걸로 시트 변경
    MultipleAttacks,      // 여러번 공격
    BasicAttackMultiplier // 평타 데미지 기반 스킬 데미지
}

public enum TargetGroup
{
    Self,
    Target,
    Ally,
    Enemy,
    AllAlly,
    AllEnemy
}

public enum TargetPriority
{
    Closest,
    Farthest,
    LowestHP,
    Random,
    All
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



