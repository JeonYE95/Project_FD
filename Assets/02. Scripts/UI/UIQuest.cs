using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIQuest : UIBase
{

    public Transform questContent;  // 퀘스트 슬롯들이 들어갈 부모 Transform
    public UIQuestSlot questSlotPrefab;
    public Toggle weekQuest;
    public Toggle dayQuest;
    public Button batchRewardButton;
    public Button closeButton;
        
    private List<UIQuestSlot> questSlots = new List<UIQuestSlot>();
    private QuestResetType currentQuestType = QuestResetType.Daily;
    private ToggleGroup toggleGroup;


    private void Start()
    {
        batchRewardButton.onClick.AddListener(OnBatchRewardButtonClick);
        closeButton.onClick.AddListener(() => { Close(); });

        toggleGroup = dayQuest.transform.parent.GetComponent<ToggleGroup>();
        if (toggleGroup == null)
        {
            toggleGroup = dayQuest.transform.parent.gameObject.AddComponent<ToggleGroup>();
        }
        toggleGroup.allowSwitchOff = false;

        // 토글들을 그룹에 등록
        dayQuest.group = toggleGroup;
        weekQuest.group = toggleGroup;

        // 토글 이벤트 설정
        dayQuest.onValueChanged.AddListener((isOn) => {
            if (isOn)
            {
                currentQuestType = QuestResetType.Daily;
                RefreshQuestList();
            }
        });

        weekQuest.onValueChanged.AddListener((isOn) => {
            if (isOn)
            {
                currentQuestType = QuestResetType.Weekly;
                RefreshQuestList();
            }
        });

    }

    protected override void OpenProcedure()
    {


        dayQuest.isOn = true;
        currentQuestType = QuestResetType.Daily;

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
        List<QuestBase> questList = QuestManager.Instance.GetQuestsByType(currentQuestType);


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
