using UnityEngine;
using UnityEngine.UI;

public class UIUnitGuide : UIBase
{
    [SerializeField] private Button _closeBtn;
    
    void Start()
    {
        _closeBtn.onClick.AddListener(() => { Close(); });
    }
}
