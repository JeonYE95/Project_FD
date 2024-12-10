using GSDatas;
using UnityEngine;

public class UnitInfo : MonoBehaviour
{
    public UnitData _unitData;

    public void SetData(GSDatas.UnitData data)
    {
        _unitData = data;

        //Debug.Log($"유닛 데이터 적용: ID={_unitData.ID}, Name={_unitData.name}, Attack={_unitData.attack}, Defense={_unitData.defense}, Health={_unitData.health}");
    }
}