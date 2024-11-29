using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;

    private Vector2 direction;
    private BaseCharacter targetCharacter;

    private void Start()
    {
        Destroy(gameObject, 7f);
    }

    public void Initialize(BaseCharacter targetCharacter, Vector2 direction)
    {
        this.direction = direction;
        this.targetCharacter = targetCharacter;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetCharacter != null && targetCharacter.isLive)
        {
            direction = ((Vector2)targetCharacter.transform.position - (Vector2)transform.position).normalized;
        }
        else
        {
            Destroy(gameObject);
        }

        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //타겟 이외의 캐릭터
        if (collision.gameObject != targetCharacter.gameObject)
        {
            return;
        }

        if (collision.TryGetComponent(out HealthSystem healthSystem))
        {
            healthSystem.TakeDamage(damage);
            Destroy(gameObject); 
        }
    }
}
