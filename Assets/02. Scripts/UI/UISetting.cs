using UnityEngine;
using UnityEngine.UI;

public class UISetting : UIBase
{
    [SerializeField] private Button exitBtn;
    
    void Start()
    {
        exitBtn.onClick.AddListener(() => { Close(); });
    }
}
