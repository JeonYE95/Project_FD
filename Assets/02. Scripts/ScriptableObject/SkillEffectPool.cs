using UnityEngine;

public static class SkillEffectPool
{
    private static SkillVisualEffectPoolConfigSO skillEffectPoolConfig;

    // Config 로드 메서드
    public static void LoadConfig()
    {
        if (skillEffectPoolConfig == null)
        {
            skillEffectPoolConfig = Resources.Load<SkillVisualEffectPoolConfigSO>("Config/SkillEffectPoolConfig");

            if (skillEffectPoolConfig == null)
            {
                Debug.LogError("SkillEffectPoolConfig not found in Resources/Config!");
            }
        }
    }

    // Config 데이터 접근
    public static SkillVisualEffectPoolConfigSO.SkillVisualEffectEntry GetSkillEffect(int skillID)
    {
        LoadConfig();

        if (skillEffectPoolConfig == null) return null;

        return skillEffectPoolConfig.skillEffects.Find(effect => effect.skillID == skillID);
    }
}
