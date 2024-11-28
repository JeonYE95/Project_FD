using UnityEngine;

public class Unit : MonoBehaviour
{
    public int UnitID;
    public string UnitName;
    public int Attack;
    public int Defense;
    public int Health;

    public void SetData(UnitData data)
    {
        UnitID = data.UnitID;
        UnitName = data.Name;
        Attack = data.Attack;
        Defense = data.Defense;
        Health = data.Health;

        Debug.Log($"[Unit Data Applied] Name: {UnitName}, Attack: {Attack}, Defense: {Defense}, Health: {Health}");
    }
}