using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    [SerializeField] private GameObject SpawnPoint; // SpawnPoint 연결
    [SerializeField] private FieldSlot[] _fieldSlots;
    [SerializeField] private FieldSlot _selectedSlot;
    [SerializeField] private UIUnitSlot _unitList;

    [SerializeField] private UIUnitSlotTest _UIUnitSlotTest;

    public enum UnitRarity
    {
        common,
        rare,
        Unique,
    }

    private List<Unit> commonUnit = new List<Unit>();
    private List<Unit> rareUnit = new List<Unit>();
    private List<Unit> uniqueUnit = new List<Unit>();

    public void Start()
    {
        Button normalButton = GetComponentsInChildren<Button>()[(int)UnitRarity.common];
        Button rareButton = GetComponentsInChildren<Button>()[(int)UnitRarity.rare];
        Button uniqueButton = GetComponentsInChildren<Button>()[(int)UnitRarity.Unique];


        normalButton.onClick.AddListener(() => UpdateCharacterSlots(commonUnit));
        rareButton.onClick.AddListener(() => UpdateCharacterSlots(rareUnit));
        uniqueButton.onClick.AddListener(() => UpdateCharacterSlots(uniqueUnit));

        _unitList = GetComponentInChildren<UIUnitSlot>();


        _fieldSlots = SpawnPoint.GetComponentsInChildren<FieldSlot>();
        for (int i = 0; i < _fieldSlots.Length; i++)
        {
            _fieldSlots[i].Init(this, i);

        }

        // 해당 등급의 유닛 데이터로 슬롯 업데이트
        /*
         
        for (int i = 0; i < UnitList.inventoryUnits.Count; i++)
        {
          
            UnitList.inventoryUnits[i] = commonUnit[i];
        }

         */

        //테스트 코드 
        {

            _UIUnitSlotTest = GetComponentInChildren<UIUnitSlotTest>();

            Unit unit1 = gameObject.AddComponent<Unit>();
            commonUnit.Add(unit1);
            Unit unit2 = gameObject.AddComponent<Unit>();
            commonUnit.Add(unit2);
            Unit unit3 = gameObject.AddComponent<Unit>();
            commonUnit.Add(unit3);


            if (_UIUnitSlotTest.InventoryUnits.Count > 0)
            {
                for (int i = 0; i < Mathf.Min(commonUnit.Count, _UIUnitSlotTest.InventoryUnits.Count); i++)
                {

                    _UIUnitSlotTest.InventoryUnits[i] = commonUnit[i];
                }

            }

        }

    }

    public void SwapItem(FieldSlot slot)
    {
        // 필드와 필드끼리 바꾸어야 함. 

        if (_selectedSlot != null)
        {

            Swap(_selectedSlot.Index, slot.Index);
            _selectedSlot = null;

        }
    }
    public void SelectSlot(FieldSlot slot)
    {
        _selectedSlot = slot;

        Debug.Log(_selectedSlot.Index);
    }

    private void Swap(int from, int to)
    {

        // 두 필드 슬롯 캐릭터 교환
        GameObject tempCharacter = _fieldslots[from].Character;
        GameObject toCharacter = _fieldslots[to].Character;

        // fromIndex 슬롯의 캐릭터를 toIndex 슬롯으로 이동
        tempCharacter.transform.SetParent(_fieldslots[to].transform);
        tempCharacter.transform.position = _fieldslots[to].transform.position;

        // toIndex 슬롯의 캐릭터를 fromIndex 슬롯으로 이동
        toCharacter.transform.SetParent(_fieldslots[from].transform);
        toCharacter.transform.position = _fieldslots[from].transform.position;

        _fieldslots[from].SetCharacter(toCharacter);
        _fieldslots[to].SetCharacter(tempCharacter);

    }

    //합성 또는 구매를 통해 캐릭터가 인벤토리에 추가 되었을 때 분류
    public void AddCharacter(Unit character)
    {
        /*
            캐릭터 등급에 따라서 분류 - 캐릭터에서 불러와 처리
           switch (UnitRarity)
        { 
           case common:
                commonUnit.Add(character);
            break;
             case rare:
                rareUnit.Add(character);
            break;
         case Unique:
                uniqueUnit.Add(character);
            break;
        }

        */
    }
    private void UpdateCharacterSlots(List<Unit> units)
    {

        // 해당 등급의 유닛 데이터로 슬롯 업데이트
        /*
         
        for (int i = 0; i < Mathf.Min(units.Count, UnitList.inventoryUnits.Count); i++)
        {
          
            UnitList.inventoryUnits[i] = units[i];
        }
         */

        for (int i = 0; i < Mathf.Min(units.Count, UIUnitSlotTest.InventoryUnits.Count); i++)
        {

            UIUnitSlotTest.InventoryUnits[i] = units[i];
        }

    }

}
