using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public static class AnimationData
{
    public static readonly int isIdle = Animator.StringToHash("isIdle");
    public static readonly int isMoving = Animator.StringToHash("isMoving");
    public static readonly int isWaiting = Animator.StringToHash("isWaiting");
    public static readonly int isAttacking = Animator.StringToHash("isAttacking");
}

public class UnitAnimationController : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        //animator.runtimeAnimatorController = f;
        SetAnimator();
    }

    public void SetAnimator()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetBool(int hashCode, bool isPlaying)
    {
        animator?.SetBool(hashCode, isPlaying);
    }

    public void SetTrigger(int hashCode)
    {
        animator?.SetTrigger(hashCode);
    }

    /*public void SetSettingAnimation()
    {
        animator?.SetBool(isSetting, true);
    }

    public void SetIdleAnimation()
    {
        animator?.SetBool(isIdle, true);
    }

    public void SetMoveAnimation()
    {
        animator?.SetBool(isMoving, true);
    }

    public void SetAttackAnimation()
    {
        animator?.SetBool(isAttacking, true);
    }*/
}
