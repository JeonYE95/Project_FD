using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIStart : UIBase
{
    [SerializeField] private Button _startBtn;

    private UIMain uiMain;

    private void Start()
    {
        _startBtn.onClick.AddListener(() => { LoadMainScene(); });
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
