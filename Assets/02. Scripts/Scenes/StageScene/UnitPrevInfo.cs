using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GSDatas;

public class UnitPrevInfo : MonoBehaviour
{
    private UnitData _unitData;

    public void SetUnitData(UnitData data)
    {
        if (data == null) return;

        // 깊은 복사
        _unitData = data.Clone();
    }

    public UnitData GetUnitData()
    {
        return _unitData;
    }

    private void OnDisable()
    {
        // 컴포넌트가 비활성화될 때 이전 UnitInfo 정리
        if (_unitData != null)
        {

            _unitData = null;
        }
    }
}
