using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : UIBase
{
    [SerializeField] private Button _startBtn;
    [SerializeField] private Button _settingBtn;
    [SerializeField] private Button _exitBtn;
 
    private UISelectStage _uiSelectStage;
    private UISetting _uiSetting;
    private UIExit _uiExit;

    private void Start()
    {
        _startBtn.onClick.AddListener(() => { UIManager.Instance.OpenUI<UISelectStage>(); });
        _settingBtn.onClick.AddListener(() => { UIManager.Instance.OpenUI<UISetting>(); });
    }

    private void OpenSelectStageUI()
    {
        if (_uiSelectStage == null)
            _uiSelectStage = UIManager.Instance.GetUI<UISelectStage>();
        
        _uiSelectStage.Open();
    }

    private void OpenSettingUI()
    {
        if (_uiSetting == null)
            _uiSetting = UIManager.Instance.GetUI<UISetting>();
        
        _uiSetting.Open();
    }
}
