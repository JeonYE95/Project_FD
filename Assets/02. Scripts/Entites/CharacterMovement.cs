using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    Vector2 moveDirection;

    Rigidbody2D rigidbody2D;
    BaseCharacter character;

    private void Awake()
    {
        character = GetComponent<BaseCharacter>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        character.OnMoveEvent += Move;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        ApplyMove(moveDirection);
    }

    private void Move(Vector2 direction)
    {
        moveDirection = direction;
    }

    private void ApplyMove(Vector2 direction)
    {
        direction = direction * character.moveSpeed;

        rigidbody2D.velocity = direction;
    }
}
