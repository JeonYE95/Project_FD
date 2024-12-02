using UnityEngine;

public class StageShop : ShopBase
{
    private StageManager _StageManager = StageManager.Instance;


    protected override void AddGold(int value)
    {

        _StageManager.Gold += value;

    }

    protected override bool HasGold(int value)
    {

        return _StageManager.Gold >= value;

    }

    protected override void UseGold(int value)
    {

        _StageManager.Gold -= value;

    }
}
