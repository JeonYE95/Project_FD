using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionHandler : MonoBehaviour
{
    protected float lastActionTime = -Mathf.Infinity;

    public float cooldownTime;

    protected BaseCharacter character;
    protected BaseCharacter targetCharacter;

    public bool IsCooldownComplete()
    {
        return Time.time >= lastActionTime + cooldownTime;
    }

    public abstract void ExecuteAction(BaseCharacter targetCharacter);

    public void ResetCooldown()
    {
        lastActionTime = Time.time;
    }
}
