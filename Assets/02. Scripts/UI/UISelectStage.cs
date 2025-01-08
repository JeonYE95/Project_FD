using System.Collections.Generic;
using GSDatas;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISelectStage : UIBase
{
    [SerializeField] private GameObject _stageBtnPrefab; 
    [SerializeField] private RectTransform _stageBtnParent; 
    [SerializeField] private Button _closeBtn; 
    private bool _isGeneratedStageBtn;
    private UIStageBtn[] _uiStageBtn = new UIStageBtn[5];


    private void Awake() 
    {
        // 스테이지 버튼 동적 생성
        for (int i = 0; i < 5; i++)
        {
            int index = i; 
            StageData stageData = GameManager.Instance.TotalStageID[index];

            GameObject stageBtnObj = Instantiate(_stageBtnPrefab, _stageBtnParent);
            stageBtnObj.name = $"StageBtn_{stageData.ID}";
            _uiStageBtn[i] = stageBtnObj.GetComponent<UIStageBtn>();
        }

        _isGeneratedStageBtn = true;
    }

    void Start()
    {
        _closeBtn.onClick.AddListener(() => { Close(); });

        if (_isGeneratedStageBtn)
        {   
            _uiStageBtn = _stageBtnParent.GetComponentsInChildren<UIStageBtn>();
        }
        else
        {
            Debug.Log("버튼 생성 안됨");
        }
    }

    private void OnEnable() 
    {
        for (int i = 0; i < 5; i++)
        {   
            int index = i; 
            StageData stageData = GameManager.Instance.TotalStageID[index];
            int stageIndex = i + 1;

            _uiStageBtn[i].GetComponent<UIStageBtn>().SetStageData(stageData, stageIndex);
        }
    }
}
