using GSDatas;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataManager
{
    private static SkillDataManager _instance;
    public static SkillDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SkillDataManager();
                _instance.Initialize();
            }
            return _instance;
        }
    }

    // 유닛 ID를 키로, 연결된 스킬 데이터를 저장
    private Dictionary<int, SkillData> unitSkillMap = new Dictionary<int, SkillData>();
    private bool isInitialized = false;

    // 데이터 초기화
    private void Initialize()
    {
        if (isInitialized)
            return;

        // SkillData에서 데이터 로드
        var skillDataList = SkillData.GetList();
        foreach (var skill in skillDataList)
        {
            if (!unitSkillMap.ContainsKey(skill.UnitID))
            {
                unitSkillMap[skill.UnitID] = skill;
            }
        }

        isInitialized = true;
        //Debug.Log("SkillDataManager 초기화 완료!");
    }

    // 유닛 ID로 스킬 가져오기
    public SkillData GetSkillByUnitID(int unitID)
    {
        if (unitSkillMap.TryGetValue(unitID, out SkillData skillData))
        {
            return skillData;
        }

        Debug.LogWarning($"SkillDataManager: Unit ID {unitID}에 해당하는 스킬이 없습니다.");
        return null;
    }

    // 모든 유닛과 스킬 출력 (디버깅용)
    public void PrintAllUnitSkills()
    {
        foreach (var pair in unitSkillMap)
        {
            Debug.Log($"유닛 ID: {pair.Key}, 스킬 이름: {pair.Value.skillName}, 스킬 타입: {pair.Value.SkillType}");
        }
    }

    // 스킬 데이터 강제 리로드
    public void ReloadSkillData()
    {
        SkillData.Load(true);
        unitSkillMap.Clear();
        Initialize();
        Debug.Log("SkillDataManager: 스킬 데이터가 다시 로드되었습니다!");
    }
}
