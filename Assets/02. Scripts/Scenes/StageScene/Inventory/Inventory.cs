using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : Singleton<Inventory>
{

 
    [SerializeField] private FieldSlot[] _fieldSlots;
    [SerializeField] private FieldSlot _selectedSlot;
    [SerializeField] private UIUnitSlot _unitList;

    [SerializeField] private UIUnitSlotTest _UIUnitSlotTest;

    public enum UnitRarity
    {
        common,
        rare,
        Unique,
        Legendary
    }

    private Dictionary<string, int> UnitHas = new Dictionary<string, int>();
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


        _fieldSlots = GetComponentsInChildren<FieldSlot>();
        for (int i = 0; i < _fieldSlots.Length; i++)
        {
            _fieldSlots[i].Init(this, i);

        }

        // 버튼 누르면 해당 등급의 유닛 데이터로 슬롯 업데이트
        /*
         
        for (int i = 0; i < UnitList.inventoryUnits.Count; i++)
        {
            //맨 처음 기본 유닛 개수 
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


        // 웨이브 끝날 때마다 캐릭터 위치 초기화
        WaveManager.Instance.OnClearWave += CharacterPosReset;


    }

    
    public void SelectSlot(FieldSlot slot)
    {
        _selectedSlot = slot;

        Debug.Log(_selectedSlot.Index);
    }


    //합성 또는 뽑기를 통해 캐릭터가 인벤토리에 추가 되었을 때 분류
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

        for (int i = 0; i < Mathf.Min(units.Count, _UIUnitSlotTest.InventoryUnits.Count); i++)
        {

            _UIUnitSlotTest.InventoryUnits[i] = units[i];
        }

    }


 
    public void CharacterPosReset()
    {

        foreach (FieldSlot characterPos in _fieldSlots)
        {

            if (characterPos.Character != null)
            {

                characterPos.CharacterInit();

            }
        
        }
        
    }


    public void AddCharacterInventory(string itemName, int amount = 1)
    {
        if (UnitHas.ContainsKey(itemName))
        {
            UnitHas[itemName] += amount;
            //유닛 타입에 따라서 해당 리스트에 넣기

        }
        else
        {
            UnitHas.Add(itemName, amount);
        }
    
    }

}
