using UnityEngine;
using UnityEngine.UI;

public class UICombineInfo : UIBase
{
    [SerializeField] private Button _closeBtn;
    
    void Start()
    {
        _closeBtn.onClick.AddListener(() => { Close(); });
    }
}
