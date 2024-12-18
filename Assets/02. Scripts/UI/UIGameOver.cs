using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameOver : UIBase
{
    [SerializeField] private Button _restartBtn;
    [SerializeField] private Button _stageBtn;

    private void Start()
    {
        _restartBtn.onClick.AddListener(() => {  });     // 게임 재시작 로직 연결
        _stageBtn.onClick.AddListener(() => { LoadMainScene(); });  
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