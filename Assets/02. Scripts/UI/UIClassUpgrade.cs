using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIClassUpgrade : UIBase
{
    private ClassUpgrade _classUpgrade = new ClassUpgrade();

    [SerializeField] private GameObject _classBtnParent;
    private Button[] _classButtons;
    [SerializeField] private Button _upgradeBtn;
    private string _targetClass;

    // Start is called before the first frame update
    void Start()
    {

        _classButtons = _classBtnParent.GetComponentsInChildren<Button>();
        _upgradeBtn.onClick.AddListener(() => {_classUpgrade.UpgradeClass("");});
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
