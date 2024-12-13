using UnityEngine;
using UnityEngine.EventSystems;
using GSDatas;

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

                // 깊은 복사를 위한 새로운 GameObject와 UnitInfo 생성
 
          

                // UnitData 깊은 복사
                UnitData newData = new UnitData
                {
                    ID = unitInfo._unitData.ID,
                    name = unitInfo._unitData.name,
                    attack = unitInfo._unitData.attack,
                    defense = unitInfo._unitData.defense,
                    health = unitInfo._unitData.health,
                    attackCooltime = unitInfo._unitData.attackCooltime,
                    skillCooltime = unitInfo._unitData.skillCooltime,
                    range = unitInfo._unitData.range,
                    grade = unitInfo._unitData.grade
                };

                GameObject tempObject = new GameObject();
                UnitInfo tempUnitInfo = tempObject.AddComponent<UnitInfo>();
                tempUnitInfo.SetData(newData);

                // 인벤토리에 유닛 추가
                InventoryManager.Instance.AddCharacter(tempUnitInfo);

                Destroy(tempObject);

                // 필드에서 유닛 제거
                fieldSlot.RemoveCharacter();

            }
        }
    }
}
