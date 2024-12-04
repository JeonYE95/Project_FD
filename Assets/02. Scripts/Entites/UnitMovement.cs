using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public float moveSpeed;

    //디버그용 퍼블릭
    public Vector2 moveDirection;
    public Vector2 destinationPosition;

    public Rigidbody2D rigidbody2D;
    BaseUnit character;


    private void Awake()
    {
        character = GetComponent<BaseUnit>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //character.OnMoveEvent += Move;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

    }

    public void Stop()
    {
        rigidbody2D.velocity = Vector2.zero;
    }

    public void MoveToTarget()
    {
        if (character.targetUnit == null)
        {
            return;
        }

        Vector2 myPosition = transform.position;
        Vector2 AdjustedTargetPosition = GetAdjustedTargetPosition(myPosition, character.targetUnit.transform.position, character.attackRange);

        moveDirection = (AdjustedTargetPosition - myPosition).normalized;
        moveDirection = moveDirection * moveSpeed;

        rigidbody2D.velocity = moveDirection;
    }

    private Vector2 GetAdjustedTargetPosition(Vector2 myPosition, Vector2 targetPosition, float attackRange)
    {
        Vector2 directionToTarget = (targetPosition - myPosition).normalized;

        Vector2 AdjustTargetPositon = targetPosition - directionToTarget * attackRange;

        return AdjustTargetPositon;
    }

    private void MoveToDirection(Vector2 direction)
    {
        //rigidbody2D.velocity = direction * moveS
    }

    private void ApplyMove(Vector2 direction)
    {
        direction = direction * character.moveSpeed;

        rigidbody2D.velocity = direction;
    }
}
