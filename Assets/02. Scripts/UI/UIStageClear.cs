using System.Collections.Generic;
using System.Linq;
using GSDatas;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIStageClear : UIBase
{
    [SerializeField] private Button _nextStageBtn;
    [SerializeField] private Button _homeBtn;

    [SerializeField] private GameObject _uiRewardPrefab;
    [SerializeField] private RectTransform _rectTransform;
    
    private Dictionary<int, int> _stageRewardData = new Dictionary<int, int>();

    private StageData _nextStageData;
    private int _nextStageID;

    private void Start()
    {

        _homeBtn.onClick.AddListener(() => { LoadMainScene(); });  

        GetStageRewardData();
        SetRewardUI();
    }

    private void OnEnable() 
    {
        _nextStageID = GameManager.Instance.StageID + 1;
        Debug.Log(_nextStageID);

        _nextStageData = GameManager.Instance.TotalStageID.FirstOrDefault(stage => stage.ID == _nextStageID);

        // 다음 스테이지 이동 로직 연결(UIClose 포함)
        _nextStageBtn.onClick.AddListener(() => 
        {
            GameManager.Instance.StageID = _nextStageID;

            // 입장 필요 에너지 확인
            if (GameManager.Instance.EnterEnergy >= _nextStageData.cost)
            {
                GameManager.Instance.EnterEnergy -= _nextStageData.cost;
                QuestManager.Instance.UpdateConsumeQuests(3000, _nextStageData.cost);
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

    private void GetStageRewardData()
    {
        _stageRewardData = StageManager.Instance.CurrentStageData.ToDictionary(data => data.RewardID, data => data.count);
    }

    private void SetRewardUI()
    {
        foreach (var reward in _stageRewardData)
        {
            int rewardID = reward.Key; // RewardID
            int count = reward.Value; // 보상 수량
            string rewardName, framecolor;

            GameObject rewardObject = Instantiate(_uiRewardPrefab, _rectTransform);
            UIReward uiReward = rewardObject.GetComponent<UIReward>();

            switch(rewardID)
            {
                case 3002:
                    rewardName = "Gold";
                    framecolor = "brown";
                    break;
                
                case 3003:
                    rewardName = "Diamond";
                    framecolor = "blue";
                    break;

                case 3004:
                    rewardName = "Ether";
                    framecolor = "purple";
                    break;
                
                default:
                    rewardName = "None";
                    framecolor = "None";
                    break;
            }

            rewardObject.name = rewardName;

            uiReward.Frame.sprite = Resources.Load<Sprite>($"Sprite/Reward/frame_{framecolor}");
            uiReward.Icon.sprite = Resources.Load<Sprite>($"Sprite/Reward/icon_{rewardName}");    
            uiReward.Value.text = count.ToString();  
        }
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
