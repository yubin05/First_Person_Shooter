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
}
