using System;
using UnityEngine;
using UnityEngine.UI;

public class UIStart : UIBase
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button menuBtn;
    [SerializeField] private Button quitBtn;

    private UIMenu uiMenu;
    private void Start()
    {
        menuBtn.onClick.AddListener(() =>
        {
            OpenMenu();
        });
    }

    private void OpenMenu()
    {
        if (uiMenu == null)
            uiMenu = UIManager.Instance.GetUI<UIMenu>();

        uiMenu.Open();
    }
}
