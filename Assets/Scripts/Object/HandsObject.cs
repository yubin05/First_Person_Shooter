using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 1인칭 모드에서 사용하는 손 오브젝트
public class HandsObject : MonoBehaviour
{
    [SerializeField] private HandsData handsData;   // data

    [SerializeField] private Animator animator;
    public Animator Animator => animator;

    [SerializeField] private Transform weaponNode;
    public Transform WeaponNode => weaponNode;

    public WeaponObject WeaponObject { get; set; }

    private void Start()
    {
        handsData.NoAimLocalVec = transform.localPosition;
    }

    public void Aiming(bool isAiming, float aimTime)
    {
        if (isAiming) transform.DOLocalMove(handsData.AimLocalVec, aimTime);
        else transform.DOLocalMove(handsData.NoAimLocalVec, aimTime);
    }
}
