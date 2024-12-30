using UnityEngine;
using UnityEngine.UI;

public class UIMain : UIBase
{
    [SerializeField] private Button _startBtn;
    [SerializeField] private Button _settingBtn;
    [SerializeField] private Button _heroesBtn;

    private void Start()
    {
        _startBtn.onClick.AddListener(() => { UIManager.Instance.OpenUI<UISelectStage>(); });
        _settingBtn.onClick.AddListener(() => { UIManager.Instance.OpenUI<UISetting>(); });
        _heroesBtn.onClick.AddListener(() => {UIManager.Instance.OpenUI<UIHeroes>(); });
    }
}
