using UnityEngine;
using UnityEngine.UI;

public class UIUnitGuide : UIBase
{
    [SerializeField] private Button closeBtn;
    
    void Start()
    {
        closeBtn.onClick.AddListener(() => { Close(); });
    }
}
