using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackHandler : AttackHandler
{
    public GameObject projectilePrefab;
    public Transform firePoint;

    private void Start()
    {
        firePoint = transform;
    }

    public override void ExecuteAction(BaseCharacter targetCharacter)
    {
        if (targetCharacter == null || projectilePrefab == null || firePoint == null)
        {
            Debug.Log("원거리 공격 에러 발생");
            return;
        }

        GameObject defaultProjectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Vector2 direction = (targetCharacter.transform.position - firePoint.position).normalized;

        defaultProjectile.GetComponent<DefaultProjectile>().Initialize(targetCharacter, direction);

        ResetCooldown();
    }

}
