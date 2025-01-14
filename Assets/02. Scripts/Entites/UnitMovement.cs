using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public float moveSpeed;

    //디버그용 퍼블릭
    public Vector2 moveDirection;
    public Vector2 destinationPosition;

    Rigidbody2D _rigidbody2D;
    BaseUnit _myUnit;

    float _attackDistanceMuti;

    private float randomCircleSize = 0.0f; // 랜덤 범위 조정 가능

    private Vector2 _randomOffset; // 타겟 위치에 추가될 랜덤 오프셋

    public float RandomCircleSize
    {
        get { return randomCircleSize; }
        set { randomCircleSize = value; }
    }

    private void Awake()
    {
        _myUnit = GetComponent<BaseUnit>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void SetRandomOffset(Vector2 randomOffset)
    {
        _randomOffset = randomOffset;
    }

    public void Stop()
    {
        _rigidbody2D.velocity = Vector2.zero;
    }

    public void MoveToTarget()
    {
        if (_myUnit.targetUnit == null)
        {
            return;
        }

        Vector2 myPosition = transform.position;
        Vector2 AdjustedTargetPosition = GetAdjustedTargetPosition(myPosition, _myUnit.targetUnit.transform.position, _myUnit.unitInfo.Range);

        moveDirection = (AdjustedTargetPosition - myPosition).normalized;
        moveDirection = moveDirection * moveSpeed;

        _rigidbody2D.velocity = moveDirection;
    }

    private Vector2 GetAdjustedTargetPosition(Vector2 myPosition, Vector2 targetPosition, float attackRange)
    {
        Vector2 directionToTarget = (targetPosition - myPosition).normalized;

        Vector2 AdjustTargetPositon = targetPosition - (directionToTarget * attackRange * _attackDistanceMuti);

        AdjustTargetPositon += _randomOffset;

        return AdjustTargetPositon;
    }

    private void ApplyMove(Vector2 direction)
    {
        direction = direction * moveSpeed;

        _rigidbody2D.velocity = direction;
    }
}
