using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIUnitSlotTest : MonoBehaviour
{

    private RectTransform content;

    [SerializeField] private GameObject unitSlotPrefab;

    [SerializeField]
    private List<UnitInfo> inventoryUnits = new List<UnitInfo>();

    public List<UnitInfo> InventoryUnits
    {
        get => inventoryUnits;
        private set => inventoryUnits = value;
    }
    void Start()
    {
        content = gameObject.GetComponent<RectTransform>();

        // 테스트 코드

        UnitInfo unit1 = gameObject.AddComponent<UnitInfo>();
        unit1.SetData(UnitDataManager.Instance.GetUnitData(1004));
        inventoryUnits.Add(unit1);

        UnitInfo unit2 = gameObject.AddComponent<UnitInfo>();
        unit2.SetData(UnitDataManager.Instance.GetUnitData(1005));
        inventoryUnits.Add(unit2);

        CreateUnitSlots();
    }



    private void CreateUnitSlots()
    {
       
        // 인덱스를 명시적으로 할당하면서 슬롯 생성
        for (int i = 0; i < inventoryUnits.Count; i++)
        {
            GameObject go = Instantiate(unitSlotPrefab, content);
            var unit = inventoryUnits[i];

            // CharacterSlot의 인덱스 직접 설정
            CharacterSlot characterSlot = go.GetComponent<CharacterSlot>();
            if (characterSlot != null)
            {
                characterSlot.SetIndex(i);  // 인덱스 직접 설정
            }

            // UI 요소 설정
            TextMeshProUGUI unitNameTxt = go.GetComponentInChildren<TextMeshProUGUI>();
            Image unitImg = go.GetComponentInChildren<Image>();

            if (unitNameTxt != null)
            {
                unitNameTxt.text = unit._unitData.name;
            }

            if (unitImg != null)
            {
                Sprite sprite = Resources.Load<Sprite>($"Sprite/{unit._unitData.name}");
                unitImg.sprite = sprite;
            }
        }

    }

    public void updateUnits(List<UnitInfo> units)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        // 새로운 유닛 목록으로 업데이트
        inventoryUnits = units;
        CreateUnitSlots();
    }


    public UnitInfo GetUnitAtIndex(int index)
    {
        if (index >= 0 && index < inventoryUnits.Count)
        {
            return inventoryUnits[index];
        }
        return null;
    }

}
