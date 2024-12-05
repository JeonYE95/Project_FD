using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : BaseUnit
{
    UnitInfo unitInfo;

    protected override void Awake()
    {
        base.Awake();

        unitInfo = GetComponent<UnitInfo>();
    }

    public void SetUnitInfo()
    {

    }
}
