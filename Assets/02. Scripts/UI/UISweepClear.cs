using System.Collections.Generic;
using System.Linq;
using GSDatas;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISweepClear : UIBase
{
 
    [SerializeField] private GameObject _uiRewardPrefab;
    [SerializeField] private RectTransform _rectTransform;


    private Dictionary<int, int> _stageRewardData = new Dictionary<int, int>();
    private int _consumeEnergy = 1; // 기본값 1로 설정
    private int _stageID;


    private EventTrigger _eventTrigger;
    private EventTrigger.Entry _clickEntry;

    private List<StageData> _currentStageData;

    public void SetStageData(List<StageData> stageData)
    {
        _currentStageData = stageData;
    }

    private void Start()
    {
        _eventTrigger = GetComponent<EventTrigger>();
        _clickEntry = new EventTrigger.Entry();
        _clickEntry.eventID = EventTriggerType.PointerClick;
        _clickEntry.callback.AddListener((data) => { Close(); });
        _eventTrigger.triggers.Add(_clickEntry);

        GetStageRewardData();
        SetRewardUI();



    }

    public void SweepCount(int consumeEnergy)
    {
        _consumeEnergy = consumeEnergy;
  
    }


   
    private void GetStageRewardData()
    {

        _stageRewardData = _currentStageData.ToDictionary(data => data.RewardID, data => data.count);

    }

    private void SetRewardUI()
    {
        foreach (var reward in _stageRewardData)
        {
            int rewardID = reward.Key; // RewardID
            int count = reward.Value * _consumeEnergy; // 보상 수량 * UISweep창에서 누른 개수
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

  
}
