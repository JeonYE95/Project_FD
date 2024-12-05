using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPrevInfo : MonoBehaviour
{
    private UnitInfo _unitInfo;

    public void SetUnitInfo(UnitInfo unit)
    {
        _unitInfo = unit;
    }

    public UnitInfo GetUnitInfo()
    {
        return _unitInfo;
    }
}
