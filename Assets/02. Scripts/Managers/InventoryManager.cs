using System.Collections.Generic;
using UnityEngine;
using GSDatas;
using System.Linq;
using UnityEditor.PackageManager;


public class InventoryManager : Singleton<InventoryManager>
{

    [SerializeField]
    private int _maxSummonUnitCount = 5;

    [SerializeField] private List<FieldSlot> _fieldSlots = new List<FieldSlot>();
    public FieldSlot[] FieldSlots => _fieldSlots.ToArray();

    [SerializeField] private FieldSlot _selectedSlot;
    [SerializeField] private UIUnitSlot _unitList;
    [SerializeField] private BindingGradeButton _characterButton;


    //유닛 인벤토리에 가지고 있는지 확인
    private Dictionary<string, int> UnitHas = new Dictionary<string, int>();

    //필드에 소환되어 있는 유닛 추적 : 필드 번호 / 유닛 정보
    private Dictionary<int, UnitData> _fieldUnitHas = new Dictionary<int, UnitData>();


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


    private List<UnitData> commonUnit = new List<UnitData>();
    private List<UnitData> rareUnit = new List<UnitData>();
    private List<UnitData> uniqueUnit = new List<UnitData>();


    public void Start()
    {


        _unitList = GetComponentInChildren<UIUnitSlot>();
        _characterButton = GetComponentInChildren<BindingGradeButton>();

        // 웨이브 끝날 때마다 유닛 위치 초기화
        WaveManager.Instance.OnClearWave += UnitPosReset;

    }

    // 필드 슬롯 등록
    public void RegisterFieldSlot(FieldSlot fieldSlot)
    {
        if (!_fieldSlots.Contains(fieldSlot))
        {
            _fieldSlots.Add(fieldSlot);
            // 인덱스 재할당
            fieldSlot.SetIndex(_fieldSlots.Count - 1);
        }
    }

    // 해당 등급 리스트에서 데이터 전달, UI 업데이트
    public void UpdateUnitGrade(Defines.UnitGrade unitGrade)
    {

        _currentSelectedGrade = unitGrade; // 현재 등급 인벤토리 저장

        List<UnitData> unitsToShow = new List<UnitData>();

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


        // 오브젝트 풀링으로 추후 수정해야.

        if (_unitList != null)
        {

            _unitList.UpdateUnits(unitsToShow);

            //foreach (var unit in unitsToShow)
            //{
            //    int unitcount = UnitHas.ContainsKey(unit._unitData.name) ? UnitHas[unit._unitData.name] : 0;
            //    _unitList.UpdateUnitCount(unit, unitcount);
            //}

        }



    }

    // 필드 유닛 정보 추가

    public void TrackFieldUnit(int fieldIndex, UnitData unitInfo)
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

        //유닛 상태 초기화
        BattleManager.Instance.ResetAllUnit();

        foreach (FieldSlot characterPos in _fieldSlots)
        {

            if (characterPos.Character != null)
            {
                characterPos.CharacterInit();

            }

        }

    }


    //합성 또는  필드에서 인벤토리로 유닛 추가할 때 분류

    public void AddCharacter(UnitInfo unitName, int amount = 1)
    {
        if (UnitHas.ContainsKey(unitName._unitData.name))
        {
            UnitHas[unitName._unitData.name] += amount;

            Defines.UnitGrade addedUnitGrade = GetUnitGrade(unitName._unitData.grade);
            if (addedUnitGrade == _currentSelectedGrade)
            {
                UpdateUnitGrade(_currentSelectedGrade);
            }
        }
        else
        {
            UnitHas.Add(unitName._unitData.name, amount);


            // UnitData를 깊은 복사하여 저장
            UnitData newData = new UnitData
            {
                ID = unitName._unitData.ID,
                name = unitName._unitData.name,
                attack = unitName._unitData.attack,
                defense = unitName._unitData.defense,
                health = unitName._unitData.health,
                attackCooltime = unitName._unitData.attackCooltime,
                skillCooltime = unitName._unitData.skillCooltime,
                range = unitName._unitData.range,
                grade = unitName._unitData.grade
            };

            Defines.UnitGrade addedUnitGrade = GetUnitGrade(unitName._unitData.grade);

            switch (addedUnitGrade)
            {
                case Defines.UnitGrade.common:
                    commonUnit.Add(newData);
                    break;
                case Defines.UnitGrade.rare:
                    rareUnit.Add(newData);
                    break;
                case Defines.UnitGrade.Unique:
                    uniqueUnit.Add(newData);
                    break;
            }

            // 현재 선택된 등급과 추가된 유닛의 등급이 같다면 UI 업데이트
            if (addedUnitGrade == _currentSelectedGrade)
            {
                UpdateUnitGrade(_currentSelectedGrade);
            }
        }

    }

    //뽑기를 통해 유닛이 인벤토리에 추가 되었을 때 분류
    public void AddCharacterData(UnitData unitData, int amount = 1)
    {
        if (UnitHas.ContainsKey(unitData.name))
        {
            UnitHas[unitData.name] += amount;

            Defines.UnitGrade addedUnitGrade = GetUnitGrade(unitData.grade);
            if (addedUnitGrade == _currentSelectedGrade)
            {
                UpdateUnitGrade(_currentSelectedGrade);
            }
        }
        else
        {
            UnitHas.Add(unitData.name, amount);

            Defines.UnitGrade addedUnitGrade = GetUnitGrade(unitData.grade);

            switch (addedUnitGrade)
            {
                case Defines.UnitGrade.common:
                    commonUnit.Add(unitData);
                    break;
                case Defines.UnitGrade.rare:
                    rareUnit.Add(unitData);
                    break;
                case Defines.UnitGrade.Unique:
                    uniqueUnit.Add(unitData);
                    break;
            }

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
                UnitData unitToRemove = null;

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
            else
            {
                UpdateUnitGrade(_currentSelectedGrade);
            }

            return true;

        }

        return false;
    }


    // 유닛 등급 문자열을 Defines.UnitGrade 변환
    public Defines.UnitGrade GetUnitGrade(string grade)
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

    private bool TryFindAndRemoveUnit(List<UnitData> unitList, string unitName, out UnitData removedUnit)
    {
        removedUnit = unitList.Find(unit => unit.name == unitName);
        if (removedUnit != null)
        {
            unitList.Remove(removedUnit);
            return true;
        }
        return false;
    }


    // 소환 가능 수 비교 
    public bool CanSummonUnit()
    {
        bool canSummon = SummonUnitCount < MaxSummonUnitCount;
        if (!canSummon)
        {
            Debug.Log($"최대 소환 가능 수({MaxSummonUnitCount})에 도달했습니다!");
        }
        return canSummon;
    }

    public int GetUnitCount(string unitName)
    {
        if (UnitHas.ContainsKey(unitName))
        {
            return UnitHas[unitName];
        }

        return 0;
    }

}
