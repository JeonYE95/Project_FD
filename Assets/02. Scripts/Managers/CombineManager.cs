using GSDatas;
using System.Collections.Generic;
using UnityEngine;

public class CombineManager : Singleton<CombineManager>
{

    public bool CanCombine(int unit1ID, int unit2ID)        // 조합 가능 여부
    {
        return CombineDataManager.Instance.GetCombineResultData(unit1ID, unit2ID) != null;
    }

    public CombineData GetCombineResult(int unitId1, int unitId2)       // 조합 결과 데이터 가져오기
    {
        return CombineDataManager.Instance.GetCombineResultData(unitId1, unitId2);
    }

    public void ExecuteCombine(int unit1ID, int unit2ID)
    {
        var combineData = GetCombineResult(unit1ID, unit2ID);

        if (combineData == null) return;

        bool isInventoryUnit1 = InventoryManager.Instance.HasUnitInInventory(unit1ID);
        bool isInventoryUnit2 = InventoryManager.Instance.HasUnitInInventory(unit2ID);


        // 전투 중일 때는 인벤토리에 있는 유닛들만 조합 가능
        if (WaveManager.Instance.IsRunningWave)
        {
            if (isInventoryUnit1 && isInventoryUnit2)
            {
                CombineInvetoryOnly(unit1ID, unit2ID, combineData);
            }
            else
            {
                Debug.LogWarning("전투 중에는 인벤토리의 유닛들만 조합할 수 있습니다.");
            }
            return;
        }


        bool isFieldUnit1 = FieldManager.Instance.HasUnitInField(unit1ID);
        bool isFieldUnit2 = FieldManager.Instance.HasUnitInField(unit2ID);

        if (!((isFieldUnit1 || isInventoryUnit1) && (isFieldUnit2 || isInventoryUnit2)))
        {
            Debug.LogWarning("조합에 필요한 유닛이 부족합니다.");
            return;
        }


        if (isFieldUnit1 && isFieldUnit2)     // 둘 다 필드에 있는 경우
        {
            CombineFieldOnly(unit1ID, unit2ID, combineData);
        }
        else if (isFieldUnit1 && isInventoryUnit2)       // unitId1은 필드, unitId2는 인벤토리
        {
            CombineFieldAndInventory(unit1ID, unit2ID, combineData);
        }
        else if (isInventoryUnit1 && isFieldUnit2)       // unitId2는 필드, unitId1은 인벤토리
        {
            CombineFieldAndInventory(unit2ID, unit1ID, combineData);
        }
        else        // 둘 다 인벤토리에만 있을 경우
        {
            CombineInvetoryOnly(unit1ID, unit2ID, combineData);
        }

    }

    private void CombineFieldAndInventory(int fieldUnitID, int InventoryUnitID, CombineData combineData)
    {
        FieldManager.Instance.RemoveUnitFromField(fieldUnitID);

        InventoryManager.Instance.subtractCharacter(UnitDataManager.Instance.GetUnitData(InventoryUnitID), 1);

        var resultUnitInfo = new UnitInfo();
        resultUnitInfo.SetData(UnitDataManager.Instance.GetUnitData(combineData.resultUnit));

        if (FieldManager.Instance.CanAddUnitToField())
        {
            FieldManager.Instance.AddUnitToField(combineData.resultUnit);
        }
        else 
        {
            InventoryManager.Instance.AddCharacter(resultUnitInfo);
        }
    }

    private void CombineFieldOnly(int unit1ID, int unit2ID, CombineData combineData)
    {
        FieldManager.Instance.RemoveUnitFromField(unit1ID);
        FieldManager.Instance.RemoveUnitFromField(unit2ID);

        FieldManager.Instance.AddUnitToField(combineData.resultUnit);
    }

    private void CombineInvetoryOnly(int unit1ID, int unit2ID, CombineData combineData)
    {
        InventoryManager.Instance.subtractCharacter(UnitDataManager.Instance.GetUnitData(unit1ID), 1);
        InventoryManager.Instance.subtractCharacter(UnitDataManager.Instance.GetUnitData(unit2ID), 1);

        var resultUnit = new UnitInfo();
        resultUnit.SetData(UnitDataManager.Instance.GetUnitData(combineData.resultUnit));

        InventoryManager.Instance.AddCharacter(resultUnit);
    }
}