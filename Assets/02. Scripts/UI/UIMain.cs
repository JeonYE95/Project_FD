using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : UIBase
{
    [SerializeField] private Button _startBtn;
    [SerializeField] private Button _settingBtn;
    [SerializeField] private Button _unitBtn;
    [SerializeField] private Button _questBtn;
    [SerializeField] private Button _unitGuideBtn;
    [SerializeField] private Button _gachaBtn;
    [SerializeField] private Button _upgradeBtn;
    [SerializeField] private TMP_Text _curEnergyText;
    [SerializeField] private TMP_Text _maxEnergyText;
    [SerializeField] private TMP_Text _diamondTxt;


    private void Start()
    {
        _startBtn.onClick.AddListener(() => { UIManager.Instance.OpenUI<UISelectStage>(); });
        _settingBtn.onClick.AddListener(() => { UIManager.Instance.OpenUI<UISetting>(); });
        _unitBtn.onClick.AddListener(() => { UIManager.Instance.OpenUI<UIUnits>(); });
        _questBtn.onClick.AddListener(() => { UIManager.Instance.OpenUI<UIQuest>(); });
        _unitGuideBtn.onClick.AddListener(() => { UIManager.Instance.OpenUI<UIUnitGuide>();});
        _gachaBtn.onClick.AddListener(() => { UIManager.Instance.OpenUI<UIGacha>(); });
        _upgradeBtn.onClick.AddListener(() => { UIManager.Instance.OpenUI<UIClassUpgrade>(); });

        _maxEnergyText.text = Defines.MAX_ENERGY.ToString();
    }

    private void Update() 
    {
        _curEnergyText.text = GameManager.Instance.playerData.energy.ToString();
        _diamondTxt.text = GameManager.Instance.playerData.diamond.ToString();    
    }
}
