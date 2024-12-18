using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UISelectStage : UIBase
{
    [SerializeField] private Button[] _stageBtn;
    [SerializeField] private Transform _stageSelectButtonParent;

    [SerializeField] private Button _exitBtn;

    void Start()
    {

        //스테이지 버튼 찾기
        _stageBtn = _stageSelectButtonParent.GetComponentsInChildren<Button>();

        //스테이지 순서에 맞는 ID 등록
        for (int i = 0; i < _stageBtn.Length; i++)
        {

            int stageId = GameManager.Instance.TotalStageID[i].ID;

            _stageBtn[i].onClick.AddListener(() =>
            {
                GameManager.Instance.StageID = stageId;

                //해당 스테이지 입장 필요 에너지 충족 시 입장.
                if (GameManager.Instance.EnterEnergy >= GameManager.Instance.TotalStageID[i].cost)
                {
                    GameManager.Instance.EnterEnergy -= GameManager.Instance.TotalStageID[i].cost;
                    LoadInGameScene();

                }
                else
                {
                    Debug.Log("입장 필요 에너지가 부족합니다.");
                }

            });

        }

        _exitBtn.onClick.AddListener(() => { Close(); });
    }

    private void LoadInGameScene()
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            // 씬 로드 후 UI 오픈
            if (SceneManager.GetActiveScene().buildIndex == 2)
                UIManager.Instance.OpenUI<UIInGame>();
        };


        SceneManager.LoadScene("InGameBattleScene"); // 씬 로드
    }
}
