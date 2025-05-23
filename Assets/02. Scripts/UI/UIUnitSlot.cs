using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GSDatas;

public class UIUnitSlot : MonoBehaviour
{
    private RectTransform _content;

    [SerializeField] private GameObject _unitSlotPrefab;

    private List<UnitData> _inventoryUnits = new List<UnitData>();

    public List<UnitData> InventoryUnits
    {
        get => _inventoryUnits;
        private set => _inventoryUnits = value;
    }

    void Start()
    {
        _content = gameObject.GetComponent<RectTransform>();

        CreateUnitSlots();
    }

    private void CreateUnitSlots()
    {
       
        // 인덱스를 명시적으로 할당하면서 슬롯 생성
        for (int i = 0; i < _inventoryUnits.Count; i++)
        {
            GameObject go = Instantiate(_unitSlotPrefab, _content);
            var unit = _inventoryUnits[i];

            // CharacterSlot의 인덱스 직접 설정
            CharacterSlot characterSlot = go.GetComponent<CharacterSlot>();
            if (characterSlot != null)
            {
                characterSlot.SetIndex(i);  // 인덱스 직접 설정
            }

            TextMeshProUGUI[] texts = go.GetComponentsInChildren<TextMeshProUGUI>();
            TextMeshProUGUI unitNameTxt = texts[0];
            TextMeshProUGUI unitCountTxt = texts[1];

            // UI 요소 설정
            // TextMeshProUGUI unitNameTxt = go.GetComponentInChildren<TextMeshProUGUI>();
            Image unitImg = go.GetComponentInChildren<Image>();

            if (unitNameTxt != null)
            {
                unitNameTxt.text = unit.name;
            }

            if (unitCountTxt != null)
            {
                unitCountTxt.text = $"x{InventoryManager.Instance.GetUnitCount(unit.name)}";
            }

            if (unitImg != null)
            {
                Sprite sprite = Resources.Load<Sprite>($"Sprite/Unit/WholeBody/{unit.grade}/{unit.name}");      
                unitImg.sprite = sprite;
            }
        }

    }

    public void UpdateUnits(List<UnitData> units)
    {
        foreach (Transform child in _content)
        {
            Destroy(child.gameObject);
        }
        // 새로운 유닛 목록으로 업데이트
        _inventoryUnits = units;
        CreateUnitSlots();
    }

    public UnitData GetUnitAtIndex(int index)
    {
        if (index >= 0 && index < _inventoryUnits.Count)
        {
            return _inventoryUnits[index];
        }
        return null;
    }
}
