using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
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

    //플레이어면 true , 적이면 false
    public bool isPlayerCharacter = false;

    private bool isSkillExecuting;

    public Action <BaseCharacter> OnDieEvent;

    //나중에 SO 화 할 것들
    public float maxHP;
    public float moveSpeed;
    public float skillDelay;
    public float attackRange;
    public float attackDelay;

    public StateMachine stateMachine;


    //For Debug

    [SerializeField]
    public string CurrentState;


    private void Awake()
    {
        stateMachine = new StateMachine(this);

        healthSystem = GetComponent<HealthSystem>();
        skillHandler = GetComponent<SkillHandler>();
        attackHandler = GetComponent<ActionHandler>();
        characterMovement = GetComponent<CharacterMovement>();

    }

    //Start 에서 호출됨
    public void CharacterInit()
    {
        isLive = true;
        healthSystem.MaxHP = maxHP;
        characterMovement.moveSpeed = moveSpeed;

        //배틀매니저에 캐릭터 등록
        BattleManager.Instance.RegisterCharacter(this);

        
        OnDieEvent += CharacterDeActive;
        OnDieEvent += BattleManager.Instance.CharacterDie;
    }

    //캐릭터 활동 시작 = 배틀 시작
    public void ActiveCharacter()
    {
        //Idle 상태로 바꾸는것도 다른 준비가 끝나고 하는게 좋을거같음
        stateMachine.ChangeState(stateMachine.IdleState);

        //평타와 스킬 쿨타임 초기화
        skillHandler.ResetCooldown();
        attackHandler.ResetCooldown();
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

        //For Debug
        CurrentState = stateMachine?.GetState();
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
        if (targetCharacter == null)
        {
            Debug.Log("스킬 사용시 타겟 캐릭터가 Null");
            return;
        }

        isSkillExecuting = true;
        skillHandler.ExecuteAction(targetCharacter);
    }

    public bool IsSkillExecuting()
    {
        return isSkillExecuting;
    }

    public void EndSkill()
    {
        isSkillExecuting = false;
    }

    public bool FindTarget()
    {
        targetCharacter = BattleManager.Instance.GetTargetClosestOpponent(this);

        //targetCharacter = BattleManager.Instance.targetting

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

    public void CharacterDeActive(BaseCharacter character)
    {
        isLive = false;
        gameObject.SetActive(false);
    }

    public void CallDieEvent()
    {
        OnDieEvent?.Invoke(this);
    }
}
