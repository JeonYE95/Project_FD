using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISelectStage : UIBase
{
    [SerializeField] private Button _stageBtn1_1;
    [SerializeField] private Button _stageBtn1_2;
    [SerializeField] private Button _stageBtn1_3;
    [SerializeField] private Button _stageBtn1_4;
    [SerializeField] private Button _stageBtn1_5;
    [SerializeField] private Button _exitBtn;

    private UIInGame _uiInGame;
    private UISelectStage _uiSelectStage;

    void Start()
    {
        _stageBtn1_1.onClick.AddListener(() => { LoadInGameScene(); });
        
        _exitBtn.onClick.AddListener(() => { Close(); });
    }

    private void OpenInGameUI()
    {
        if (_uiInGame == null)
            _uiInGame = UIManager.Instance.GetUI<UIInGame>();
        
        _uiInGame.Open();
    }

    private void LoadInGameScene()
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            // 씬 로드 후 UI 오픈
            if (SceneManager.GetActiveScene().buildIndex == 2)
                UIManager.Instance.OpenUI<UIInGame>();
        };


        SceneManager.LoadScene("InGameBattleScene"); // 씬 로드
    }
}
