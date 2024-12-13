using GSDatas;
using UnityEngine;
using System.Collections.Generic;

public class GachaManager : Singleton<GachaManager>
{
    public int GachaGold = 1;

    public void PlayGacha()
    {
        if (StageManager.Instance.Gold < GachaGold)
        {
            return;
        }

        StageManager.Instance.Gold -= GachaGold;

        List<UnitData> commonUnits = new List<UnitData>();

        foreach (var unit in UnitDataManager.Instance.GetUnitDatas())
        {
            if (unit.grade == "Common")
            {
                commonUnits.Add(unit);
            }
        }

        UnitData selectedUnit = commonUnits[Random.Range(0, commonUnits.Count)];
        InventoryManager.Instance.AddCharacterData(selectedUnit);


    }
}