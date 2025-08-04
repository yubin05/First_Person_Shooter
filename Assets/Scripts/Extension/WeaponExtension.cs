using System.Collections;
using System.Collections.Generic;

public static class WeaponExtension
{
    public static WeaponObject TakeWeapon(this CharacterObject characterObject, int weaponId)
    {
        WeaponObject weaponObj = null;

        if (GameApplication.Instance.GameModel.PresetData.ReturnData<GunInfo>(nameof(GunInfo), weaponId) is GunInfo gunInfo)
        {
            if (gunInfo.WeaponType == Define.EWeaponType.Rifle)
            {
                weaponObj = characterObject.OnTake<GunInfo, RifleObject>(weaponId);
            }
            else if (gunInfo.WeaponType == Define.EWeaponType.Pistol)
            {
                weaponObj = characterObject.OnTake<GunInfo, PistolObject>(weaponId);
            }
        }
        else if (GameApplication.Instance.GameModel.PresetData.ReturnData<KnifeInfo>(nameof(KnifeInfo), weaponId) != null)
        {
            weaponObj = characterObject.OnTake<KnifeInfo, KnifeObject>(weaponId);
        }

        return weaponObj;
    }
}
