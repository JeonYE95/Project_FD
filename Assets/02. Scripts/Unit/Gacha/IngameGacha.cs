using GSDatas;
using UnityEngine;
using System.Collections.Generic;

public class IngameGacha : Singleton<IngameGacha>
{
    public int GachaGold = 3;

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
            Debug.Log($"유닛 : {selectedUnit.name}, 현재 레벨 : {selectedUnit.level}, 공격력 : {selectedUnit.attack}, 방어력 : {selectedUnit.defense}, 체력 : {selectedUnit.health}");
        }
    }
}