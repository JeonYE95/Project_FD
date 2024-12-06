using System.Collections.Generic;
using UnityEngine;
using GSDatas;


public class InventoryManager : Singleton<InventoryManager>
{


    private int _maxSummonUnitCount;


    [SerializeField] private FieldSlot[] _fieldSlots;
    [SerializeField] private FieldSlot _selectedSlot;
    [SerializeField] private UIUnitSlot _unitList;
    [SerializeField] private BindingGradeButton _characterButton;


    //테스트 코드 - 추후 UIUnitSlot으로 합칠 예정
    [SerializeField] private UIUnitSlotTest _UIUnitSlotTest;


    //유닛 인벤토리에 가지고 있는지 확인
    private Dictionary<string, int> UnitHas = new Dictionary<string, int>();

    //필드에 소환되어 있는 유닛 추적 : 필드 번호 / 유닛 정보
    private Dictionary<int, UnitInfo> _fieldUnitHas = new Dictionary<int, UnitInfo>();


    // 필드에 소환되어 있는 유닛 수 
    public int SummonUnitCount => _fieldUnitHas.Count;

    //최대 소환 가능 유닛 수
    public int MaxSummonUnitCount
    {
        get => _maxSummonUnitCount;
        set => _maxSummonUnitCount = value;
    }


    public GameObject PreviewObject { get; set; }

    //현재 켜져있는 인벤토리 등급 확인 
    private Defines.UnitGrade _currentSelectedGrade;

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
                unit1.SetData(UnitDataManager.Instance.GetUnitData(1001));
                commonUnit.Add(unit1);


                UnitInfo unit2 = gameObject.AddComponent<UnitInfo>();
                unit2.SetData(UnitDataManager.Instance.GetUnitData(1002));  // Archer ID
                commonUnit.Add(unit2);


                UnitInfo unit3 = gameObject.AddComponent<UnitInfo>();
                unit3.SetData(UnitDataManager.Instance.GetUnitData(1003));  // Maze ID
                commonUnit.Add(unit3);


            }


            // 웨이브 끝날 때마다 유닛 위치 초기화
            WaveManager.Instance.OnClearWave += UnitPosReset;

        }
    }


    // 해당 등급 리스트에서 데이터 전달, UI 업데이트
    public void UpdateUnitGrade(Defines.UnitGrade unitGrade)
    {

        _currentSelectedGrade = unitGrade; // 현재 등급 인벤토리 저장

        List<UnitInfo> unitsToShow = new List<UnitInfo>();

        switch (unitGrade)
        {
            case Defines.UnitGrade.common:
                unitsToShow = commonUnit;
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

    // 필드 유닛 정보 추가

    public void TrackFieldUnit(int fieldIndex, UnitInfo unitInfo)
    {
        if (_fieldUnitHas.ContainsKey(fieldIndex))
        {
            _fieldUnitHas[fieldIndex] = unitInfo;
        }
        else
        {
            _fieldUnitHas.Add(fieldIndex, unitInfo);
        }
    }


    // 필드 유닛 정보 제거

    public void UntrackFieldUnit(int fieldIndex)
    {
        if (_fieldUnitHas.ContainsKey(fieldIndex))
        {
            _fieldUnitHas.Remove(fieldIndex);
        }
    }



    public void SelectSlot(FieldSlot slot)
    {
        _selectedSlot = slot;

    }


    // 필드 유닛 위치 초기화
    public void UnitPosReset()
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


            Defines.UnitGrade addedUnitGrade = GetUnitGrade(unitName._unitData.grade);

            switch (addedUnitGrade)
            {
                case Defines.UnitGrade.common:
                    commonUnit.Add(unitName);
                    break;
                case Defines.UnitGrade.rare:
                    rareUnit.Add(unitName);
                    break;
                case Defines.UnitGrade.Unique:
                    uniqueUnit.Add(unitName);
                    break;
            }

            // 현재 선택된 등급과 추가된 유닛의 등급이 같다면 UI 업데이트
            if (addedUnitGrade == _currentSelectedGrade)
            {
                UpdateUnitGrade(_currentSelectedGrade);
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

                // 제거할 유닛 찾기
                UnitInfo unitToRemove = null;

                // 각 등급별 리스트에서 해당 유닛 찾기
                switch (_currentSelectedGrade)
                {
                    case Defines.UnitGrade.common:
                        if (TryFindAndRemoveUnit(commonUnit, itemName, out unitToRemove))
                        {
                            UpdateUnitGrade(_currentSelectedGrade);
                        }
                        break;

                    case Defines.UnitGrade.rare:
                        if (TryFindAndRemoveUnit(rareUnit, itemName, out unitToRemove))
                        {
                            UpdateUnitGrade(_currentSelectedGrade);
                        }
                        break;

                    case Defines.UnitGrade.Unique:
                        if (TryFindAndRemoveUnit(uniqueUnit, itemName, out unitToRemove))
                        {
                            UpdateUnitGrade(_currentSelectedGrade);
                        }
                        break;
                }

            }

            return true;

        }

        return false;
    }


    // 유닛 등급 문자열을 Defines.UnitGrade 변환
    private Defines.UnitGrade GetUnitGrade(string grade)
    {
        switch (grade.ToLower())
        {
            case "common":
                return Defines.UnitGrade.common;
            case "rare":
                return Defines.UnitGrade.rare;
            case "unique":
                return Defines.UnitGrade.Unique;
            default:
                return Defines.UnitGrade.common; // 기본값
        }
    }

    private bool TryFindAndRemoveUnit(List<UnitInfo> unitList, string unitName, out UnitInfo removedUnit)
    {
        removedUnit = unitList.Find(unit => unit._unitData.name == unitName);
        if (removedUnit != null)
        {
            unitList.Remove(removedUnit);
            return true;
        }
        return false;
    }

}
