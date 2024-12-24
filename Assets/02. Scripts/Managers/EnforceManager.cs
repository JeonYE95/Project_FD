using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnforceManager : SingletonDontDestory<EnforceManager>
{

    private List<UnitEnforceData> _currentGradeEnforceData;
    private List<ClassEnforceData> _AllClassEnforceData;


    protected override void Awake()
    {
        base.Awake();
        GetClassEnforceData();

    }

    //유닛 강화 가능 여부 체크
    public bool CanUnitEnforce(int baseUnitID)
    {
        // 다음 강화 단계에 맞는 강화 정보 찾기
        int EnforceUnitLevel = GetNextUnitEnforceUnitID(baseUnitID);

        // 강화 데이터 조회

        UnitEnforceMaterialData enforceData = EnforceMaterialDataManager.Instance.GetUnitData(baseUnitID);
        if (enforceData == null)
        {
            Debug.Log("강화 최대치에 도달하여 강화 할 수 없습니다");
            return false;
        } 

        // 필요 아이템 보유 여부 확인
        int requireItemID = enforceData.requireItemID;

        UnitData unitInfo = UnitDataManager.Instance.GetUnitData(baseUnitID);
        Defines.UnitEnforceCost unitGradeCost = GetUnitGrade(unitInfo.grade);

        int requireCount = (int)unitGradeCost * EnforceUnitLevel; // 필요 아이템 수 = 유닛 등급 * 개수  


        //아이템 보유량 체크
        if (GameManager.Instance.playerData.items.TryGetValue(requireItemID, out int currentCount))
        {
            return currentCount >= requireCount;
        }

        

        Debug.LogWarning("강화에 필요한 재료가 부족합니다.");

        return false;
    }


    //유닛 강화 진행
    public void ExecuteUnitEnforce(int baseUnitID)
    {
        if (!CanUnitEnforce(baseUnitID))
        {
            return;
        }

         
        int nextEnforceUnitLevel = GetNextUnitEnforceUnitID(baseUnitID);
        UnitEnforceMaterialData enforceData = EnforceMaterialDataManager.Instance.GetUnitData(nextEnforceUnitLevel);

        //필요 아이템 개수 확인
        UnitData unitInfo = UnitDataManager.Instance.GetUnitData(baseUnitID);
        Defines.UnitEnforceCost unitGradeCost = GetUnitGrade(unitInfo.grade);
        int requireCount = (int)unitGradeCost * nextEnforceUnitLevel;


        //아이템 소모
        GameManager.Instance.substractItemSave(enforceData.requireItemID, requireCount);

        // 강화 정보 저장 
        GameManager.Instance.EnforceUnitSave(baseUnitID);

        Debug.Log($"유닛 {baseUnitID} 강화 성공! (다음 강화 ID: {nextEnforceUnitLevel})" );
         
        
    }

    //유닛 강화 ID 정보 찾기
    private int GetNextUnitEnforceUnitID(int baseUnitID)
    {
        // 현재 강화 레벨 확인
        int currentLevel = GetCurrentUnitEnforceLevel(baseUnitID);

        //다음 강화 레벨 반환
        return currentLevel + 1;
    }

    //현재 유닛 강화 횟수 확인
    private int GetCurrentUnitEnforceLevel(int baseUnitID)
    {
        //현재 강화 횟수 확인
        if (GameManager.Instance.playerData.UnitEnforce.TryGetValue(baseUnitID, out int level))
        {
            return level;
        }
        return 0; // 강화하지 않은 상태 
    }


    // 유닛의 강화된 스탯 정보 반환
    public void GetUnitEnforcedData(UnitData baseData)
    {
        if (baseData == null) return;

        // 현재 강화 레벨로 강화된 유닛 ID 계산
        int currentLevel = GetCurrentUnitEnforceLevel(baseData.ID);
        if (currentLevel == 0) return;

        GetGradeEnforceData(baseData.grade);

        baseData.range += _currentGradeEnforceData[currentLevel -1].range;
        baseData.attack += _currentGradeEnforceData[currentLevel - 1].attack;
        baseData.health += _currentGradeEnforceData[currentLevel - 1].health;
        baseData.defense += _currentGradeEnforceData[currentLevel - 1].defense;
        baseData.skillCooltime += _currentGradeEnforceData[currentLevel - 1].skillCooltime;
        baseData.attackCooltime += _currentGradeEnforceData[currentLevel - 1].attackCooltime;

       
    }

    //클래스 강화 여부 체크
    public bool CanClassEnforce(string baseUnitClass)
    {
        // 다음 클래스 강화 단계에 맞는 강화 정보 찾기
        int EnforceUnitLevel = GetNextClassEnforceUnit(baseUnitClass);

        // 필요 아이템 보유 여부 확인
        int requireCount = Defines.CLASS_ENFORCE_COST + EnforceUnitLevel * 10;


        //아이템 보유량 체크
        if (GameManager.Instance.playerData.items.TryGetValue(3002, out int currentCount))
        {
            return currentCount >= requireCount;
        }


        Debug.LogWarning("강화에 필요한 재화가 부족합니다.");

        return false;
    }

    // 클래스 강화
    public void ExecuteClassEnforce(string baseUnitClass)
    {
        if (!CanClassEnforce(baseUnitClass))
        {
            return;
        }


        int nextEnforceUnitLevel = GetNextClassEnforceUnit(baseUnitClass);
        int requireCount = Defines.CLASS_ENFORCE_COST + nextEnforceUnitLevel * 10;

        //골드 소모
        GameManager.Instance.substractItemSave(3002, requireCount);

        // 강화 정보 저장 
        GameManager.Instance.EnforceClassSave(baseUnitClass);

        Debug.Log($"유닛 {baseUnitClass} 강화 성공! (다음 강화 ID: {nextEnforceUnitLevel})");
    }

    //다음 클래스 강화 확인
    private int GetNextClassEnforceUnit(string baseUnitID)
    {
        // 현재 강화 레벨 확인
        int currentLevel = GetCurrentClassEnforceLevel(baseUnitID);

        //다음 강화 필요 요구 ID 반환
        return currentLevel + 1;
    }

    //현재 클래스 강화 횟수 확인
    public int GetCurrentClassEnforceLevel(string baseUnitID)
    {
        //현재 강화 횟수 확인
        if (GameManager.Instance.playerData.ClassEnforce.TryGetValue(baseUnitID, out int level))
        {
            return level;
        }
        return 0; // 강화하지 않은 상태 
    }



    // 클래스의 강화된 스탯 정보 반환
    public void GetClassEnforcedData(UnitData baseData)
    {
        if (baseData == null) return;

        // 현재 강화 레벨로 강화된 유닛 ID 계산
        //int currentLevel = GetCurrentClassEnforceLevel(baseData.classtype);
        // ClassEnforceData enforceData = ClassEnforceDataManaer.Instance.GetItemData(currentLevel);

        // 강화된 유닛 데이터 가져오기

        //baseData.range += _AllClassEnforceData[currentLevel - 1].range;
        //baseData.attack += _AllClassEnforceData[currentLevel - 1].attack;
        //baseData.health += _AllClassEnforceData[currentLevel - 1].health;
        //baseData.defense += _AllClassEnforceData[currentLevel - 1].defense;
        //baseData.skillCooltime += _AllClassEnforceData[currentLevel - 1].skillCooltime;
        //baseData.attackCooltime += _AllClassEnforceData[currentLevel - 1].attackCooltime;

    }


    // 유닛 등급 문자열을 Defines.UnitEnforceCost 변환
    public Defines.UnitEnforceCost GetUnitGrade(string grade)
    {
        switch (grade.ToLower())
        {
            case "common":
                return Defines.UnitEnforceCost.common;
            case "rare":
                return Defines.UnitEnforceCost.rare;
            case "unique":
                return Defines.UnitEnforceCost.Unique;
            default:
                return Defines.UnitEnforceCost.common; 
        }
    }


    private void GetGradeEnforceData(string grade)
    {
        _currentGradeEnforceData = UnitEnforceData.GetList().Where(data => data.grade == grade).ToList();

    }

    private void GetClassEnforceData()
    {

        _AllClassEnforceData = ClassEnforceData.GetList();

    }

}
