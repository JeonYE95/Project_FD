using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnforceManager : SingletonDontDestory<EnforceManager>
{

    private List<UnitEnforceData> _currentGradeUnitEnforceData;

    //유닛 강화 가능 여부 체크
    public bool CanUnitEnforce(int baseUnitID)
    {
        // 다음 강화 단계에 맞는 강화 정보 찾기
        int currentEnforceUnitLevel = GetCurrentUnitEnforceLevel(baseUnitID);

        // 강화 데이터 조회

        UnitEnforceMaterialData enforceData = EnforceMaterialDataManager.Instance.GetUnitData(baseUnitID);
        if (enforceData == null)
        {
            Debug.Log("존재하지 않는 유닛입니다. 강화 할 수 없습니다");
            return false;
        } 

        // 필요 아이템 보유 여부 확인
        int requireItemID = enforceData.requireItemID;

        UnitData unitInfo = UnitDataManager.Instance.GetUnitData(baseUnitID);

        GetGradeUnitEnforceData(unitInfo.grade);
        int requireCount = _currentGradeUnitEnforceData[currentEnforceUnitLevel].requiredPiece;

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


        int currentEnforceUnitLevel = GetCurrentUnitEnforceLevel(baseUnitID);
        UnitEnforceMaterialData enforceData = EnforceMaterialDataManager.Instance.GetUnitData(currentEnforceUnitLevel);

        //유닛의 필요 아이템 개수 확인
        UnitData unitInfo = UnitDataManager.Instance.GetUnitData(baseUnitID);

        GetGradeUnitEnforceData(unitInfo.grade);
        int requireCount = _currentGradeUnitEnforceData[currentEnforceUnitLevel - 1 ].requiredPiece; 

        //아이템 소모
        GameManager.Instance.substractItemSave(enforceData.requireItemID, requireCount);

        // 강화 정보 저장 
        GameManager.Instance.EnforceUnitSave(baseUnitID);

        Debug.Log($"유닛 {baseUnitID} 강화 성공! (다음 강화 : {currentEnforceUnitLevel + 1})" );
         
        
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

        GetGradeUnitEnforceData(baseData.grade);

        //baseData.range += _currentGradeEnforceData[currentLevel -1].range;
        baseData.attack += _currentGradeUnitEnforceData[currentLevel - 1].attack;
        baseData.health += _currentGradeUnitEnforceData[currentLevel - 1].health;
        baseData.defense += _currentGradeUnitEnforceData[currentLevel - 1].defense;
        //baseData.skillCooltime += _currentGradeEnforceData[currentLevel - 1].skillCooltime;
        //baseData.attackCooltime += _currentGradeEnforceData[currentLevel - 1].attackCooltime;

       
    }

    //클래스 강화 여부 체크
    public bool CanClassEnforce(string baseUnitClass)
    {
        // 현재 클래스 강화 단계에 맞는 강화 비용 찾기
        int currentEnforceClassLevel = GetCurrentClassEnforceLevel(baseUnitClass);
        ClassEnforceData enforceData = ClassEnforceDataManager.Instance.GetClassData(currentEnforceClassLevel);
        
        // 필요 아이템 보유 여부 확인
        int requireCount = enforceData.requiredCost;


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


        int currentEnforceClassLevel = GetCurrentClassEnforceLevel(baseUnitClass);

        ClassEnforceData enforceData = ClassEnforceDataManager.Instance.GetClassData(currentEnforceClassLevel);
        int requireCount = enforceData.requiredCost;

        //골드 소모
        GameManager.Instance.substractItemSave(3002, requireCount);

        //강화 정보 저장 
        GameManager.Instance.EnforceClassSave(baseUnitClass);

        Debug.Log($"유닛 {baseUnitClass} 강화 성공! (다음 강화: {currentEnforceClassLevel + 1})");
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
        if (string.IsNullOrEmpty(baseData.classtype)) return;

        // 현재 강화 레벨로 강화된 유닛 ID 계산
        int currentLevel = GetCurrentClassEnforceLevel(baseData.classtype);
         ClassEnforceData enforceData = ClassEnforceDataManager.Instance.GetClassData(currentLevel);

        if (enforceData == null) return; 


        // 강화된 유닛 데이터 가져오기

        //baseData.range += enforceData.range;
        baseData.attack += enforceData.attack;
        baseData.health += enforceData.health;
        baseData.defense += enforceData.defense;
        //baseData.skillCooltime += _AllClassEnforceData[currentLevel - 1].skillCooltime;
        //baseData.attackCooltime += _AllClassEnforceData[currentLevel - 1].attackCooltime;

    }



    public void GetGradeUnitEnforceData(string grade)
    {
        _currentGradeUnitEnforceData = UnitEnforceData.GetList().Where(data => data.grade == grade).ToList();

    }

    

}
