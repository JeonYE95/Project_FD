using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : UIBase
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button exitBtn;
 
    private UISelectStage uiSelectStage;
    private UISetting uiSetting;
    private UIExit uiExit;

    private void Start()
    {
        startBtn.onClick.AddListener(() => { OpenSelectStageUI(); });
        settingBtn.onClick.AddListener(() => { OpenSettingUI(); });
        // exitBtn.onClick.AddListener(() => { OpenExitUI(); });
    }

    private void OpenSelectStageUI()
    {
        if (uiSelectStage == null)
            uiSelectStage = UIManager.Instance.GetUI<UISelectStage>();
        
        uiSelectStage.Open();
    }

    private void OpenSettingUI()
    {
        if (uiSetting == null)
            uiSetting = UIManager.Instance.GetUI<UISetting>();
        
        uiSetting.Open();
    }

    // private void OpenExitUI()
    // {
    //     if (uiExit == null)
    //         uiExit = UIManager.Instance.GetUI<UIExit>();
        
    //     uiExit.Open();
    // }
}
