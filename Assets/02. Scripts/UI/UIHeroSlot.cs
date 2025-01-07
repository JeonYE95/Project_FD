using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using GSDatas;
using TMPro;

public class UIHeroSlot : UIBase
{
    [SerializeField] private Button _heroInfoBtn;
    [SerializeField] private TextMeshProUGUI _heroName;
    [SerializeField] private Image _heroIcon;
    [SerializeField] private Slider _levelUpbarSlider;
    [SerializeField] private TextMeshProUGUI _currentPiece;
    [SerializeField] private TextMeshProUGUI _needPiece;
    [SerializeField] private TextMeshProUGUI _unitLevel;



    private UnitData _unitData;
    private UnitEnforceData _unitEnforceData;

    // Start is called before the first frame update
    void Start()
    {
        _heroInfoBtn = GetComponent<Button>();
        _heroInfoBtn.onClick.AddListener(OnHeroInfoButtonClick);
    }


    private void OnHeroInfoButtonClick()
    {
        UIHeroInfo heroInfo = UIManager.Instance.OpenUI<UIHeroInfo>();
        if (heroInfo != null && _unitData != null)
        {
            heroInfo.UpdateInfo(_unitData);
        }
    }

    public void UpdateInfo(UnitData unitData)
    {

        _unitData = unitData;

        if (_heroName != null)
            _heroName.text = unitData.name;

        if (_heroIcon != null)
        {
            Sprite sprite = Resources.Load<Sprite>($"Sprite/Unit/WholeBody/{unitData.grade}/{unitData.name}");
            _heroIcon.sprite = sprite;
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
