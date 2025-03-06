using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;

public class BaseUnit : MonoBehaviour
{
    //Unit's Field
    public bool isLive;
    public BaseUnit targetUnit;

    public IUnitInfo unitInfo;

    //Unit's Component
    public HealthSystem healthSystem;
    public ActionHandler actionHandler;
    public UnitMovement unitMovement;
    public UnitAnimationController animController;

    //공통된 이동속도
    float moveSpeed = 1.5f;

    //플레이어면 true , 적이면 false
    public bool isPlayerUnit = false;
    public bool isRangedUnit = false;


    //캐릭터 에셋 참조 형식으로 저장
    public GameObject unitAsset;
    public GameObject[] unitAssetChildren;

    private StateMachine stateMachine;

    //For Debug
    [SerializeField] private string CurrentState;
    public int ID;
    public int Defense;

    public Action <BaseUnit> OnDieEvent;

    protected virtual void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        //앞으로 이거 통합으로
        actionHandler = GetComponent<ActionHandler>();

        unitMovement = GetComponent<UnitMovement>();
        animController = GetComponent<UnitAnimationController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UnitInit();
    }

    //Start 에서 호출됨
    public virtual void UnitInit()
    {
        healthSystem.MaxHP = unitInfo.Health;

        unitMovement.moveSpeed = moveSpeed;

        if (unitInfo.Range >= 4f)
        {
            isRangedUnit = true;
        }

        //애니컨트롤러 때문에 여기로 이동 나중에 생각해보기
        stateMachine = new StateMachine(this);
    }

    //캐릭터 활동 시작 = 배틀 시작 = 지금은 배틀매니저가 호출
    public void UnitBattleStart()
    {
        //Idle 상태로 바꾸는것도 다른 준비가 끝나고 하는게 좋을거같음
        stateMachine.ChangeState(stateMachine.IdleState);

        //배틀스타트할때 초기화해야할것 해주는 함수
        animController.StartAnim();
    }

    //유닛을 타일에 배치(셋팅) 햇을때
    public void RegisterToBattleManager()
    {
        //배틀매니저에 캐릭터 등록
        BattleManager.Instance.RegisterUnit(this);

        //죽엇을 시 이벤트 등록
        OnDieEvent += UnitDeActive;
        OnDieEvent += BattleManager.Instance.UnitDie;

        ReSetUnit();
    }

    //플레이어는 배치 해제시, 몬스터는 배틀 끝났을 시
    public void UnregisterFromBattleManager()
    {
        if (OnDieEvent != null)
        {
            OnDieEvent -= UnitDeActive;

            OnDieEvent -= BattleManager.Instance.UnitDie;
        }

        if (isPlayerUnit)
        {
            BattleManager.Instance.UnRegisterUnit(this);
        }
    }


    public void ReSetUnit()
    {
        gameObject.SetActive(true);

        isLive = true;
        healthSystem.ResetHealth();

        ResetCoolTime();

        stateMachine?.ChangeState(stateMachine.StopState);

        //GetComponentInChildren<CharacterAnimation>().
        //ResetTransformRecursive(transform);

        if(this is PlayerUnit)
        {
            SetSortingOrder(Defines.PlayerSortingOrder);
        }
        else if (this is EnemyUnit)
        {
            SetSortingOrder(Defines.EnemySortingOrder);
        }
    }

    private void ResetTransformRecursive(Transform parent)
    {
        foreach (Transform child in parent)
        {
            //child.localPosition = Vector3.zero;
            child.localRotation = Quaternion.identity;

            // 자식의 자식도 초기화
            ResetTransformRecursive(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine?.Update();

        //For Debug
        CurrentState = stateMachine?.GetState();
        ID = unitInfo.ID;
        Defense = unitInfo.Defense;
    }

    public void ResetCoolTime()
    {
        //평타와 스킬 쿨타임 초기화
        actionHandler.ResetSkillCoolTime();
        actionHandler.ResetAttackCoolTime();
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
    }
    
    public void SetStun()
    {
        stateMachine.ChangeState(stateMachine.StopState);
    }

    public void SetIdle()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
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

        return Vector2.Distance(transform.position, targetUnit.transform.position) < (unitInfo.Range * 1.1f) + Defines.RANGEADJUST;
    }

    
    //유닛이 죽었을 경우 그 전투에서는 비활성화
    public void UnitDeActive(BaseUnit Unit)
    {
        isLive = false;
        stateMachine.ChangeState(stateMachine.DeathState);
        SetSortingOrder(Defines.BehindSortingOrder);
    }

    public void CallDieEvent()
    {
        OnDieEvent?.Invoke(this);
    }

    public void SetSortingOrder(int sortingOrderNumber)
    {
        unitAsset.GetComponentInChildren<SortingGroup>().sortingOrder = sortingOrderNumber;
    }

    public virtual void PlayWaitAnimation() {}

    public virtual void PlayIdleAnimation() {}

    public virtual void PlayMoveAnimation() {}

    public virtual void PlayAttackAnimation() {}

    public virtual void PlayDeathAnimation() {}
}
