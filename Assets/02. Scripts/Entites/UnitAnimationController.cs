using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerAnimData
{
    public static readonly int Death = Animator.StringToHash("Death");
    public static readonly int isIdle = Animator.StringToHash("isIdle");
    public static readonly int isMoving = Animator.StringToHash("isMoving");
    public static readonly int isWaiting = Animator.StringToHash("isWaiting");
    public static readonly int ResetAnim = Animator.StringToHash("ResetAnim");
    public static readonly int isAttacking = Animator.StringToHash("isAttacking");
}

public static class EnemyAnimData
{
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

    [SerializeField] private RuntimeAnimatorController animController; // RuntimeAnimatorController 사용

    private void Awake()
    {
        _myUnit = GetComponent<BaseUnit>();
    }

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

        //여기서는 진짜 몬스터 에너미 유닛은 다른 애니메이터기에 이 코드 유지
        //isPlayer 안씀
        if (_myUnit is PlayerUnit)
        {
            animator.runtimeAnimatorController = animController; // RuntimeAnimatorController 설정
        }
        else if (_myUnit is EnemyUnit)
        {
            animator.ResetTrigger(EnemyAnimData.Attack);
        }
    }

    public void SetBool(int hashCode, bool value)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(hashCode, value);
    }

    public void SetState(int stateValue)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetInteger("State", stateValue);
    }

    public void SetTrigger(int hashCode)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetTrigger(hashCode);
    }

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
            animator?.ResetTrigger(PlayerAnimData.isAttacking);
        }
        else if (_myUnit is EnemyUnit)
        {
            animator?.ResetTrigger(EnemyAnimData.Attack);
        }
    }

    public void ResetDeathTrigger()
    {
        if (_myUnit is PlayerUnit)
        {
            animator?.ResetTrigger(PlayerAnimData.Death);
        }
    }

    public void StartAnim()
    {
        if (_myUnit is PlayerUnit)
        {
            animator.ResetTrigger(PlayerAnimData.ResetAnim);
        }
    }

    public void ResetAnim()
    {
        // 필요 시 초기화 로직 추가
    }
}
