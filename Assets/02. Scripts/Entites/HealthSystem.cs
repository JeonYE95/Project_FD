using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private float maxHP;
    public float currentHP;

    //현재는 최대 HP가 바뀌면 현재 HP도 최대 HP로 설정
    //배틀 시 최대 체력이 바뀔리 없다는 가정
    public float MaxHP
    {
        get 
        { 
            return maxHP;
        }
        set 
        { 
            maxHP = value;
            currentHP = maxHP;
        }
    }

    BaseUnit unit;
    private void Awake()
    {
        unit = GetComponent<BaseUnit>();
    }

    public void ResetHealth()
    {
        currentHP = maxHP;
    }
    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP < 0)
        {
            unit.CallDieEvent();
        }
    }
}
