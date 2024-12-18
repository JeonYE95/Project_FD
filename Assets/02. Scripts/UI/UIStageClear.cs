using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIStageClear : UIBase
{
    [SerializeField] private Button _nextStageBtn;
    [SerializeField] private Button _homeBtn;

    private void Start()
    {
        _nextStageBtn.onClick.AddListener(() => {  });     // 다음 스테이지 이동 로직 연결
        _homeBtn.onClick.AddListener(() => { LoadMainScene(); });  
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
