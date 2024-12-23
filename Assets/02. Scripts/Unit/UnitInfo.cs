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
            if (_unitData != null)
            {
                _unitData = new UnitData();
            }

            //_unitData = data;

            //_unitData = data.Clone();


            //강화 단계에 맞는 유닛 데이터 가져오기
            int currentLevel = EnforceManager.Instance.GetCurrentUnitEnforceLevel(data.ID);
            int enforcedUnitID = (data.ID * 10) + currentLevel;

            EnforceData enforcedData = EnforceDataManager.Instance.GetUnitData(enforcedUnitID);

            // 강화 정보가 있으면 강화된 스탯 적용, 없으면 기본 스탯 적용

            _unitData.ID = data.ID;
            _unitData.name = data.name;
            _unitData.grade = data.grade;

            if (enforcedData != null)
            {
                _unitData.range = enforcedData.range;
                _unitData.attack = enforcedData.attack;
                _unitData.health = enforcedData.health;
                _unitData.defense = enforcedData.defense;
                _unitData.skillCooltime = enforcedData.skillCooltime;
                _unitData.attackCooltime = enforcedData.attackCooltime;
            }
            else
            {
                _unitData.range = data.range;
                _unitData.attack = data.attack;
                _unitData.health = data.health;
                _unitData.defense = data.defense;
                _unitData.skillCooltime = data.skillCooltime;
                _unitData.attackCooltime = data.attackCooltime;

            }
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
