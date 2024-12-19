using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceManager : MonoBehaviour
{


    public bool CanEnforce(int unitID)
    {
        // 강화 데이터 조회
        EnforceData enforceData = EnforceDataManager.Instance.GetUnitData(unitID);
        if (enforceData == null) return false;

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

    public void ExecuteEnforce(int unitID)
    {
        if (!CanEnforce(unitID))
        {
            Debug.LogWarning("강화에 필요한 재료가 부족합니다.");
            return;
        }

        EnforceData enforceData = EnforceDataManager.Instance.GetUnitData(unitID);

        //아이템 소모
        GameManager.Instance.substractItemSave(enforceData.requireItemID, enforceData.requireItemCount);

        // 강화 정보 저장 - 기본 유닛 ID로 변환하여 강화 정보 저장
        int baseUnitID = GetBaseUnitID(unitID);
        GameManager.Instance.EnforceUnitSave(baseUnitID);

        Debug.Log($"유닛 {unitID} 강화 성공!");
    }

    
    //강화 안 한 유닛 초기 ID 얻기
    private int GetBaseUnitID(int unitID)
    {
        
        return (unitID / 10) * 10;

    }



}
