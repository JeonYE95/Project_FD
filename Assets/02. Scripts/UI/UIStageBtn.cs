using GSDatas;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIStageBtn : MonoBehaviour
{
    [SerializeField] private GameObject _defaultFrame;
    [SerializeField] private GameObject _clearFrame;
    [SerializeField] private GameObject _stageTxt;
    [SerializeField] private GameObject _lockImg;
    private Button _stageBtn;
    private StageData _stageData;
    private int _stageIndex;
    private int _stageID;

    public void SetStageData(StageData stageData, int stageIndex)
    {
        _stageData = stageData;
        _stageIndex = stageIndex;
        _stageID = stageData.ID;

        _stageBtn = GetComponent<Button>();

        SetStageState();
    }

    void Start()
    {
        TMP_Text stageTxt = _stageTxt.GetComponent<TMP_Text>();
        Image lockImg = _lockImg.GetComponent<Image>();
        
        stageTxt.text = $"1-{_stageIndex}";

        _stageBtn.onClick.AddListener(() => 
        {
            GameManager.Instance.StageID = _stageData.ID;
            UIManager.Instance.OpenUI<UIStageEnter>();
           
        });
    }

    private void UnlockStage()
    {
        _defaultFrame.SetActive(true);
        _clearFrame.SetActive(false);
        _lockImg.SetActive(false);
        _stageTxt.SetActive(true);
        _stageBtn.interactable = true;
    }

    private void LockStage()
    {
        _defaultFrame.SetActive(true);
        _clearFrame.SetActive(false);        
        _lockImg.SetActive(true);
        _stageTxt.SetActive(false);
        _stageBtn.interactable = false;
    }

    private void ClearStage()
    {
        _defaultFrame.SetActive(false);
        _clearFrame.SetActive(true);
        _lockImg.SetActive(false);
        _stageTxt.SetActive(true);
        _stageBtn.interactable = true;
    }

    private void SetStageState()
    {
        if (GameManager.Instance.playerData.StageClearData.ContainsKey(_stageID))
        {
            switch (GameManager.Instance.playerData.StageClearData[_stageID])
            {
                case Defines.StageClearState.Clear:
                    ClearStage();
                    break;
                case Defines.StageClearState.Unlock:
                    UnlockStage();
                    break;
                case Defines.StageClearState.Lock:
                    LockStage();
                    break;
            }
        }
        else
        {
            Debug.Log($"StageClearData에 키 {_stageID}가 존재하지 않습니다.");
        }
    }

 
}
