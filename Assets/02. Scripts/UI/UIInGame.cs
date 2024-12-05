using System;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : UIBase
{
    [SerializeField] private Button drawBtn;
    [SerializeField] private Button unitGuideBtn;

    private UIUnitGuide uiUnitGuide;

    private void Start()
    {
        drawBtn.onClick.AddListener(() => {  });    // 버튼 클릭 시 호출 함수 필요
        unitGuideBtn.onClick.AddListener(() => { OpenUnitGuideUI(); });  
    }

    private void OpenUnitGuideUI()
    {
        if (uiUnitGuide == null)
            uiUnitGuide = UIManager.Instance.GetUI<UIUnitGuide>();
        
        uiUnitGuide.Open();
    }
}
