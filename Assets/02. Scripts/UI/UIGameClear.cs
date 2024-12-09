using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameClear : UIBase
{
    [SerializeField] private Button nextStageBtn;
    [SerializeField] private Button homeBtn;

    private UIMain uiMain;

    private void Start()
    {
        nextStageBtn.onClick.AddListener(() => {  });     // 다음 스테이지 이동 로직 연결
        homeBtn.onClick.AddListener(() => { LoadMainScene(); });  
    }

    private void OpenMainUI()
    {
        if (uiMain == null)
            uiMain = UIManager.Instance.GetUI<UIMain>();
        
        uiMain.Open();
    }

    private void LoadMainScene()
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)    
                OpenMainUI();
        };

        UIManager.Instance.Clear();
        SceneManager.LoadScene("MainScene");
    }
}
