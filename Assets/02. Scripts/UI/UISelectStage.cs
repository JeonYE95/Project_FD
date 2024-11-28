using UnityEngine;
using UnityEngine.UI;

public class UISelectStage : UIBase
{
    [SerializeField] private Button stageBtn1_1;
    [SerializeField] private Button stageBtn1_2;
    [SerializeField] private Button stageBtn1_3;
    [SerializeField] private Button stageBtn1_4;
    [SerializeField] private Button stageBtn1_5;

    private UIInGame uiInGame;

    void Start()
    {
        stageBtn1_1.onClick.AddListener(() => { OpenInGameUI(1); });
    }

    private void OpenInGameUI(int stageNum)
    {
        if (uiInGame == null)
            uiInGame = UIManager.Instance.GetUI<UIInGame>();
        
        uiInGame.Open();
    }
}
