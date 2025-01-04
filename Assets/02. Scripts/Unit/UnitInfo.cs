using GSDatas;
using UnityEngine;
public interface IUnitInfo
{
    int ID { get; }
    int Attack { get; set; }
    int Defense { get; set; }
    int Health { get; set; }
    float Range { get; set; }
    float SkillCooltime { get; set; }
    float AttackCooltime { get; set; }
    string Name { get; set; }

     //public void SetData();
}


public class UnitInfo : MonoBehaviour, IUnitInfo
{
    public UnitData _unitData;

    public UnitInfo()
    {
        _unitData = new UnitData();

    }

    public void SetData(GSDatas.UnitData data)
    {
        if (data != null)
        {
            if (_unitData == null)
            {
                _unitData = new UnitData();
            }

            //_unitData = data;

            //_unitData = data.Clone();

            _unitData.ID = data.ID;
            _unitData.name = data.name;
            _unitData.grade = data.grade;
            _unitData.range = data.range;
            _unitData.attack = data.attack;
            _unitData.health = data.health;
            _unitData.defense = data.defense;
            _unitData.skillCooltime = data.skillCooltime;
            _unitData.attackCooltime = data.attackCooltime;
            _unitData.level = data.level;
            _unitData.maxLevel = data.maxLevel;
            _unitData.classtype = data.classtype;

            //EnforceManager.Instance.GetUnitEnforcedData(_unitData);
            //EnforceManager.Instance.GetClassEnforcedData(_unitData);
        }

    }



    public int ID
    {
        get => _unitData.ID;
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
