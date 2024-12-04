using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPrevInfo : MonoBehaviour
{
    private Unit _unitInfo;

    public void SetUnitInfo(Unit unit)
    {
        _unitInfo = unit;
    }

    public Unit GetUnitInfo()
    {
        return _unitInfo;
    }
}
