using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GSDatas;
using System.Linq;

public class UIUnits : UIBase
{


    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _unitSlotPrefab;

    [SerializeField] private Button _backBtn;

    [SerializeField] private GameObject _gradeBtnParent;
    [SerializeField] private Button[] _gradeButtons;

    [SerializeField] private UIMainUnitSlot _unitList;

    //현재 켜져있는 인벤토리 등급 확인 
    public Defines.UnitGrade _currentSelectedGrade;

    List<UnitData> _allUnit;


    // Start is called before the first frame update
    void Start()
    {
        _backBtn.onClick.AddListener(() => { UIManager.Instance.CloseUI<UIUnits>(); });

        _allUnit = UnitDataManager.Instance.GetUnitDatas();

        _gradeButtons = _gradeBtnParent.GetComponentsInChildren<Button>();
        Button commonButton = _gradeButtons[(int)Defines.UnitGrade.common];
        Button rareButton = _gradeButtons[(int)Defines.UnitGrade.rare];
        Button uniqueButton = _gradeButtons[(int)Defines.UnitGrade.Unique];

        commonButton.onClick.AddListener(() => OnGradeButtonClick(Defines.UnitGrade.common, commonButton));
        rareButton.onClick.AddListener(() => OnGradeButtonClick(Defines.UnitGrade.rare, rareButton));
        uniqueButton.onClick.AddListener(() => OnGradeButtonClick(Defines.UnitGrade.Unique, uniqueButton));

        OnGradeButtonClick(Defines.UnitGrade.common, commonButton);
    }


    private void OnGradeButtonClick(Defines.UnitGrade units, Button clickedButton)
    {
        UpdateUnitGrade(units);

        foreach (Button btn in _gradeButtons)
        {
            btn.GetComponent<Image>().color = btn.colors.normalColor;
        }

        clickedButton.GetComponent<Image>().color = new Color(80f / 255f, 80f / 255f, 90f / 255f);
    }


    public void UpdateUnitGrade(Defines.UnitGrade unitGrade)
    {

        _currentSelectedGrade = unitGrade; // 현재 등급 인벤토리 저장

        List<UnitData> unitsToShow = new List<UnitData>();

        switch (unitGrade)
        {
            case Defines.UnitGrade.common:
                unitsToShow = _allUnit.Where(unit => unit.grade.Equals("Common")).ToList();
                break;
            case Defines.UnitGrade.rare:
                unitsToShow = _allUnit.Where(unit => unit.grade.Equals("Rare")).ToList();
                break;
            case Defines.UnitGrade.Unique:
                unitsToShow = _allUnit.Where(unit => unit.grade.Equals("Unique")).ToList();
                break;
            default:
                unitsToShow = _allUnit.Where(unit => unit.grade.Equals("Common")).ToList();
                break;
        }


        if (_unitList != null)
        {

            UpdateUnits(unitsToShow);

            //foreach (var unit in unitsToShow)
            //{
            //    int unitcount = UnitHas.ContainsKey(unit._unitData.name) ? UnitHas[unit._unitData.name] : 0;
            //    _unitList.UpdateUnitCount(unit, unitcount);
            //}

        }

    }

    public void UpdateUnits(List<UnitData> units)
    {

        // 기존 content의 모든 자식 오브젝트 제거
        foreach (Transform child in _content.transform)
        {
            Destroy(child.gameObject);
        }

        // 새로운 유닛들로 슬롯 생성
        foreach (var unit in units)
        {
            GameObject go = Instantiate(_unitSlotPrefab, _content.transform);
            UIMainUnitSlot unitSlot = go.GetComponent<UIMainUnitSlot>();
            if (unitSlot != null)
            {
                unitSlot.UpdateInfo(unit);
            }
        }

    }

    private void OnEnable()
    {
        // UI가 활성화될 때마다 현재 선택된 등급의 유닛 데이터를 갱신
        if (_currentSelectedGrade != null && _gradeButtons != null && _gradeButtons.Length > 0)
        {
            UpdateUnitGrade(_currentSelectedGrade);
        }
    }

}