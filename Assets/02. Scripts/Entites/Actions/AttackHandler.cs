using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : ActionHandler
{
    public float attackDamage = 10f;

    public override void ExecuteAction(BaseUnit targetCharacter)
    {
        if (targetCharacter == null)
        {
            return;
        }

        targetCharacter.TryGetComponent(out HealthSystem healthSystem);

        if (healthSystem != null)
        {
            healthSystem.TakeDamage(attackDamage);
        }

        ResetCooldown();
    }
    
}
