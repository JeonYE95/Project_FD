using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISweep : UIBase
{
    [SerializeField] private Button _closeBtn;
    [SerializeField] private Button _sweepClearBtn;
    [SerializeField] private Button _plusBtn;
    [SerializeField] private Button _minusBtn;
    [SerializeField] private Button _stageStartBtn;

    [SerializeField] private Button _maxBtn;
    [SerializeField] private TextMeshProUGUI _haveEnergyTxt;
    [SerializeField] private TextMeshProUGUI _consumeEnergyTxt;

    [SerializeField] private Image _energySprite;

    private void Start()
    {
        _closeBtn.onClick.AddListener(() => { Close(); });
        _sweepClearBtn.onClick.AddListener(() => { UIManager.Instance.CloseUI<UISweep>(); });
        _stageStartBtn.onClick.AddListener(() => { });
    }

   



}
