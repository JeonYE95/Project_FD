using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GSDatas;

public class UIUnitGuideSlot : UIBase
{

   [SerializeField] private Image _heroIcon;
   [SerializeField ]private Image _classIcon;

    private Button _findMixtureBtn;

    [SerializeField] private TextMeshProUGUI _heroName;

    private UnitData _unitData;

    public void UpdateInfo(UnitData unitData)
    {

        _unitData = unitData;

        if (_heroName != null)
            _heroName.text = unitData.name;

        if (_heroIcon != null)
        {
            Sprite sprite = Resources.Load<Sprite>($"Sprite/Unit/UpperBody/{unitData.grade}/{unitData.name}");
            _heroIcon.sprite = sprite;
        }

        if (_classIcon != null)
        {
            Sprite sprite = Resources.Load<Sprite>($"Sprite/Icon/Class/{unitData.classtype}");
            _classIcon.sprite = sprite;
        }
    }

}
