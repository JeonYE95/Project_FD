using UnityEngine;
using UnityEngine.EventSystems;

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
                // 인벤토리에 유닛 추가
                InventoryManager.Instance.AddCharacter(unitInfo);

                // 필드에서 유닛 제거
                fieldSlot.RemoveCharacter();

                // 현재 선택된 등급의 인벤토리 UI 업데이트
                InventoryManager.Instance.UpdateUnitGrade(
                    InventoryManager.Instance.GetUnitGrade(unitInfo._unitData.grade)
                );
            }
        }
    }
}
