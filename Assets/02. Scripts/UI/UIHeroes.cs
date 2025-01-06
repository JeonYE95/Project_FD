using UnityEngine;
using UnityEngine.UI;

public class UIHeroes : UIBase
{
    [SerializeField] private Button _backBtn;
    
    // Start is called before the first frame update
    void Start()
    {
        _backBtn.onClick.AddListener(() => { UIManager.Instance.CloseUI<UIHeroes>(); } );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
