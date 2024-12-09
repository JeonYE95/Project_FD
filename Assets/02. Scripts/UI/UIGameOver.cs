using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameOver : UIBase
{
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button homeBtn;

    private UIMain uiMain;

    private void Start()
    {
        restartBtn.onClick.AddListener(() => {  });     // 게임 재시작 로직 연결
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
