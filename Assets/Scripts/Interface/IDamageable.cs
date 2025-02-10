using System;

/// <summary>
/// 데미지를 받을 수 있는 오브젝트를 정의?하는 인터페이스
/// 해당 인터페이스를 상속받는 클래스는 데미지를 받을 수 있는 클래스를 의미
/// </summary>
public interface IDamageable
{
    public HealthSystem HealthSystem { get; set; }
    public void OnHit(float attackPower);
}
