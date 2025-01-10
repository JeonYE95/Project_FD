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
    [SerializeField] private Button _sweepBtn;

    [SerializeField] private TextMeshProUGUI _stageNum;
    [SerializeField] private TextMeshProUGUI _stageName;
    [SerializeField] private TextMeshProUGUI _stageGoal1;
    [SerializeField] private TextMeshProUGUI _stageGoal2;
    [SerializeField] private TextMeshProUGUI _stageGoal3;

    [SerializeField] private TextMeshProUGUI _waveNum;
    [SerializeField] private Image _bossSprite;
    [SerializeField] private TextMeshProUGUI _bossName;

    [SerializeField] private GameObject _uiRewardPrefab;
    [SerializeField] private RectTransform _uiRewardRectTransform;


    private List<StageData> _currentStageData;
    private WaveData _waveData;
    private EnemyData _stageBossData;

    private int _stageID;
    private int _waveCount;

    private void Start()
    {
        _closeBtn.onClick.AddListener(() => { Close(); });
        _sweepBtn.onClick.AddListener(() => { UIManager.Instance.OpenUI<UISweep>(); });
        _enterStageBtn.onClick.AddListener(EnterCondition);


        //스테이지 이름, 번호
        _stageName.text = _currentStageData[0].name;
        _stageNum.text = $"{_currentStageData[0].ChapterID} - {_currentStageData[0].StageID}";

        //웨이브 수
        _waveNum.text = _waveData.wave.ToString();
       
        // 몬스터 이미지 
        _stageBossData = EnemyDataManager.Instance.GetEnemyData(_waveData.enemyID);
        _bossSprite.sprite = Resources.Load<Sprite>($"Sprite/Enemy/{_stageBossData.name}");

    }

    private void OnEnable()
    {

        _stageID = GameManager.Instance.StageID;
        _currentStageData = GetCurrentStageData();
        _waveData = GetStageWaveData();
    }


    private void EnterCondition()
    {

        // 입장 필요 에너지 확인
        if (GameManager.Instance.EnterEnergy >= GameManager.Instance.StageID)
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
 
        return WaveData.GetList().Where(data => data.ID == 101).OrderByDescending(data => data.enemyID).First();

    }



}
