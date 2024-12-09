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

        bool isFieldUnit1 = FieldManager.Instance.HasUnitInField(unit1ID);
        bool isFieldUnit2 = FieldManager.Instance.HasUnitInField(unit2ID);

        if (isFieldUnit1 && isFieldUnit2)     // 둘 다 필드에 있는 경우
        {
            CombineFieldOnly(unit1ID, unit2ID, combineData);
        }
        else if (isFieldUnit1 && !isFieldUnit2)       // unitId1은 필드, unitId2는 인벤토리
        {
            CombineFieldAndInventory(unit1ID, unit2ID, combineData);
        }
        else if (!isFieldUnit1 && isFieldUnit2)       // unitId2는 필드, unitId1은 인벤토리
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

        InventoryManager.Instance.subtractCharacter(UnitDataManager.Instance.GetUnitData(InventoryUnitID).name, 1);

        var resultUnitInfo = new UnitInfo();
        resultUnitInfo.SetData(UnitDataManager.Instance.GetUnitData(combineData.reuslutUnit));

        if (FieldManager.Instance.CanAddUnitToField())
        {
            FieldManager.Instance.AddUnitToField(combineData.reuslutUnit);
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

        FieldManager.Instance.AddUnitToField(combineData.reuslutUnit);
    }

    private void CombineInvetoryOnly(int unit1ID, int unit2ID, CombineData combineData)
    {
        InventoryManager.Instance.subtractCharacter(UnitDataManager.Instance.GetUnitData(unit1ID).name, 1);
        InventoryManager.Instance.subtractCharacter(UnitDataManager.Instance.GetUnitData(unit2ID).name, 1);

        var resultUnit = new UnitInfo();
        resultUnit.SetData(UnitDataManager.Instance.GetUnitData(combineData.reuslutUnit));

        InventoryManager.Instance.AddCharacter(resultUnit);
    }
}