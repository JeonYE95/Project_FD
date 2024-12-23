using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceManager : SingletonDontDestory<EnforceManager>
{

    //유닛 강화 가능 여부 체크
    public bool CanUnitEnforce(int baseUnitID)
    {
        // 다음 강화 단계에 맞는 강화 ID 정보 찾기
        int EnforceUnitID = GetNextUnitEnforceUnitID(baseUnitID);

        // 강화 데이터 조회

        /*
         
        EnforceMaterialData enforceData = EnforceMaterialDataManager.Instance.GetUnitData(EnforceUnitID);
        if (enforceData == null)
        {
            Debug.Log("강화 최대치에 도달하여 강화 할 수 없습니다");
            return false;
        } 

        // 필요 아이템 보유 여부 확인
        int requireItemID = enforceData.requireItemID;
        int requireCount = enforceData.requireItemCount;

        //아이템 보유량 체크
        if (GameManager.Instance.playerData.items.TryGetValue(requireItemID, out int currentCount))
        {
            return currentCount >= requireCount;
        }

         */


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

        /*
         
        int nextEnforceUnitID = GetNextUnitEnforceUnitID(baseUnitID);
        EnforceMaterialData enforceData = EnforceMaterialDataManager.Instance.GetUnitData(nextEnforceUnitID);

        //아이템 소모
        GameManager.Instance.substractItemSave(enforceData.requireItemID, enforceData.requireItemCount);

        // 강화 정보 저장 
        GameManager.Instance.EnforceUnitSave(baseUnitID);

        Debug.Log($"유닛 {baseUnitID} 강화 성공! (다음 강화 ID: {nextEnforceUnitID})" );
         
         */

    }

    //유닛 강화 ID 정보 찾기
    private int GetNextUnitEnforceUnitID(int baseUnitID)
    {
        // 현재 강화 레벨 확인
        int currentLevel = GetCurrentUnitEnforceLevel(baseUnitID);

        //다음 강화 필요 요구 ID 반환
        return currentLevel + 1;
    }

    //현재 유닛 강화 횟수 확인
    public int GetCurrentUnitEnforceLevel(int baseUnitID)
    {
        //현재 강화 횟수 확인
        if (GameManager.Instance.playerData.UnitEnforce.TryGetValue(baseUnitID, out int level))
        {
            return level;
        }
        return 0; // 강화하지 않은 상태 
    }


    // 유닛의 강화된 스탯 정보 반환
    public UnitData GetEnforcedUnitData(UnitData baseData)
    {
        if (baseData == null) return null;

        // 현재 강화 레벨로 강화된 유닛 ID 계산
        int currentLevel = GetCurrentUnitEnforceLevel(baseData.ID);
        int enforcedUnitID = baseData.ID;


        // 강화된 유닛 데이터 가져오기



        return null;

    }

    //클래스 강화 여부 체크
    public bool CanClassEnforce(string baseUnitClass)
    {
        // 다음 강화 단계에 맞는 강화 ID 정보 찾기
      

       

        // 필요 아이템 보유 여부 확인
       


        //아이템 보유량 체크
        if (GameManager.Instance.playerData.items.TryGetValue(3002, out int currentCount))
        {
            //return currentCount >= requireCount;
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

        int nextEnforceUnitID = GetNextClassEnforceUnit(baseUnitClass);
       

        //골드 소모
        //GameManager.Instance.substractItemSave(3002, );

        // 강화 정보 저장 
        GameManager.Instance.EnforceClassSave(baseUnitClass);

        Debug.Log($"유닛 {baseUnitClass} 강화 성공! (다음 강화 ID: {nextEnforceUnitID})");
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


  


}
