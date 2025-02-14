using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MotionHandler : MonoBehaviour
{
    private Animator animator;

    public bool IsMove { get; private set; }
    public event Action MoveEvent;

    public bool IsAttack { get; private set; }
    public event Action AttackEvent;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Init()
    {
        IsMove = false;
        MoveEvent = null;
    }

    // 이동 시작
    public void StartMove()
    {
        IsMove = true;
        animator.SetBool(nameof(IsMove), IsMove);
    }
    // 이동 이벤트
    public void OnMove()
    {
        MoveEvent?.Invoke();
    }
    // 이동 끝
    public void EndMove()
    {
        IsMove = false;
        animator.SetBool(nameof(IsMove), IsMove);
    }

    // 공격 시작
    public void StartAttack()
    {
        IsAttack = true;
        animator.SetBool(nameof(IsAttack), IsAttack);
        animator.SetTrigger(nameof(OnAttack));
    }
    // 공격 이벤트
    public void OnAttack()
    {
        AttackEvent?.Invoke();
    }
    // 공격 끝
    public void EndAttack()
    {
        IsAttack = false;
        animator.SetBool(nameof(IsAttack), IsAttack);
    }
}
