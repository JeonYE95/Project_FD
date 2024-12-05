using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GSDatas;

public class UnitPrevInfo : MonoBehaviour
{
    private UnitInfo _unitInfo;

    public void SetUnitInfo(UnitInfo unit)
    {
        // 새로운 UnitInfo 인스턴스 생성
        _unitInfo = gameObject.AddComponent<UnitInfo>();

        // 데이터 복사
        if (unit != null && unit._unitData != null)
        {
            // UnitData 복사
            UnitData newData = new UnitData
            {
                ID = unit._unitData.ID,
                name = unit._unitData.name,
                grade = unit._unitData.grade,
             
            };

            _unitInfo.SetData(newData);
        }

        Debug.Log($"Preview Info set with unit: {unit?._unitData.name ?? "null"}");
    }

    public UnitInfo GetUnitInfo()
    {
        return _unitInfo;
    }

    private void OnDisable()
    {
        // 컴포넌트가 비활성화될 때 이전 UnitInfo 정리
        if (_unitInfo != null)
        {
            Destroy(_unitInfo);
            _unitInfo = null;
        }
    }
}
