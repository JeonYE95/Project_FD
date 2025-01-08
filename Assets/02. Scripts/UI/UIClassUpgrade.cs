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

    void Start()
    {
        _classButtons = _classBtnParent.GetComponentsInChildren<Button>();
        _targetClassUnitImage = _targetClassUnitParent.GetComponentsInChildren<Image>().Where((image, index) => index % 2 == 1).ToArray();
        _targetClassLevel = _classBtnParent.GetComponentsInChildren<TMP_Text>().Where((text, index) => index % 2 == 1).ToArray();

        InitializeClassButtons();

        _backBtn.onClick.AddListener(() => { UIManager.Instance.CloseUI<UIClassUpgrade>(); });
        _upgradeBtn.onClick.AddListener(() => { _classUpgrade.UpgradeClass(_targetClass); LoadClassLevel(); });

        OnClassButtonClick("Knight", _classButtons[0]);
    }

    // 클래스 버튼 초기화
    private void InitializeClassButtons()
    {
        for (int i = 0; i < _classButtons.Length; i++)
        {
            if (i < _classTypes.Length)
            {
                string classType = _classTypes[i];
                Button button = _classButtons[i];
                _classButtons[i].onClick.AddListener(() => OnClassButtonClick(classType, button));
            }
        }
        LoadClassLevel();
    }

    private void OnClassButtonClick(string classType, Button clickedButton)
    {
        _targetClass = classType; 
        Debug.Log($"클래스 '{_targetClass}' 선택됨");

        foreach (Button btn in _classButtons)
        {
            btn.GetComponent<Image>().color = btn.colors.normalColor;
        }
        clickedButton.GetComponent<Image>().color = new Color(120f / 255f, 120f / 255f, 120f / 255f);

        LoadClassUnit(_targetClass);
        LoadAddedClassValue(_targetClass);
        LoadEtherValue(_targetClass);
    }

    private void LoadEtherValue(string classType)
    {
        _currentEther.text = GameManager.Instance.GetItemCount(3004).ToString();    // 이것도 playerData에서 관리해야 하는 거 아닌지
        _requestEther.text = ClassEnforceDataManager.Instance.GetClassData(GameManager.Instance.playerData.ClassEnforce[classType]).requiredCost.ToString(); 
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
