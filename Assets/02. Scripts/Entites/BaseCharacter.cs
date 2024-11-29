using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseCharacter : MonoBehaviour
{
    //Character's Component
    public HealthSystem healthSystem;
    public ActionHandler attackHandler;
    public ActionHandler skillHandler;
    public CharacterMovement characterMovement;

    public bool isLive;
    public Vector2 direction;
    public BaseCharacter targetCharacter;

    //플레이어 면 true , 적이면 false
    public bool isPlayerCharacter = false;

    public Action OnDieEvent;

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

    }

    public void CharacterInit()
    {
        isLive = true;
        healthSystem.maxHP = maxHP;
        healthSystem.currentHP = healthSystem.maxHP;
        characterMovement.moveSpeed = moveSpeed;

        if (isPlayerCharacter)
        {
            BattleManager.Instance.players.Add(this);
        }
        else
        {
            BattleManager.Instance.enemies.Add(this);
        }

        
        OnDieEvent += CharacterDeActive;
    }

    public void ActiveCharacter()
    {
        //Idle 상태로 바꾸는것도 다른 준비가 끝나고 하는게 좋을거같음
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    // Start is called before the first frame update
    void Start()
    {
        CharacterInit();
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
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

    public void ResetCharacter()
    {

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
