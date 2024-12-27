using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuest : UIBase
{

    public Transform questContent;  // 퀘스트 슬롯들이 들어갈 부모 Transform
    public UIQuestSlot questSlotPrefab;  // 퀘스트 슬롯 프리팹

    private List<UIQuestSlot> questSlots = new List<UIQuestSlot>();

    protected override void OpenProcedure()
    {
        RefreshQuestList();
    }

    private void RefreshQuestList()
    {
        // 기존 슬롯들 제거
        foreach (UIQuestSlot slot in questSlots)
        {
            Destroy(slot.gameObject);
        }
        questSlots.Clear();

        // 새로운 퀘스트 슬롯들 생성
        var questList = QuestManager.Instance.GetCurrentQuests();
        foreach (var quest in questList)
        {
            UIQuestSlot slot = Instantiate(questSlotPrefab, questContent);
            slot.SetQuestData(quest);
            questSlots.Add(slot);
        }
    }


}
