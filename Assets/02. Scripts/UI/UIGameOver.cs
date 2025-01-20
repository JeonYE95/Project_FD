using System.Linq;
using GSDatas;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameOver : UIBase
{
    [SerializeField] private Button _restartBtn;
    [SerializeField] private Button _stageBtn;

    private int _currentStageID;
    private StageData _currentStageData;

    private void Start()
    {
        _stageBtn.onClick.AddListener(() => { LoadMainScene(); });  
    }

    private void OnEnable() 
    {
        _restartBtn.onClick.AddListener(() => 
        { 
            _currentStageID = GameManager.Instance.StageID;
            _currentStageData = GameManager.Instance.TotalStageID.FirstOrDefault(stage => stage.ID == _currentStageID);

            // 입장 필요 에너지 확인
            if (GameManager.Instance.EnterEnergy >= _currentStageData.cost)
            {
                GameManager.Instance.EnterEnergy -= _currentStageData.cost;
                QuestManager.Instance.UpdateConsumeQuests(3000, _currentStageData.cost);
                LoadInGameScene();
            }
            else
            {
                Debug.Log("입장 필요 에너지가 부족합니다.");
            } 
        });          
    }

    private void LoadMainScene()
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)  
            {  
                UIManager.Instance.OpenUI<UIMain>();
                UIManager.Instance.OpenUI<UISelectStage>();
            }
        };
        
        UIManager.Instance.Clear();
        SceneManager.LoadScene("MainScene");
        SoundManager.Instance.PlayBGM("MainBGM");
    }

    private void LoadInGameScene()
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            // 씬 로드 후 UI 오픈
            if (SceneManager.GetActiveScene().buildIndex == 2)
                UIManager.Instance.OpenUI<UIInGame>();
        };

        UIManager.Instance.Clear();
        SceneManager.LoadScene("InGameBattleScene"); // 씬 로드
        SoundManager.Instance.PlayBGM("BattleBGM");
    }
}