using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultProjectile : MonoBehaviour
{
    bool _haveEffect;
    string _effectTag;

    public float speed = 10f;
    public int damage = 10;

    private Vector2 direction;
    private BaseUnit targetUnit;

    private Action <int>Boo;

    private void Start()
    {
        //Destroy(gameObject, 7f);
    }

    public void SetProjectile(BaseUnit targetUnit, Vector2 direction, int damage)
    {
        this.damage = damage;
        this.direction = direction;
        this.targetUnit = targetUnit;
    }

    public void SetTargetTriggerEffect(string effectTag)
    {
        _haveEffect = true;
        _effectTag = effectTag;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetUnit != null && targetUnit.isLive)
        {
            direction = ((Vector2)targetUnit.transform.position - (Vector2)transform.position).normalized;
        }
        else
        {
            DeActiveThis();
        }

        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //타겟 이외의 캐릭터
        if (collision.gameObject != targetUnit.gameObject)
        {
            return;
        }

        if (collision.TryGetComponent(out HealthSystem healthSystem))
        {
            if (_haveEffect)
            {
                GameObject visualEffect = ObjectPool.Instance.SpawnFromPool(_effectTag);
                visualEffect.transform.position = healthSystem.transform.position;
            }

            healthSystem.TakeDamage(damage);
            DeActiveThis();
        }
    }

    protected virtual void DeActiveThis()
    {
        ObjectPool.Instance.ReturnToPool(this.gameObject, Defines.DefaultProejectileTag);
    }
}
