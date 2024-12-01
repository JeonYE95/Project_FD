using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutStageShop : ShopBase
{

    private DataManager _dataManager => DataManager.Instance;

    protected override void AddGold(int value)
    {

        _dataManager.PlayerData.gold += value;
    
    }

    protected override bool HasGold(int value)
    {

        return _dataManager.PlayerData.gold >= value;

    }

    protected override void UseGold(int value)
    {

        _dataManager.PlayerData.gold -= value;

    }



}
