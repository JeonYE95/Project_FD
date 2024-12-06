using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public static class AnimationData
{
    // Boolean 파라미터 (플레이어용)
    public static readonly int isIdle = Animator.StringToHash("isIdle");
    public static readonly int isMoving = Animator.StringToHash("isMoving");
    public static readonly int isWaiting = Animator.StringToHash("isWaiting");
    public static readonly int isAttacking = Animator.StringToHash("isAttacking");

    // Integer 파라미터 (몬스터용)
    public static readonly int IdleState = 0;   // Idle 애니메이션은 State = 0
    public static readonly int WalkState = 2;  // Walk 애니메이션은 State = 2
    public static readonly int RunState = 3;   // Run 애니메이션은 State = 3
    public static readonly int DeathState = 9; // Death 애니메이션은 State = 9
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

        if (_myUnit is PlayerUnit)
        {
            animator.runtimeAnimatorController = animController;
        }
    }

    // Boolean 파라미터 설정
    public void SetBool(int hashCode, bool value)
    {
        if (animator == null) return;
        animator.SetBool(hashCode, value);
    }

    // Integer 파라미터 설정
    public void SetState(int stateValue)
    {
        if (animator == null) return;
        animator.SetInteger("State", stateValue);
    }

    // Trigger 파라미터 설정
    public void SetTrigger(int hashCode)
    {
        if (animator == null) return;
        animator.SetTrigger(hashCode);
    }

    // 애니메이션 상태 디버깅
    public void DebugAnimationState()
    {
        if (animator == null) return;

        Debug.Log($"{gameObject.name}: 현재 상태: {animator.GetCurrentAnimatorStateInfo(0).shortNameHash}");
    }
}
