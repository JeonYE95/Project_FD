using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GSDatas;
using UnityEngine.Rendering;

public class UIUnitGuide : UIBase
{


    [SerializeField] private GameObject _commonGradeContent;
    [SerializeField] private GameObject _rareGradeContent;
    [SerializeField] private GameObject _uniqueGradeContent;

    [SerializeField] private Button _closeBtn;
    [SerializeField] private UIUnitGuideSlot _unitSlotPrefab;
    private bool _isInBattleScene;


    List<UnitData> _allUnit;


    public void Initialize(bool isInBattleScene)
    {
        _isInBattleScene = isInBattleScene;

        if (_isInBattleScene)
            StageManager.Instance.StopGame();
    }


    void Start()
    {
        _closeBtn.onClick.AddListener(() => { Close(); });

        _allUnit = UnitDataManager.Instance.GetUnitDatas();
        UpdateUnits(_allUnit);
    }

    public void UpdateUnits(List<UnitData> units)
    {

        // 새로운 유닛들로 슬롯 생성
        foreach (var unit in units)
        {

            GameObject parentContent = GetContentByClassType(unit.grade);

            GameObject go = Instantiate(_unitSlotPrefab.gameObject, parentContent.transform);
            UIUnitGuideSlot UnitSlot = go.GetComponent<UIUnitGuideSlot>();
            if (UnitSlot != null)
            {
                UnitSlot.UpdateInfo(unit);
            }
        }
    }


    private GameObject GetContentByClassType(string classType)
    {
        switch (classType)
        {
            case "Common":
                return _commonGradeContent;
            case "Rare":
                return _rareGradeContent;
            case "Unique":
                return _uniqueGradeContent;
            default:
                Debug.LogWarning($"Unknown class type: {classType}. Using normal grade content as fallback.");
                return _commonGradeContent;
        }
    }


    public void OnDisable()
    {
        if(StageManager.Instance != null)
        StageManager.Instance.ResumeGame();

        _isInBattleScene = false;
    }
}
