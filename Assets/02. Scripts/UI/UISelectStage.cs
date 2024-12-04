using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISelectStage : UIBase
{
    [SerializeField] private Button stageBtn1_1;
    [SerializeField] private Button stageBtn1_2;
    [SerializeField] private Button stageBtn1_3;
    [SerializeField] private Button stageBtn1_4;
    [SerializeField] private Button stageBtn1_5;
    [SerializeField] private Button exitBtn;

    private UIInGame uiInGame;
    private UISelectStage uiSelectStage;

    void Start()
    {
        stageBtn1_1.onClick.AddListener(() => 
        { 
            LoadInGameScene();
        });
        
        exitBtn.onClick.AddListener(() => { Close(); });
    }

    private void OpenInGameUI()
    {
        if (uiInGame == null)
            uiInGame = UIManager.Instance.GetUI<UIInGame>();
        
        uiInGame.Open();
    }

    private void LoadInGameScene()
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            // 씬 로드 후 UI 오픈
            if (scene.name == "KYM_InGameScene")
                OpenInGameUI();
        };

        UIManager.Instance.Clear();
        SceneManager.LoadScene("KYM_InGameScene"); // 씬 로드
    }
}
