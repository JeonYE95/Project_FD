using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{

 
    [SerializeField] private FieldSlot[] _fieldSlots;
    [SerializeField] private FieldSlot _selectedSlot;
    [SerializeField] private UIUnitSlot _unitList;
    [SerializeField] private BindingGradeButton _characterButton;


    //테스트 코드
    [SerializeField] private UIUnitSlotTest _UIUnitSlotTest;


    //유닛 가지고 있는지 확인
    private Dictionary<string, int> UnitHas = new Dictionary<string, int>();


    public GameObject PreviewObject { get; set; }


    private List<UnitInfo> commonUnit = new List<UnitInfo>();
    private List<UnitInfo> rareUnit = new List<UnitInfo>();
    private List<UnitInfo> uniqueUnit = new List<UnitInfo>();



    public void Start()
    {

        
        // 실제 동작 코드
        // _unitList = GetComponentInChildren<UIUnitSlot>();

        _fieldSlots = GetComponentsInChildren<FieldSlot>();

        _characterButton = GetComponentInChildren<BindingGradeButton>();


        //테스트 코드 
        {

            _UIUnitSlotTest = GetComponentInChildren<UIUnitSlotTest>();
            if (_UIUnitSlotTest != null)
            {

                UnitInfo unit1 = gameObject.AddComponent<UnitInfo>();
                unit1._unitData = new GSDatas.UnitData
                {
                    name = "Knight",
                    grade = "Common"
                };
                commonUnit.Add(unit1);
                UnitInfo unit2 = gameObject.AddComponent<UnitInfo>();
                unit2._unitData = new GSDatas.UnitData
                {
                    name = "Maze",
                    grade = "Common"
                };
                commonUnit.Add(unit2);

                UnitInfo unit3 = gameObject.AddComponent<UnitInfo>();
                unit3._unitData = new GSDatas.UnitData
                {
                    name = "Warrior",
                    grade = "Common"
                };
                commonUnit.Add(unit3);


            }


            // 웨이브 끝날 때마다 캐릭터 위치 초기화
            WaveManager.Instance.OnClearWave += CharacterPosReset;

        }
    }


    // 버튼 누르면 해당 등급 리스트에서 데이터 전달 
    public void UpdateUnitGrade(Defines.UnitGrade unitGrade)
    {

        List<UnitInfo> unitsToShow = new List<UnitInfo>();

        switch (unitGrade)
        {
            case Defines.UnitGrade.common :
                unitsToShow =  commonUnit;
                break;
            case Defines.UnitGrade.rare:
                unitsToShow = rareUnit;
                break;
            case Defines.UnitGrade.Unique:
                unitsToShow = uniqueUnit;
                break;
            default:
                break;
        }



        // 실제 동작 코드 
        /*
             
          // 오브젝트 풀링으로 추후 수정해야.
         
         
        if (_unitList != null)
        {

            _unitList.UpdateUnits(unitsToShow);      
            =>  오브젝트 풀링 적용으로 추후 수정해야.
          
        }
        */


        // 테스트 코드
        { 
        if (_UIUnitSlotTest != null)
        {

            _UIUnitSlotTest.updateUnits(unitsToShow);

        }
        
        }


    }

    
    public void SelectSlot(FieldSlot slot)
    {
        _selectedSlot = slot;

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



    //합성 또는 뽑기를 통해 유닛이 인벤토리에 추가 되었을 때 분류

    public void AddCharacter(UnitInfo unitName, int amount = 1)
    {
        if (UnitHas.ContainsKey(unitName._unitData.name))
        {
            UnitHas[unitName._unitData.name] += amount;
    
        }
        else
        {
            UnitHas.Add(unitName._unitData.name, amount);

            switch (unitName._unitData.grade)
            {
                case "Common":
                    commonUnit.Add(unitName);
                    break;
                case "Rare":
                    rareUnit.Add(unitName);
                    break;
                case "Unique":
                    uniqueUnit.Add(unitName);
                    break;

            }

        }
    
    }

    // 유닛 조합 / 필드로 내보낼 때 인벤토리에서 개수 감소
    public bool subtractCharacter(string itemName, int amount = 1)
    {
        if (UnitHas.ContainsKey(itemName) && UnitHas[itemName] >= amount)
        {
            UnitHas[itemName] -= amount;

            // 개수가 0이 되면 딕셔너리에서 제거
            if (UnitHas[itemName] <= 0)
            {
                UnitHas.Remove(itemName);
            }
          
            return true;

        }

        return false;
    }


}
