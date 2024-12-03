using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIStart : UIBase
{
    [SerializeField] private Button startBtn;

    private UIMain uiMain;

    private void Start()
    {
        startBtn.onClick.AddListener(() => { LoadMainScene(); });
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
            if (scene.name == "KYM_MainScene")
                OpenMainUI();
        };

        UIManager.Instance.Clear();
        SceneManager.LoadScene("KYM_MainScene");
    }
}
