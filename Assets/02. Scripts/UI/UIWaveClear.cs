using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIWaveClear : UIBase
{
    [SerializeField] private Button _nextWaveBtn;
    [SerializeField] private Button _stageBtn;
    [SerializeField] private TMP_Text _gold;

    private UIMain _uiMain;
    private UISelectStage _uiSelectStage;

    private void Start()
    {
        _nextWaveBtn.onClick.AddListener(() => { UIManager.Instance.CloseUI<UIWaveClear>(); });     // 다음 웨이브 이동 로직 연결
        _stageBtn.onClick.AddListener(() => { LoadMainScene(); });  

        _gold.text = WaveManager.Instance._currentWaveGold.ToString();
    }

    private void OpenMainUI()
    {
        if (_uiMain == null)
            _uiMain = UIManager.Instance.GetUI<UIMain>();
        
        _uiMain.Open();
    }

    private void OpenStageSelectUI()
    {
        if (_uiSelectStage == null)
            _uiSelectStage = UIManager.Instance.GetUI<UISelectStage>();
        
        _uiSelectStage.Open();
    }

    private void LoadMainScene()
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)    
                OpenMainUI();
                OpenStageSelectUI();
        };

        UIManager.Instance.Clear();
        SceneManager.LoadScene("MainScene");
    }
}
