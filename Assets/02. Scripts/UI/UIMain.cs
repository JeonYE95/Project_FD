using UnityEngine;
using UnityEngine.UI;

public class UIMain : UIBase
{
    [SerializeField] private Button startBtn;
 
    private UISelectStage uiSelectStage;
    
    private void Start()
    {
        startBtn.onClick.AddListener(() => { OpenSelectStage(); });
    }

    private void OpenSelectStage()
    {
        if (uiSelectStage == null)
            uiSelectStage = UIManager.Instance.GetUI<UISelectStage>();
        
        uiSelectStage.Open();
    }
}
