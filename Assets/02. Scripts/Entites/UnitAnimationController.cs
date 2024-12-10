using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public static class PlayerAnimData
{
    // Boolean 파라미터 (플레이어용)
    public static readonly int isIdle = Animator.StringToHash("isIdle");
    public static readonly int isMoving = Animator.StringToHash("isMoving");
    public static readonly int isWaiting = Animator.StringToHash("isWaiting");
    public static readonly int isAttacking = Animator.StringToHash("isAttacking");
}

public static class EnemyAnimData
{
    // Integer 파라미터 (몬스터용)
    public static readonly int Attack = Animator.StringToHash("Attack");
    public static readonly int IdleState = 0;
    public static readonly int ReadyState = 1;
    public static readonly int WalkState = 2;
    public static readonly int RunState = 3;
    public static readonly int DeathState = 9; 
}

public class UnitAnimationController : MonoBehaviour
{
    BaseUnit _myUnit;
    Animator animator;

    [SerializeField] private AnimatorController animController;

    private void Awake()
    {
        _myUnit = GetComponent<BaseUnit>();
    }

    //Awake에서는 아직 애니메이션 에셋이 달리지 않음
    void Start()
    {
        SetAnimator();
    }

    public void SetAnimator()
    {
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogWarning($"Animator가 {gameObject.name}에 연결되지 않았습니다.");
            return;
        }

        //각각의 프리팹(에셋)마다 해야되는 동작이 다름
        //플레이어 에셋은 컨트롤러 교체 필요, 몬스터는 어택트리거 초기화필요
        if (_myUnit is PlayerUnit)
        {
            animator.runtimeAnimatorController = animController;
        }
        else if (_myUnit is EnemyUnit)
        {
            animator.ResetTrigger(EnemyAnimData.Attack);
        }
    }

    // Bool 파라미터 설정
    public void SetBool(int hashCode, bool value)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(hashCode, value);
    }

    // Int 파라미터 설정
    public void SetState(int stateValue)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetInteger("State", stateValue);
    }

    // Trigger 파라미터 설정
    public void SetTrigger(int hashCode)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetTrigger(hashCode);
    }

    // 애니메이션 상태 디버깅
    public void DebugAnimationState()
    {
        if (animator == null)
        {
            return;
        }

        Debug.Log($"{gameObject.name}: 현재 상태: {animator.GetCurrentAnimatorStateInfo(0).shortNameHash}");
    }

    public void ResetAttackTrigger()
    {
        if (_myUnit is PlayerUnit)
        {
            animator.ResetTrigger(PlayerAnimData.isAttacking);
        }
        else if (_myUnit is EnemyUnit)
        {
            animator.ResetTrigger(EnemyAnimData.Attack);
        }
    }
}
