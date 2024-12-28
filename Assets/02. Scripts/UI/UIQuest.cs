using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIQuest : UIBase
{

    public Transform questContent;  // 퀘스트 슬롯들이 들어갈 부모 Transform
    public UIQuestSlot questSlotPrefab;
    public Button batchRewardButton;
    public Button closeButton;
        
    private List<UIQuestSlot> questSlots = new List<UIQuestSlot>();

    private void Start()
    {
        batchRewardButton.onClick.AddListener(OnBatchRewardButtonClick);
        closeButton.onClick.AddListener(() => { Close(); });
    }

    protected override void OpenProcedure()
    {
        RefreshQuestList();
        UpdateBatchRewardButton();
    }


    // 리팩토링 예정
    private void RefreshQuestList()
    {
        
        foreach (UIQuestSlot slot in questSlots)
        {
            Destroy(slot.gameObject);
        }
        questSlots.Clear();

        // 새로운 퀘스트 슬롯 생성
        List<Quest> questList = QuestManager.Instance.GetCurrentQuests();
        foreach (var quest in questList)
        {
            UIQuestSlot slot = Instantiate(questSlotPrefab, questContent);
            slot.SetQuestData(quest);
            questSlots.Add(slot);
        }

        UpdateBatchRewardButton();
    }


    //일괄 수령 - 각 슬롯의 보상 버튼 클릭 메서드 호출
    private void OnBatchRewardButtonClick()
    {
        foreach (UIQuestSlot slot in questSlots)
        {
            slot.OnRewardButtonClick();  
        }
        RefreshQuestList(); 
    }

    // 수령 가능한 퀘스트가 있는지 체크
    private void UpdateBatchRewardButton()
    {

        bool hasCompletedQuest = questSlots.Any(slot =>
            slot.CurrentQuest.isCompleted &&
            !GameManager.Instance.playerData.questData[slot.CurrentQuest.questData.ID].isCompleted);

        batchRewardButton.interactable = hasCompletedQuest;
    }
}
