using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIStart : UIBase
{
    [SerializeField] private Button _startBtn;

    private void Start()
    {
        _startBtn.onClick.AddListener(() => { LoadMainScene(); });
    }

    private void LoadMainScene()
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)    
                UIManager.Instance.OpenUI<UIMain>();
        };

        UIManager.Instance.Clear();
        SceneManager.LoadScene("MainScene");
    }
}
