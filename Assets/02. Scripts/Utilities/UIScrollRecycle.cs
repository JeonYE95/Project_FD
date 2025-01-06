
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using System.Collections.Generic;


public class UIScrollRecycle : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public UIQuestSlot slotPrefab;
    public float slotHeight = 200f; // 슬롯 하나의 높이

    private List<QuestBase> questList = new List<QuestBase>();
    private List<UIQuestSlot> activeSlots = new List<UIQuestSlot>();
    private ObjectPool<UIQuestSlot> slotPool;
    private float lastScrollPos = 0;

    private void Awake()
    {
        // 오브젝트 풀 초기화
        slotPool = new ObjectPool<UIQuestSlot>(
            createFunc: () => Instantiate(slotPrefab, content),
            actionOnGet: (slot) => slot.gameObject.SetActive(true),
            actionOnRelease: (slot) => slot.gameObject.SetActive(false),
            actionOnDestroy: (slot) => Destroy(slot.gameObject),
            defaultCapacity: 10,
            maxSize: 20
        );

        content.anchorMin = new Vector2(0.5f, 1);
        content.anchorMax = new Vector2(0.5f, 1);
        content.pivot = new Vector2(0.5f, 1);
        content.anchoredPosition = new Vector2(0, 0);

        scrollRect.onValueChanged.AddListener(OnScroll);
    }

    public void SetQuestList(List<QuestBase> newQuestList)
    {
        questList = newQuestList;

        // 컨텐츠 영역 크기 설정
        float contentHeight = questList.Count * slotHeight;
        content.sizeDelta = new Vector2(0, contentHeight);

        // 스크롤 위치 초기화
        scrollRect.normalizedPosition = new Vector2(0, 1);
        lastScrollPos = 0;

        // 초기 슬롯 생성
        RefreshVisibleSlots();
    }

    private void OnScroll(Vector2 value)
    {

        // 스크롤 제한 체크 - 마지막 항목에서 멈추기
        float totalHeight = questList.Count * slotHeight;
        float viewportHeight = ((RectTransform)scrollRect.transform).rect.height;
        float maxScroll = totalHeight - viewportHeight;

        // 아래로 더 스크롤 되지 않도록 제한
        if (content.anchoredPosition.y > maxScroll)
        {
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, maxScroll);
            return;
        }

        // 위로 더 스크롤 되지 않도록 제한
        if (content.anchoredPosition.y < 0)
        {
            content.anchoredPosition = Vector2.zero;
            return;
        }

        RefreshVisibleSlots();
    }


    private void RefreshVisibleSlots()
    {

        if (questList.Count == 0) return;

        // 현재 보이는 영역 계산
        float scrollPosition = content.anchoredPosition.y;
        float viewportHeight = ((RectTransform)scrollRect.transform).rect.height;

        // 보이는 슬롯의 시작과 끝 인덱스 계산
        int startIndex = Mathf.Max(0, Mathf.FloorToInt(scrollPosition / slotHeight));
        int endIndex = Mathf.Min(questList.Count - 1, Mathf.CeilToInt((scrollPosition + viewportHeight) / slotHeight));

        // 현재 활성화된 모든 슬롯을 풀로 반환
        foreach (var slot in activeSlots)
        {
            slotPool.Release(slot);
        }
        activeSlots.Clear();

        // 보이는 영역의 슬롯만 생성/재활용
        for (int i = startIndex; i <= endIndex; i++)
        {

            if (i >= questList.Count) break;

            UIQuestSlot slot = slotPool.Get();
            slot.gameObject.SetActive(true);

            // 슬롯 상태 초기화
            if (slot.completeMark != null)
            {
                slot.completeMark.SetActive(false);
            }

            slot.transform.localPosition = new Vector3(0, -i * slotHeight, 0);
            slot.SetQuestData(questList[i]);
            activeSlots.Add(slot);
        }
    }


    private void OnDestroy()
    {
        slotPool.Dispose();
    }
}
