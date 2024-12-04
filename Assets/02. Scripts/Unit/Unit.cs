using GSDatas;
using UnityEngine;

public class Unit : MonoBehaviour
{
    /*public int Unit_ID;
    public string Name;
    public string Grade;
    public int Attack;
    public int Defense;
    public int Health;
    public int AttackSpeed;
    public int Cooltime;
    public int Range;*/

    private UnitData _unitData;

    public void SetData(GSDatas.UnitData data)
    {
        /*Unit_ID = data.Unit_ID;
        Name = data.Name;
        Grade = data.Grade;
        Attack = data.Attack;
        Defense = data.Defense;
        Health = data.Health;
        AttackSpeed = data.AttackSpeed;
        Cooltime = data.Cooltime;
        Range = data.Range;*/

        _unitData = data;

        //Debug.Log($"[Unit 데이터 확인] Name: {Name}, Grade: {Grade}, Attack: {Attack}, Defense: {Defense}, Health: {Health}");
    }
}