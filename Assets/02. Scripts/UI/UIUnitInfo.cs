using UnityEngine;
using UnityEngine.UI;
using GSDatas;
using TMPro;

public class UIUnitInfo : UIBase
{
    [SerializeField] private Button _closeBtn;
    [SerializeField] private Button _levelUpBtn;
    [SerializeField] private TextMeshProUGUI _unitName;
    [SerializeField] private Image _unitIcon;
    [SerializeField] private Slider _levelUpbarSlider;
    [SerializeField] private TextMeshProUGUI _currentPiece;
    [SerializeField] private TextMeshProUGUI _needPiece;
    [SerializeField] private TextMeshProUGUI _unitLevel;
    [SerializeField] private TextMeshProUGUI _health;
    [SerializeField] private TextMeshProUGUI _attack;
    [SerializeField] private TextMeshProUGUI _defense;
    [SerializeField] private TextMeshProUGUI _attackSpeed;
    [SerializeField] private TextMeshProUGUI _attackRange;
    [SerializeField] private TextMeshProUGUI _skillName;
    [SerializeField] private TextMeshProUGUI _skillDescription;

    private UnitData _unitData;

    void Start()
    {
        _closeBtn.onClick.AddListener(() => { UIManager.Instance.CloseUI<UIUnitInfo>(); } );

        //유닛 강화 등록
        _levelUpBtn.onClick.AddListener(Upgrade);
    }


    public void Upgrade()
    {

        UnitUpgrade.UpgradeUnit(_unitData.ID);
        UpdateInfo(_unitData);
        UIUnits unitsUI = UIManager.Instance.OpenUI<UIUnits>();
        if (unitsUI != null)
        {
            unitsUI.UpdateUnitGrade(unitsUI._currentSelectedGrade);
        }
    }


    public void UpdateInfo(UnitData unitData)
    {

        _unitData = unitData;

        if (_unitName != null)
            _unitName.text = unitData.name;

        if (_unitIcon != null)
        {
            Sprite sprite = Resources.Load<Sprite>($"Sprite/Unit/WholeBody/{unitData.grade}/{unitData.name}");
            _unitIcon.sprite = sprite;
        }

        if (_unitLevel != null)
            _unitLevel.text = $"{unitData.level}";

        if (_currentPiece != null)
        {
            _currentPiece.text = GameManager.Instance.GetItemCount(unitData.ID).ToString();

        }

        if (_needPiece != null)
        {
            _needPiece.text = UnitEnforceDataManager.Instance.GetRequriedPieces(unitData.grade, unitData.level).ToString();

        }

        if (_levelUpbarSlider != null)
        {
            int currentPieces = GameManager.Instance.GetItemCount(unitData.ID);
            int requiredPieces = UnitEnforceDataManager.Instance.GetRequriedPieces(unitData.grade, unitData.level);

            float ratio = requiredPieces > 0 ? (float)currentPieces / requiredPieces : 0f;
            _levelUpbarSlider.value = Mathf.Clamp01(ratio);

        }

        if (_skillName != null )
        {
            if (SkillDataManager.Instance.GetSkillByUnitID(unitData.ID) == null)
                _skillName.text = "none";
            else
                _skillName.text = SkillDataManager.Instance.GetSkillByUnitID(unitData.ID).skillName;

        }

        if (_skillDescription != null)
        {
            if (SkillDataManager.Instance.GetSkillByUnitID(unitData.ID) == null)
                _skillDescription.text = "none";
            else 
            _skillDescription.text = SkillDataManager.Instance.GetSkillByUnitID(unitData.ID).SkillDescription;
        }

        _attack.text = unitData.attack.ToString();
        _defense.text = unitData.defense.ToString();
        _health.text = unitData.health.ToString(); 
        _attackRange.text = unitData.range.ToString();
        _attackSpeed.text = unitData.attackCooltime.ToString();
    }



}
