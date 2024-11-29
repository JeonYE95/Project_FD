using UnityEngine;

public enum SkillType
{
    Damage,
    Heal,
    Buff,
    Debuff
}


[System.Serializable]
public class SkillData
{
    public string skillName;
    public SkillType skillType;
    public float power; // 스킬 효과 크기
    public float cooldown;
    public int maxTargets; // 타겟 수 (0은 무제한)

    public GameObject skillEffectPrefab; // 스킬 효과(이펙트) 프리팹
}