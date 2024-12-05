using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BindingGradeButton : MonoBehaviour
{

    public void Start()
    {
        Button normalButton = GetComponentsInChildren<Button>()[(int)Defines.UnitGrade.common];
        Button rareButton = GetComponentsInChildren<Button>()[(int)Defines.UnitGrade.rare];
        Button uniqueButton = GetComponentsInChildren<Button>()[(int)Defines.UnitGrade.Unique];

        normalButton.onClick.AddListener(() => OnGradeButtonClick(Defines.UnitGrade.common));
        rareButton.onClick.AddListener(() => OnGradeButtonClick(Defines.UnitGrade.rare));
        uniqueButton.onClick.AddListener(() => OnGradeButtonClick(Defines.UnitGrade.Unique));
    }

    private void OnGradeButtonClick(Defines.UnitGrade units)
    {
        InventoryManager.Instance.UpdateUnitGrade(units);
    }

}
