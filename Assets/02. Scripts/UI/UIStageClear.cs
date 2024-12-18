using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIStageClear : UIBase
{
    [SerializeField] private Button _nextStageBtn;
    [SerializeField] private Button _homeBtn;

    private UIMain _uiMain;
    private UISelectStage _uiSelectStage;

    private void Start()
    {
        _nextStageBtn.onClick.AddListener(() => {  });     // 다음 스테이지 이동 로직 연결
        _homeBtn.onClick.AddListener(() => { LoadMainScene(); });  
    }

    private void OpenMainUI()
    {
        if (_uiMain == null)
            _uiMain = UIManager.Instance.GetUI<UIMain>();
        
        _uiMain.Open();
    }

    private void OpenStageSelectUI()
    {
        if (_uiSelectStage == null)
            _uiSelectStage = UIManager.Instance.GetUI<UISelectStage>();
        
        _uiSelectStage.Open();
    }

    private void LoadMainScene()
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)    
                UIManager.Instance.OpenUI<UIMain>();
                UIManager.Instance.OpenUI<UISelectStage>();
        };

        SceneManager.LoadScene("MainScene");
    }
}
