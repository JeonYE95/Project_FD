using UnityEngine;
using UnityEngine.UI;

public class UIUnitGuide : UIBase
{
    [SerializeField] private Button exitBtn;
    
    void Start()
    {
        exitBtn.onClick.AddListener(() => { Close(); });
    }
}
