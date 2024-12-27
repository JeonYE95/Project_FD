using UnityEngine;
using UnityEngine.UI;

public class UIMain : UIBase
{
    [SerializeField] private Button _startBtn;
    [SerializeField] private Button _settingBtn;
    [SerializeField] private Button _unitStateBtn;

    private void Start()
    {
        _startBtn.onClick.AddListener(() => { UIManager.Instance.OpenUI<UISelectStage>(); });
        _settingBtn.onClick.AddListener(() => { UIManager.Instance.OpenUI<UISetting>(); });
    }
}
