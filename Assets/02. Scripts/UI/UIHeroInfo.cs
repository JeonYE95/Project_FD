using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroInfo : UIBase
{
    [SerializeField] private Button _closeBtn;
    // Start is called before the first frame update
    void Start()
    {
        _closeBtn.onClick.AddListener(() => { UIManager.Instance.CloseUI<UIHeroInfo>(); } );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
