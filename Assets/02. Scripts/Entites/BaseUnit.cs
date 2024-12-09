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
    public ActionHandler actionHandler;
    public UnitMovement UnitMovement;
    public UnitAnimationController animController;

    //Unit's Field
    public bool isLive;
    public BaseUnit targetUnit;
    public int defense;
    public int attackDamage;
    public int skillDamage;
    public int maxHP;
    public float skillCooltime;
    public float attackCooltime;
    public float attackRange;
    public string unitGrade;

    float moveSpeed = 1.5f;

    //플레이어면 true , 적이면 false
    public bool isPlayerUnit = false;
    public bool isRangedUnit = false;

    public GameObject attackProjectile;

    private bool isSkillExecuting;

    public Action <BaseUnit> OnDieEvent;


    private StateMachine stateMachine;


    //For Debug
    [SerializeField] private string CurrentState;


    protected virtual void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        //앞으로 이거 통합으로
        actionHandler = GetComponent<ActionHandler>();

        UnitMovement = GetComponent<UnitMovement>();
        animController = GetComponent<UnitAnimationController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UnitInit();
    }

    //Start 에서 호출됨
    public void UnitInit()
    {
        healthSystem.MaxHP = maxHP;
        UnitMovement.moveSpeed = moveSpeed;

        if (attackRange >= 4f)
        {
            isRangedUnit = true;
        }

        //배틀매니저에 캐릭터 등록
        BattleManager.Instance.RegisterUnit(this);

        //죽엇을 시 이벤트 등록
        OnDieEvent += UnitDeActive;
        OnDieEvent += BattleManager.Instance.UnitDie;

        //애니컨트롤러 때문에 여기로 이동 나중에 생각해보기
        stateMachine = new StateMachine(this);

        ReSetUnit();
    }

    //캐릭터 활동 시작 = 배틀 시작 = 지금은 배틀매니저가 호출
    public void UnitBattleStart()
    {
        //Idle 상태로 바꾸는것도 다른 준비가 끝나고 하는게 좋을거같음
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    //유닛을 타일에 배치(셋팅) 햇을때
    public void SetUnit()
    {

    }

    //유닛을 타일에서 해제했을때
    //플레이어는 Destory 고 몬스터는 오브젝트풀링이라 따로
    //플레이어는 걍 오브젝트 파괴됬을때 호출, 몬스터는 비활성화?? 어쩌지
    //이걸 내가 아니라 배치타일 하는 사람이 호출해달라고 해야할듯
    //플레이어는 하나하나 배치 해제 하면 언셋인데
    //몬스터는 웨이브 끝나면 전부 언셋 한 다음 다음 웨이브 ...?
    public void UnsetUnit()
    {
        OnDieEvent -= UnitDeActive;
        OnDieEvent -= BattleManager.Instance.UnitDie;

        BattleManager.Instance.UnRegisterUnit(this);
    }

    public void ReSetUnit()
    {
        isLive = true;
        healthSystem.ResetHealth();

        //평타와 스킬 쿨타임 초기화
        actionHandler.ResetSkillCoolTime();
        actionHandler.ResetAttackCoolTime();

        //아예 가만히 있는 애니메이션으로 셋팅
        animController.SetBool(PlayerAnimData.isWaiting, true);
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
        return actionHandler.IsAttackCoolTimeComplete();
    }

    public bool PerformAction()
    {
        if (targetUnit == null)
        {
            Debug.Log("액션 시 타겟 캐릭터가 Null");
        }
        return actionHandler.ExecuteAction(targetUnit);
    }

    public void UseSkill()
    {
        if (targetUnit == null)
        {
            Debug.Log("스킬 사용시 타겟 캐릭터가 Null");
            return;
        }

        isSkillExecuting = true;
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

    
    //유닛이 죽었을 경우 그 전투에서는 비활성화
    public void UnitDeActive(BaseUnit Unit)
    {
        isLive = false;
        gameObject.SetActive(false);
    }

    public void CallDieEvent()
    {
        OnDieEvent?.Invoke(this);
    }

    public virtual void PlayIdleAnimation() {}

    public virtual void PlayMoveAnimation() {}

    public virtual void PlayAttackAnimation() {}

    public virtual void PlayDeathAnimation() {}
}
