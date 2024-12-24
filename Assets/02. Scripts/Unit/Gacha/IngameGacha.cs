using GSDatas;
using UnityEngine;
using System.Collections.Generic;

public class IngameGacha : Singleton<IngameGacha>
{
    public int GachaGold = 1;

    public void PlayGacha()
    {
        if (StageManager.Instance.Gold < GachaGold)
        {
            return;
        }

        StageManager.Instance.Gold -= GachaGold;

        GachaData ingameData = GachaDataManager.Instance.GetRandomData("Ingame");
        if (ingameData == null) return;

        UnitData selectedUnit = UnitDataManager.Instance.GetUnitData(ingameData.ID);
        if (selectedUnit != null)
        {
            InventoryManager.Instance.AddCharacter(selectedUnit);
        }
    }
}