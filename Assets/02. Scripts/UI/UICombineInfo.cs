using UnityEngine;
using UnityEngine.UI;

public class UICombineInfo : UIBase
{
    [SerializeField] private Button closeBtn;
    
    void Start()
    {
        closeBtn.onClick.AddListener(() => { Close(); });
    }
}
