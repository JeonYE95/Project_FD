using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIQuest : UIBase
{

    public UIScrollRecycle questScrollView;
    public Toggle weekQuest;
    public Toggle dayQuest;
    public Button batchRewardButton;
    public Button closeButton;
        
    private QuestResetType currentQuestType = QuestResetType.Daily;
    private ToggleGroup toggleGroup;


    private void Start()
    {
        batchRewardButton.onClick.AddListener(OnBatchRewardButtonClick);
        closeButton.onClick.AddListener(() => { Close(); });
        InitializeToggleGroup();

    }

    private void InitializeToggleGroup()
    {
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

        // 새로운 퀘스트 슬롯 생성
        List<QuestBase> questList = QuestManager.Instance.GetQuestsByType(currentQuestType)
       .Where(quest =>
       {
           // GameManager에서 퀘스트 데이터 확인
           var questData = GameManager.Instance.playerData.questData[quest.questData.ID];

           // 보상을 받지 않은 퀘스트는 모두 표시
           if (!questData.hasReceivedReward)
               return true;

           // 보상을 받은 퀘스트 중 연계 퀘스트가 없는 것만 표시
           return quest.questData.nextQuestID == 0;
       })
       .ToList();

        questScrollView.SetQuestList(questList);
        UpdateBatchRewardButton();
    }


    //일괄 수령 - 각 슬롯의 보상 버튼 클릭 메서드 호출
    private void OnBatchRewardButtonClick()
    {
        var questList = QuestManager.Instance.GetQuestsByType(currentQuestType);
        foreach (var quest in questList)
        {
            if (quest.isCompleted && !GameManager.Instance.playerData.questData[quest.questData.ID].isCompleted)
            {
                QuestManager.Instance.QuestCompletion(quest.questData.ID);
            }
        }
        RefreshQuestList();
    }

    // 수령 가능한 퀘스트가 있는지 체크
    private void UpdateBatchRewardButton()
    {
        var questList = QuestManager.Instance.GetQuestsByType(currentQuestType);
        bool hasCompletedQuest = questList.Any(quest =>
            quest.isCompleted &&
            !GameManager.Instance.playerData.questData[quest.questData.ID].isCompleted);

        batchRewardButton.interactable = hasCompletedQuest;
    }




}
