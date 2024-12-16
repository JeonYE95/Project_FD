using GSDatas;
using UnityEngine;
public interface IUnitInfo
{
    int Attack { get; set; }
    int Defense { get; set; }
    int Health { get; set; }
    float Range { get; set; }
    float SkillCooltime { get; set; }
    float AttackCooltime { get; set; }
    string Name { get; set; }
}


public class UnitInfo : MonoBehaviour, IUnitInfo
{
    public UnitData _unitData;

    public void SetData(GSDatas.UnitData data)
    {
        _unitData = data;

        //Debug.Log($"유닛 데이터 적용: ID={_unitData.ID}, Name={_unitData.name}, Attack={_unitData.attack}, Defense={_unitData.defense}, Health={_unitData.health}");
    }

    public int Attack
    {
        get => _unitData.attack;
        set => _unitData.attack = value;
    }

    public int Defense
    {
        get => _unitData.defense;
        set => _unitData.defense = value;
    }

    public int Health
    {
        get => _unitData.health;
        set => _unitData.health = value;
    }

    public float Range
    {
        get => _unitData.range;
        set => _unitData.range = value;
    }

    public float SkillCooltime
    {
        get => _unitData.skillCooltime;
        set => _unitData.skillCooltime = value;
    }

    public float AttackCooltime
    {
        get => _unitData.attackCooltime;
        set => _unitData.attackCooltime = value;
    }

    public string Name
    {
        get => _unitData.name;
        set => _unitData.name = value;
    }

    // 플레이어 전용 데이터
    public string GetGrade() => _unitData?.grade ?? "None";
}