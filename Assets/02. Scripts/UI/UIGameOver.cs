using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameOver : UIBase
{
    [SerializeField] private Button _restartBtn;
    [SerializeField] private Button _homeBtn;

    private UIMain _uiMain;

    private void Start()
    {
        _restartBtn.onClick.AddListener(() => {  });     // 게임 재시작 로직 연결
        _homeBtn.onClick.AddListener(() => { LoadMainScene(); });  
    }

    private void OpenMainUI()
    {
        if (_uiMain == null)
            _uiMain = UIManager.Instance.GetUI<UIMain>();
        
        _uiMain.Open();
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