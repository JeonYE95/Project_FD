using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VisualEffect", menuName = "Config/SkillVisualEffectPoolConfig")]
public class SkillVisualEffectPoolConfigSO : ScriptableObject
{
    [System.Serializable]
    public class SkillVisualEffectEntry
    {
        public int skillID;             // 스킬 ID
        public string casterEffectTag;  // 캐스터 이펙트 풀 태그
        public string targetEffectTag;  // 타겟 이펙트 풀 태그
    }

    public List<SkillVisualEffectEntry> skillEffects;
}
