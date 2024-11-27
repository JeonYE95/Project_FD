using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseCharacter : MonoBehaviour
{
    public BaseCharacter targetCharacter;
    public Transform target;
    public Vector2 direction;

    public CharacterMovement characterMovement;

    public bool isGoingToTarget = false;
    public bool isPlayerCharacter = false;
    public bool isFightWithTarget = false;

    public Action<Vector2> OnMoveEvent;
    public Action OnAttackEvent;
    public Action OnDie;

    private float timeSinceLastAttack = float.MaxValue;

    //나중에 SO 화 할 것들
    public float moveSpeed;
    public float attackRange;
    public float attackDelay;

    //State Machine 도입 테스트
    public StateMachine stateMachine;

    private void Awake()
    {
        //상태 머신 만들기
        stateMachine = new StateMachine(this);
        characterMovement = GetComponent<CharacterMovement>();

        CharacterInit();
    }

    public void CharacterInit()
    {
        characterMovement.moveSpeed = moveSpeed;
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //HandleAttackDelay();

        stateMachine.Update();
    }

    public void HandleAttackDelay()
    {
        //현재 싸우고 있는 타겟이 없는 경우
        if (!isFightWithTarget)
        {
            //타겟을 찾을려고 시도했지만 못 찾은 경우
            if (!FindTarget())
            {
                Debug.Log("타겟을 못찾음");
                return;
            }
        }

        //타겟을 향해 가고있는 경우
        if (isGoingToTarget)
        {
            CallMoveEvent();
            return;
        }

        //타겟이 사정거리 내에 도착해서 때려야 하는 경우
        if (timeSinceLastAttack < attackDelay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }
        else if (timeSinceLastAttack >= attackDelay)
        {
            timeSinceLastAttack = 0f;
            CallAttackEvent();
        }

        /*if (isAttacking)
        {
            CallMoveEvent();
            return;
        }*/
    }

    public void CallMoveEvent()
    {
        direction = (targetCharacter.transform.position - transform.position).normalized;

        OnMoveEvent?.Invoke(direction);
    }

    public void CallAttackEvent()
    {
        OnAttackEvent?.Invoke();
    }

    public void ResetTarget()
    {

    }

    public bool FindTarget()
    {
        targetCharacter = BattleManager.Instance.GetClosestTarget(this);

        if (targetCharacter == null)
        {
            return false;
        }

        target = targetCharacter.transform;

        isFightWithTarget = true;

        return true;
    }

    //타겟이 사정거리 내에 있는지 체크
    public bool IsTargetInRange()
    {
        if (targetCharacter == null)
        {
            return false;
        }

        return Vector2.Distance(transform.position, targetCharacter.transform.position) < attackRange;
    }
}
