using System;

/// <summary>
/// 해당 인터페이스를 상속받는 클래스는 무기 교체를 할 수 있는 클래스를 의미
/// </summary>
public interface IChangeWeapon
{
    public void OnTake<T, K>(int weaponId) where T : WeaponInfo where K : WeaponObject;
}
