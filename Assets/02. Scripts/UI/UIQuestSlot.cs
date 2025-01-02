using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIQuestSlot : MonoBehaviour
{

    private Canvas _canvas; 

    public TMP_Text titleText;             
    public TMP_Text descriptionText;        
    public TMP_Text progressText;     
    public Image progressBar;          
    public Button rewardButton;         
    public TMP_Text rewardAmountText;       
    public Image rewardIcon;            
    public GameObject completeMark;      

    private QuestBase _currentQuest;
    public QuestBase CurrentQuest => _currentQuest;


    private void Start()
    {
        _canvas = GetComponentInParent<Canvas>();
        rewardButton.onClick.AddListener(OnRewardButtonClick);
        
    }

    public void SetQuestData(QuestBase quest)
    {
        _currentQuest = quest;
        UpdateQuestProgress();
    }

    public void UpdateQuestProgress()
    {
        titleText.text = _currentQuest.questData.name;
        descriptionText.text = _currentQuest.questData.description;

        int progress = _currentQuest.GetProgress();  
        int requireCount = _currentQuest.questData.requireCount;

        progressText.text = $"{progress}/{requireCount}";

        // 진행도 바 업데이트
        float progressBarText = (float)progress / requireCount;
        progressBar.fillAmount = progressBarText;

        // 보상 정보 표시
        rewardAmountText.text = _currentQuest.questData.rewardCount.ToString();
        // 보상 아이콘은 RewardData에서 가져와서 설정

        rewardButton.interactable = _currentQuest.isCompleted;
    }


    // 완료 상태와 보상 버튼 관련 업데이트
    public void UpdateRewardState()
    {
        bool isCompleted = _currentQuest.isCompleted;
        bool hasReceivedReward = false;

        if (GameManager.Instance.playerData.questData.ContainsKey(_currentQuest.questData.ID))
        {
            hasReceivedReward = GameManager.Instance.playerData.questData[_currentQuest.questData.ID].isCompleted;

            QuestManager.Instance.UpdateQuestClearQuest(0);
        }
        else
        {
            QuestManager.Instance.CreateNewQuestSaveData(_currentQuest.questData.ID);
            hasReceivedReward = false;
        }

        rewardButton.interactable = isCompleted && !hasReceivedReward;
        completeMark.SetActive(hasReceivedReward);
    }

    public void OnRewardButtonClick()
    {
        if (_currentQuest != null && _currentQuest.isCompleted)
        {
            QuestManager.Instance.QuestCompletion(_currentQuest.questData.ID);
            UpdateRewardState();
        }
    }

}
