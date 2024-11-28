using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float maxHP;
    public float currentHP;

    BaseCharacter character;
    private void Awake()
    {
        character = GetComponent<BaseCharacter>();
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP < 0)
        {
            character.CallDieEvent();
        }
    }
}
