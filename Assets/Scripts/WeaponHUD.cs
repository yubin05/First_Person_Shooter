using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

public class WeaponHUD : View<WeaponHUDPresenter, WeaponHUDModel>
{
    [Header("텍스트")]
    public TextMeshProUGUI ammoTxt;

    [Header("캔버스 그룹")]
    [SerializeField] protected CanvasGroup aimCanvasGroup;
    public CanvasGroup AimCanvasGroup => aimCanvasGroup;
    [SerializeField] protected CanvasGroup hitCanvasGroup;
    public CanvasGroup HitCanvasGroup => hitCanvasGroup;

    //private Coroutine blinkCoroutine;

    public override void UpdateUI(WeaponHUDModel model)
    {
        UpdateAmmoTxt(model.CurMagazineCapacity, model.ReserveAmmo);
    }

    // 탄창 HUD 업데이트
    public void UpdateAmmoTxt(int curMagazineCapacity, int reserveAmmo)
    {
        var curMagazineCapacityStr = curMagazineCapacity == -1 ? "∞" : curMagazineCapacity.ToString();
        var reserveAmmoStr = reserveAmmo == -1 ? "∞" : reserveAmmo.ToString();
        ammoTxt.text = $"{curMagazineCapacityStr} / {reserveAmmoStr}";
    }

    // 명중 시, 조준점 깜빡임
    public async UniTaskVoid BlinkHitCanvas(float delayTime)
    {
        HitCanvasGroup.alpha = 1f;
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
        HitCanvasGroup.alpha = 0f;
    }
}

public class WeaponHUDPresenter : Presenter<WeaponHUDModel>
{
    public override void UpdateUI()
    {
        model.CurMagazineCapacity = -1;
        model.ReserveAmmo = -1;

        base.UpdateUI();
    }
}

public class WeaponHUDModel : Model
{
    public int CurMagazineCapacity { get; set; }
    public int ReserveAmmo { get; set; }
}
