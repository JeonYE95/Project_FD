using UnityEngine.UI;
using UnityEngine;
using GSDatas;
using TMPro;

public class UIMainUnitSlot : UIBase
{
    [SerializeField] private Button _unitInfoBtn;
    [SerializeField] private TextMeshProUGUI _unitName;
    [SerializeField] private Image _unitIcon;
    [SerializeField] private Slider _levelUpbarSlider;
    [SerializeField] private TextMeshProUGUI _currentPiece;
    [SerializeField] private TextMeshProUGUI _needPiece;
    [SerializeField] private TextMeshProUGUI _unitLevel;



    private UnitData _unitData;
    private UnitEnforceData _unitEnforceData;

    // Start is called before the first frame update
    void Start()
    {
        _unitInfoBtn = GetComponent<Button>();
        _unitInfoBtn.onClick.AddListener(OnHeroInfoButtonClick);
    }


    private void OnHeroInfoButtonClick()
    {
        UIUnitInfo unitInfo = UIManager.Instance.OpenUI<UIUnitInfo>();
        if (unitInfo != null && _unitData != null)
        {
            unitInfo.UpdateInfo(_unitData);
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
    }
}
