using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceManager : MonoBehaviour
{


    public bool CanEnforce(int baseUnitID)
    {
        // 다음 강화 단계에 맞는 강화 ID 정보 찾기
        int EnforceUnitID = GetNextEnforceUnitID(baseUnitID);

        // 강화 데이터 조회
        EnforceData enforceData = EnforceDataManager.Instance.GetUnitData(EnforceUnitID);
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

        return false;
    }


    //유닛 강화 진행
    public void ExecuteEnforce(int baseUnitID)
    {
        if (!CanEnforce(baseUnitID))
        {
            Debug.LogWarning("강화에 필요한 재료가 부족합니다.");
            return;
        }

        int nextEnforceUnitID = GetNextEnforceUnitID(baseUnitID);
        EnforceData enforceData = EnforceDataManager.Instance.GetUnitData(nextEnforceUnitID);

        //아이템 소모
        GameManager.Instance.substractItemSave(enforceData.requireItemID, enforceData.requireItemCount);

        // 강화 정보 저장 
        GameManager.Instance.EnforceUnitSave(baseUnitID);

        Debug.Log($"유닛 {baseUnitID} 강화 성공! (다음 강화 ID: {nextEnforceUnitID})" );
    }


    //강화 ID 정보 찾기
    private int GetNextEnforceUnitID(int baseUnitID)
    {
        // 현재 강화 레벨 확인
        int currentLevel = GetCurrentEnforceLevel(baseUnitID);

        //다음 강화 필요 요구 ID 반환
        return (baseUnitID * 10) + currentLevel;
    }

    // 현재 강화 횟수 확인
    private int GetCurrentEnforceLevel(int baseUnitID)
    {
        //현재 강화 횟수 확인
        if (GameManager.Instance.playerData.UnitInforce.TryGetValue(baseUnitID, out int level))
        {
            return level;
        }
        return 0; // 강화하지 않은 상태 : 0번 시작
    }

}
