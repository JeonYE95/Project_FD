using UnityEngine;
using UnityEngine.UI;
using GSDatas;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class UIStageEnter : UIBase
{
    [SerializeField] private Button _closeBtn;
    [SerializeField] private Button _enterStageBtn;
    [SerializeField] private TextMeshProUGUI _enterStageCost;

    [SerializeField] private Button _sweepBtn;

    [SerializeField] private TextMeshProUGUI _stageNum;
    [SerializeField] private TextMeshProUGUI _stageName;
    [SerializeField] private TextMeshProUGUI _stageGoal1;
    [SerializeField] private Image _clearmark1;
    [SerializeField] private TextMeshProUGUI _stageGoal2;
    [SerializeField] private Image _clearmark2;
    [SerializeField] private TextMeshProUGUI _stageGoal3;
    [SerializeField] private Image _clearmark3;

    [SerializeField] private TextMeshProUGUI _waveNum;
    [SerializeField] private Image _bossSprite;
    [SerializeField] private TextMeshProUGUI _bossName;

    [SerializeField] private GameObject _uiRewardPrefab;
    [SerializeField] private RectTransform _uiRewardRectTransform;

    private List<challengeData> _currentChellengeData;
    private List<StageData> _currentStageData;

    private WaveData _waveData;
    private EnemyData _stageBossData;
    
    private int _stageID;
    private int _waveCount;

    private void Start()
    {
        _closeBtn.onClick.AddListener(OnClose);
        _sweepBtn.onClick.AddListener(() => {
            var sweepUI = UIManager.Instance.OpenUI<UISweep>();
            sweepUI.SetStageData(_currentStageData);
        });
        _enterStageBtn.onClick.AddListener(EnterCondition);


    }

    private void OnEnable()
    {

        _stageID = GameManager.Instance.StageID;
        _currentStageData = GetCurrentStageData();
        _waveData = GetStageWaveData();
        _currentChellengeData = GetChallengeData();
        SetRewardUI();



        //스테이지 이름, 번호
        _stageName.text = _currentStageData[0].name;
        _stageNum.text = $"{_currentStageData[0].ChapterID} - {_currentStageData[0].StageID}";

        //웨이브 수
        _waveNum.text = _waveData.wave.ToString();

        // 몬스터 이미지 
        _stageBossData = EnemyDataManager.Instance.GetEnemyData(_waveData.enemyID);
        _bossSprite.sprite = Resources.Load<Sprite>($"Sprite/Enemy/{_stageBossData.name}");
        _bossName.text = _stageBossData.name;

        //첼린지 텍스트
        _stageGoal1.text = _currentChellengeData[0].Description;
        _stageGoal2.text = _currentChellengeData[1].Description;
        _stageGoal3.text = _currentChellengeData[2].Description;

        // 클리어 했다면 0,0,0,0 / 아직이라면 검정색
        var challenge1State = ChallengeManager.Instance.GetChallengeState(_stageID, 1);
        _clearmark1.color = challenge1State == Defines.StageChallengeClearState.Clear ? new Color(1.0f, 1.0f, 1.0f, 1.0f) : new Color(0.39f, 0.39f, 0.39f, 0.39f);

        var challenge2State = ChallengeManager.Instance.GetChallengeState(_stageID, 2);
        _clearmark2.color = challenge2State == Defines.StageChallengeClearState.Clear ? new Color(1.0f, 1.0f, 1.0f, 1.0f) : new Color(0.39f, 0.39f, 0.39f, 0.39f);

        var challenge3State = ChallengeManager.Instance.GetChallengeState(_stageID, 3);
        _clearmark3.color = challenge3State == Defines.StageChallengeClearState.Clear ? new Color(1.0f, 1.0f, 1.0f, 1.0f) : new Color(0.39f, 0.39f, 0.39f, 0.39f);


        // 스테이지 진입 시 소비 비용
        _enterStageCost.text = _currentStageData[0].cost.ToString();
    }


    private void EnterCondition()
    {

        // 입장 필요 에너지 확인
        if (GameManager.Instance.EnterEnergy >= _currentStageData[0].cost)
        {
            GameManager.Instance.EnterEnergy -= _currentStageData[0].cost;
            QuestManager.Instance.UpdateConsumeQuests(3000, _currentStageData[0].cost);
            LoadInGameScene();
        }
        else
        {
            Debug.Log("입장 필요 에너지가 부족합니다.");
        }

    }

    private void OnClose()
    {
        ClearData();
        Close();
    }


    private void SetRewardUI()
    {
        foreach (var reward in _currentStageData)
        {
            int rewardID = reward.RewardID; // RewardID
            int count = reward.count; // 보상 수량
            string rewardName, framecolor;

            GameObject rewardObject = Instantiate(_uiRewardPrefab, _uiRewardRectTransform);
            UIReward uiReward = rewardObject.GetComponent<UIReward>();

            switch (rewardID)
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

    private void SetStageState()
    {//
     // if (GameManager.Instance.playerData.ChallengeClearData.ContainsKey(_stageID))
     // {
     //     switch (GameManager.Instance.playerData.StageClearData[_stageID].)
     //     {
     //         case Defines.StageChallengeClearState.Clear:
     //            
     //             break;
     //         case Defines.StageChallengeClearState.None:
     //           
     //             break;
     //      
     //     }
     // }
     // else
     // {
     //     Debug.Log($"StageClearData에 키 {_stageID}가 존재하지 않습니다.");
     // }
    }



    private void ClearData()
    {
        // 모든 리스트와 변수 초기화
        _currentChellengeData = null;
        _currentStageData = null;
        _waveData = null;
        _stageBossData = null;
        _stageID = 0;
        _waveCount = 0;

        // UI 리워드 오브젝트들 제거
        foreach (Transform child in _uiRewardRectTransform)
        {
            Destroy(child.gameObject);
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



    private List<StageData> GetCurrentStageData()
    {
        return StageData.GetList().Where(data => data.ID == _stageID).ToList();
    }

    private WaveData GetStageWaveData()
    { 
 
        return WaveData.GetList().Where(data => data.ID == _stageID).OrderByDescending(data => data.enemyID).First();

    }

    private List<challengeData> GetChallengeData()
    {

        return challengeData.GetList().Where(data => data.ID == _stageID).ToList();

    }

}
