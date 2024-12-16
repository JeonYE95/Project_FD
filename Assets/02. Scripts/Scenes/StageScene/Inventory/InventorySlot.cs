using UnityEngine;
using UnityEngine.EventSystems;
using GSDatas;
using System;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // 필드에서 드래그된 경우
        FieldSlot fieldSlot = eventData.pointerDrag.GetComponent<FieldSlot>();
        if (fieldSlot != null && fieldSlot.Character != null)
        {
            // 필드의 유닛 정보 가져오기
            UnitInfo unitInfo = fieldSlot.Character.GetComponent<UnitInfo>();
            if (unitInfo != null)
            {

                // UnitData 깊은 복사
                UnitData newData = unitInfo._unitData.Clone();

                GameObject tempObject = new GameObject();
                UnitInfo tempUnitInfo = tempObject.AddComponent<UnitInfo>();
                tempUnitInfo.SetData(newData);

                // 인벤토리에 유닛 추가
                InventoryManager.Instance.AddCharacter(tempUnitInfo);

                Destroy(tempObject);

                // 필드에서 유닛 제거
                fieldSlot.RemoveCharacter();

                InventoryManager.Instance.UntrackFieldUnit(fieldSlot.Index);

            }
        }
    }
}
