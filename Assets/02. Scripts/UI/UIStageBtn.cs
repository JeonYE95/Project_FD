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

            // 입장 필요 에너지 확인
            if (GameManager.Instance.EnterEnergy >= _stageData.cost)
            {
                GameManager.Instance.EnterEnergy -= _stageData.cost;
                QuestManager.Instance.UpdateConsumeQuests(3000, _stageData.cost);
                LoadInGameScene();
            }
            else
            {
                Debug.Log("입장 필요 에너지가 부족합니다.");
            }
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

    
    private void LoadInGameScene()
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            // 씬 로드 후 UI 오픈
            if (SceneManager.GetActiveScene().buildIndex == 2)
                UIManager.Instance.OpenUI<UIInGame>();
        };

        UIManager.Instance.Clear();
        SceneManager.LoadScene("InGameBattleScene"); // 씬 로드
        SoundManager.Instance.PlayBGM("BattleBGM");
    }
}
