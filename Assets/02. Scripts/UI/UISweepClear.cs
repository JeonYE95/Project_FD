using System.Collections.Generic;
using System.Linq;
using GSDatas;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISweepClear : UIBase
{
 
    [SerializeField] private GameObject _uiRewardPrefab;
    [SerializeField] private RectTransform _rectTransform;
    
    private Dictionary<int, int> _stageRewardData = new Dictionary<int, int>();


    private void Start()
    {

        GetStageRewardData();
        SetRewardUI();

    }

    private void OnEnable() 
    {
        
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
            int count = reward.Value ; // 보상 수량 * 이전에 누른 개수
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
