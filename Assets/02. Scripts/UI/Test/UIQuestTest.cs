using UnityEngine.UI;
using UnityEngine;

public class UIQuestTest : UIBase
{
    [SerializeField] private Button _startBtn;
 

    private void Start()
    {
        _startBtn.onClick.AddListener(() => { UIManager.Instance.OpenUI<UIQuest>(); });
      
    }
}
