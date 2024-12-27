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

    private Quest _currentQuest;
    public Quest CurrentQuest => _currentQuest;



    private void Start()
    {
        _canvas = GetComponentInParent<Canvas>();
        rewardButton.onClick.AddListener(OnRewardButtonClick); 

    }

    public void SetQuestData(Quest quest)
    {
        _currentQuest = quest;
        UpdateUI();
    }

    private void UpdateUI()
    {
        titleText.text = _currentQuest.questData.description;
        progressText.text = $"{_currentQuest.progress}/{_currentQuest.questData.count}";

        // 진행도 바 업데이트
        float progress = (float)_currentQuest.progress / _currentQuest.questData.count;
        progressBar.fillAmount = progress;

        // 보상 정보 표시
        rewardAmountText.text = _currentQuest.questData.count.ToString();
        // 보상 아이콘은 RewardData에서 가져와서 설정

        // 버튼 상태 및 완료 표시 업데이트
        bool isCompleted = _currentQuest.isCompleted;
        bool hasReceivedReward = GameManager.Instance.playerData.questData[_currentQuest.questData.ID].isCompleted;

        rewardButton.interactable = isCompleted && !hasReceivedReward;
        completeMark.SetActive(hasReceivedReward);
    }

    public void OnRewardButtonClick()
    {
        if (_currentQuest != null && _currentQuest.isCompleted)
        {
            QuestManager.Instance.CheckQuestCompletion(_currentQuest.questData.ID);
            UpdateUI();
        }
    }

}
