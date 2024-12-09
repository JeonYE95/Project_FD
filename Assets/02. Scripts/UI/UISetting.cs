using UnityEngine;
using UnityEngine.UI;

public class UISetting : UIBase
{
    [SerializeField] private Button _exitBtn;
    
    void Start()
    {
        _exitBtn.onClick.AddListener(() => { Close(); });
    }
}
