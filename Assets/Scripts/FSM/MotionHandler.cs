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

    public bool IsReload { get; private set; }
    public event Action ReloadEvent;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Init()
    {
        IsMove = false;
        MoveEvent = null;

        IsAttack = false;
        AttackEvent = null;

        IsReload = false;
        ReloadEvent = null;
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

    // 재장전 시작
    public void StartReload()
    {
        IsReload = true;
        animator.SetBool(nameof(IsReload), IsReload);
        animator.SetTrigger(nameof(OnReload));
    }
    // 재장전 이벤트
    public void OnReload()
    {
        ReloadEvent?.Invoke();
    }
    // 재장전 끝
    public void EndReload()
    {
        IsReload = false;
        animator.SetBool(nameof(IsReload), IsReload);
    }
}
