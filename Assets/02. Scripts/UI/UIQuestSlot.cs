using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIQuestSlot : MonoBehaviour
{

    private Canvas _canvas; // Canvas 참조 캐싱용 변수 추가

    public TMP_Text titleText;              // 퀘스트 제목
    public TMP_Text descriptionText;        // 퀘스트 설명
    public TMP_Text progressText;           // 진행도 텍스트 (예: 2/3)
    public Image progressBar;           // 진행도 바
    public Button rewardButton;         // 보상 수령 버튼
    public TMP_Text rewardAmountText;       // 보상 수량
    public Image rewardIcon;            // 보상 아이콘
    public GameObject completeMark;      // 완료 표시

    private Quest currentQuest;


    private void Start()
    {

       
        _canvas = GetComponentInParent<Canvas>(); // Start에서 한 번만 가져오기


    }

    public void SetQuestData(Quest quest)
    {
        currentQuest = quest;
        UpdateUI();
    }

    private void UpdateUI()
    {
        titleText.text = currentQuest.questData.description;
        progressText.text = $"{currentQuest.progress}/{currentQuest.questData.count}";

        // 진행도 바 업데이트
        float progress = (float)currentQuest.progress / currentQuest.questData.count;
        progressBar.fillAmount = progress;

        // 보상 정보 표시
        rewardAmountText.text = currentQuest.questData.count.ToString();
        // 보상 아이콘은 RewardData에서 가져와서 설정

        // 버튼 상태 및 완료 표시 업데이트
        bool isCompleted = currentQuest.isCompleted;
        bool hasReceivedReward = GameManager.Instance.playerData.questData[currentQuest.questData.ID].isCompleted;

        rewardButton.interactable = isCompleted && !hasReceivedReward;
        completeMark.SetActive(hasReceivedReward);
    }

    public void OnRewardButtonClick()
    {
        if (currentQuest.isCompleted)
        {
            QuestManager.Instance.CheckQuestCompletion(currentQuest.questData.ID);
            UpdateUI();
        }
    }

}
