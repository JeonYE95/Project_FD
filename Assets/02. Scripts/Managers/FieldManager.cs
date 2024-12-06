using UnityEngine;
using System.Collections.Generic;
using UnityEditor.PackageManager;

public class FieldManager : Singleton<FieldManager>
{
    [SerializeField] private FieldSlot[] _fieldSlots;

    public bool HasUnitInField(int unitID)      // 유닛ID 필드에서 존재하는지 확인
    {
        foreach (var slot in _fieldSlots)
        {
            if (slot.Character != null)
            {
                var unitInfo = slot.Character.GetComponent<UnitInfo>();
                if (unitInfo != null && unitInfo._unitData.ID == unitID)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void RemoveUnitFromField(int unitID)     //특정 유닛ID 필드에서 제거
    {
        foreach (var slot in _fieldSlots)
        {
            if (slot.Character != null)
            {
                var unitInfo = slot.Character.GetComponent<UnitInfo>();
                if (unitInfo != null && unitInfo._unitData.ID == unitID)
                {
                    slot.RemoveCharacter();
                    return;
                }
            }
        }
    }

    public void AddUnitToField(int unitID)      // 필드에 유닛 생성 배치
    {
        var unitData = UnitDataManager.Instance.GetUnitData(unitID);

        if (unitData == null) return;

        GameObject unitInstance = UnitManager.Instance.CreatePlayerUnit(unitID);

        if (unitInstance == null) return;

        foreach (var slot in _fieldSlots)
        {
            if (slot.Character == null)
            {
                slot.SetCharacter(unitInstance);
                unitInstance.transform.SetParent(slot.transform);
                unitInstance.transform.localPosition = Vector3.zero;
                return;
            }
        }
    }

    public bool CanAddUnitToField()
    {
        foreach (var slot in _fieldSlots)
        {
            if (slot.Character == null) return true;
        }
        return false;
    }

}