using GSDatas;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISelectStage : UIBase
{
    [SerializeField] private GameObject _stageBtnPrefab; 
    [SerializeField] private RectTransform _stageBtnParent; 
    [SerializeField] private Button _closeBtn; 
    private UIStageBtn _uiStageBtn;

    void Start()
    {
        _closeBtn.onClick.AddListener(() => { Close(); });


 
            // UIStageBtn uiStageBtn = new UIStageBtn(stageData);

            // // 버튼 텍스트 설정
            // Text buttonText = newButton.GetComponentInChildren<Text>();
            // if (buttonText != null)
            // {
            //     buttonText.text = $"Stage {index + 1}"; // 예: "Stage 1"
            // }

            // 버튼 클릭 이벤트 설정
            // newButton.onClick.AddListener(() =>
            // {
            //     GameManager.Instance.StageID = stageId.ID;

            //     // 입장 필요 에너지 확인
            //     if (GameManager.Instance.EnterEnergy >= stageId.cost)
            //     {
            //         GameManager.Instance.EnterEnergy -= stageId.cost;
            //         QuestManager.Instance.UpdateConsumeQuests(3000, stageId.cost);
            //         LoadInGameScene();
            //     }
            //     else
            //     {
            //         Debug.Log("입장 필요 에너지가 부족합니다.");
            //     }
            // });
        
    }

    private void OnEnable() 
    {
        // 스테이지 버튼 동적 생성
        for (int i = 0; i < 5; i++)
        {
            int index = i; 
            StageData stageData = GameManager.Instance.TotalStageID[index];
            int stageIndex = i + 1;

            // 버튼 생성
            GameObject stageBtnObj = Instantiate(_stageBtnPrefab, _stageBtnParent);
            stageBtnObj.name = $"StageBtn_{stageData.ID}";

            _uiStageBtn = stageBtnObj.GetComponent<UIStageBtn>();
            _uiStageBtn.SetStageData(stageData, stageIndex);
        }
    }
}