using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseCharacter : MonoBehaviour
{
    //얘네들 어웨이크 셋팅은 아직임
    public ActionHandler attackHandler;
    public ActionHandler skillHandler;
    public HealthSystem healthSystem;

    public bool isLive;
    public Vector2 direction;
    public BaseCharacter targetCharacter;

    public CharacterMovement characterMovement;

    public bool isGoingToTarget = false;
    public bool isPlayerCharacter = false;
    public bool isFightWithTarget = false;

    public Action<Vector2> OnMoveEvent;
    public Action OnAttackEvent;
    public Action OnDieEvent;

    private float timeSinceLastAttack = float.MaxValue;

    //나중에 SO 화 할 것들
    public float maxHP;
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
        attackHandler = GetComponent<ActionHandler>();
        skillHandler = GetComponent<ActionHandler>();
        healthSystem = GetComponent<HealthSystem>();

        CharacterInit();
    }

    public void CharacterInit()
    {
        isLive = true;
        healthSystem.maxHP = maxHP;
        healthSystem.currentHP = healthSystem.maxHP;
        characterMovement.moveSpeed = moveSpeed;
        stateMachine.ChangeState(stateMachine.IdleState);

        OnDieEvent += CharacterDeActive;
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

    public bool IsAttackReady()
    {
        return attackHandler.IsCooldownComplete();
    }

    public bool IsSkillReady()
    {
        return skillHandler.IsCooldownComplete();
    }

    public void PerformAttack()
    {
        if (targetCharacter == null || attackHandler == null)
        {
            Debug.Log("공격 시 타겟 캐릭터가 Null");
            return;
        }
        attackHandler.ExecuteAction(targetCharacter);
    }

    public void UseSkill()
    {
        skillHandler.ExecuteAction(targetCharacter);
    }

    public bool FindTarget()
    {
        targetCharacter = BattleManager.Instance.GetClosestTarget(this);

        if (targetCharacter == null)
        {
            return false;
        }

        isFightWithTarget = true;

        return true;
    }

    //타겟이 사정거리 내에 있는지 체크
    public bool IsTargetInRange()
    {
        //타겟이 살아있는지도 추가
        if (targetCharacter == null || !targetCharacter.isLive)
        {
            return false;
        }

        return Vector2.Distance(transform.position, targetCharacter.transform.position) < attackRange;
    }

    public void CharacterDeActive()
    {
        isLive = false;
        gameObject.SetActive(false);
    }

    public void CallDieEvent()
    {
        OnDieEvent?.Invoke();
    }
}
