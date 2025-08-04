using System.Collections;
using System.Collections.Generic;

public static class Define
{
    public const float HIT_EFFECT_FADE_SPEED = 2f;

    public const int DEFAULT_GUN_ID = (int)EPistolIdInfo.HK47Pistol;
    public enum EWeaponType
    {
        Rifle,
        Pistol,
        Knife,
    }
    public enum ERifleIdInfo
    {
        ScifiRazer = 90001,
    }
    public enum EPistolIdInfo
    {
        HK47Pistol = 90002,
    }
    public enum EKnifeIdInfo
    {
        ScifiKnife = 120001,
    }

    public const int DEFAULT_PLAYER_ID = (int)EPlayerIdInfo.Brian;
    public const int DEFAULT_ENEMY_ID = (int)EEnemyIdInfo.Bot_Jackie;
    public enum EPlayerIdInfo
    {
        Brian = 20001,
        Amy = 20002,
    }
    public enum EEnemyIdInfo
    {
        Bot = 30001,
        Bot_Jackie = 30002,
    }
}
