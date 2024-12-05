using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class BaseUnit : MonoBehaviour
{
    //Unit's Component
    public HealthSystem healthSystem;
    public ActionHandler attackHandler;
    public ActionHandler skillHandler;
    public UnitMovement UnitMovement;
    public UnitAnimationController animController;

    public bool isLive;
    public Vector2 direction;
    public BaseUnit targetUnit;

    //플레이어면 true , 적이면 false
    public bool isPlayerUnit = false;

    private bool isSkillExecuting;

    public Action <BaseUnit> OnDieEvent;

    //나중에 SO 화 할 것들
    public float maxHP;
    public float moveSpeed;
    public float skillDelay;
    public float attackRange;
    public float attackDelay;

    public StateMachine stateMachine;


    //For Debug
    [SerializeField] private string CurrentState;


    protected virtual void Awake()
    {

        healthSystem = GetComponent<HealthSystem>();
        skillHandler = GetComponent<SkillHandler>();
        attackHandler = GetComponent<ActionHandler>();
        UnitMovement = GetComponent<UnitMovement>();
        animController = GetComponent<UnitAnimationController>();
    }

    //Start 에서 호출됨
    public void UnitInit()
    {
        isLive = true;
        healthSystem.MaxHP = maxHP;
        UnitMovement.moveSpeed = moveSpeed;

        //배틀매니저에 캐릭터 등록
        BattleManager.Instance.RegisterUnit(this);

        
        OnDieEvent += UnitDeActive;
        OnDieEvent += BattleManager.Instance.UnitDie;

        //나중에 셋캐릭터로 이동
        animController.SetSettingAnimation();

        //애니컨트롤러 때문에 여기로 이동 나중에 생각해보기
        stateMachine = new StateMachine(this);
    }

    //캐릭터 활동 시작 = 배틀 시작 = 지금은 배틸매니저가 호출
    public void ActiveUnit()
    {
        //Idle 상태로 바꾸는것도 다른 준비가 끝나고 하는게 좋을거같음
        stateMachine.ChangeState(stateMachine.IdleState);

        //평타와 스킬 쿨타임 초기화
        skillHandler.ResetCooldown();
        attackHandler.ResetCooldown();
    }

    //유닛을 타일에 배치(셋팅) 햇을때
    public void SetUnit()
    {
    }

    //유닛을 타일에서 해제했을때 = 현재 구조로는 프리팹자체 파괴
    public void UnsetUnit()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        UnitInit();
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();

        //For Debug
        CurrentState = stateMachine?.GetState();
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
        if (targetUnit == null || attackHandler == null)
        {
            Debug.Log("공격 시 타겟 캐릭터가 Null");
            return;
        }

        attackHandler.ExecuteAction(targetUnit);
    }

    public void UseSkill()
    {
        if (targetUnit == null)
        {
            Debug.Log("스킬 사용시 타겟 캐릭터가 Null");
            return;
        }

        isSkillExecuting = true;
        skillHandler.ExecuteAction(targetUnit);
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
        targetUnit = BattleManager.Instance.GetTargetClosestOpponent(this);

        //targetUnit = BattleManager.Instance.targetting

        if (targetUnit == null)
        {
            return false;
        }

        return true;
    }

    //타겟이 사정거리 내에 있는지 체크
    public bool IsTargetInRange()
    {
        //타겟이 살아있는지도 추가
        if (targetUnit == null || !targetUnit.isLive)
        {
            return false;
        }

        return Vector2.Distance(transform.position, targetUnit.transform.position) < attackRange;
    }

    

    public void UnitDeActive(BaseUnit Unit)
    {
        isLive = false;
        gameObject.SetActive(false);
    }

    public void CallDieEvent()
    {
        OnDieEvent?.Invoke(this);
    }
}
