using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 데미지 받는 오브젝트를 관장하는 시스템 클래스
/// 해당 오브젝트 인스펙터 창에 이 컴포넌트를 붙여서 사용
/// </summary>
[RequireComponent(typeof(HealthSystem))]
public class DamageSystem : MonoBehaviour
{
    private IDamageable damageable;

    private void Awake()
    {
        damageable = GetComponent<IDamageable>();
    }

    public void OnHit(float attackPower)
    {
        damageable.OnHit(attackPower);
    }
}
