using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GSDatas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIClassUpgrade : UIBase
{
    private ClassUpgrade _classUpgrade = new ClassUpgrade();

    [SerializeField] private Button _backBtn;
    [SerializeField] private RectTransform _classBtnParent;
    [SerializeField] private RectTransform _targetClassUnitParent;
    private Button[] _classButtons;
    private Image[] _targetClassUnitImage;
    [SerializeField] private Button _upgradeBtn;
    private string _targetClass;

    private TMP_Text[] _targetClassLevel;

    [SerializeField] private TMP_Text _currentEther;
    [SerializeField] private TMP_Text _requestEther;

    [SerializeField] private TMP_Text _addedAttack;
    [SerializeField] private TMP_Text _addedDefense;
    [SerializeField] private TMP_Text _addedHealth;

    private string[] _classTypes = { "Knight", "Archer", "Mage", "Healer", "Rogue", "Warrior" };

    [SerializeField] private Image _unit1;
    [SerializeField] private Image _unit2;
    [SerializeField] private Image _unit3;
    [SerializeField] private Image _unit4;


    void Awake()
    {
        // if (_targetClassUnitParent == null)
        // {
        //     _targetClassUnitParent = transform.Find("EnforceUnit")?.gameObject;
        // }
        // else
        // {
        //     Debug.Log($"_targetClassUnitParent is assigned: {_targetClassUnitParent.name}");
        // }

        // if (_classBtnParent == null)
        // {
        //     _classBtnParent = transform.Find("ClassBtn")?.gameObject;
        // }
        // else
        // {
        //     Debug.Log($"_classBtnParent is assigned: {_classBtnParent.name}");
        // }
    }

    void Start()
    {
        // if (_targetClassUnitParent == null)
        // {
        //     Debug.LogError("_targetClassUnitParent is null. Ensure it's assigned or initialized.");
        // }
        // if (_classBtnParent == null)
        // {
        //     Debug.LogError("_classBtnParent is null. Ensure it's assigned or initialized.");
        // }

       //  _classButtons = _classBtnParent.GetComponentsInChildren<Button>();
        _targetClassUnitImage = _targetClassUnitParent.GetComponentsInChildren<Image>().Where((image, index) => index % 2 == 1).ToArray();
        _targetClassLevel = _classBtnParent.GetComponentsInChildren<TMP_Text>().Where((text, index) => index % 2 == 1).ToArray();
        

        InitializeClassButtons();

        _backBtn.onClick.AddListener(() => { UIManager.Instance.CloseUI<UIClassUpgrade>(); });
        _upgradeBtn.onClick.AddListener(() => { _classUpgrade.UpgradeClass(_targetClass); LoadClassLevel(); });
    }

    // 클래스 버튼 초기화
    private void InitializeClassButtons()
    {
        for (int i = 0; i < _classButtons.Length; i++)
        {
            if (i < _classTypes.Length)
            {
                string classType = _classTypes[i];
                _classButtons[i].onClick.AddListener(() => OnClassButtonClick(classType));
            }
        }
        LoadClassLevel();
    }

    private void OnClassButtonClick(string classType)
    {
        _targetClass = classType; 
        Debug.Log($"클래스 '{_targetClass}' 선택됨");
        LoadClassUnit(_targetClass);
        LoadAddedClassValue(_targetClass);
        LoadEtherValue(_targetClass);
    }

    private void LoadEtherValue(string classType)
    {
        _currentEther.text = GameManager.Instance.GetItemCount(3004).ToString();    // 이것도 playerData에서 관리해야 하는 거 아닌지
        _requestEther.text = ClassEnforceDataManager.Instance.GetClassData(GameManager.Instance.playerData.ClassEnforce[classType]).ToString(); // 안불러와짐
    }

    private void LoadClassLevel()
    {
        for (int i = 0; i < _targetClassLevel.Length; i++)
        {
            _targetClassLevel[i].text = GameManager.Instance.playerData.ClassEnforce[_classTypes[i]].ToString();
        }
    }

    private void LoadClassUnit(string classType)
    {
        List<UnitData> classUnits = UnitDataManager.Instance.GetClassUnits(classType);

        for (int i = 0; i < classUnits.Count; i++)
        {
            _targetClassUnitImage[i].sprite = Resources.Load<Sprite>($"Sprite/Unit/UpperBody/{classUnits[i].grade}/{classUnits[i].name}");
        }
    }

    private void LoadAddedClassValue(string classType)
    {
        _addedAttack.text = GameManager.Instance.playerData.ClassAddedData[classType].AddedAttackValue.ToString();
        _addedDefense.text = GameManager.Instance.playerData.ClassAddedData[classType].AddedDefenseValue.ToString();;
        _addedHealth.text = GameManager.Instance.playerData.ClassAddedData[classType].AddedHealthValue.ToString();;
    }
}
