using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 반동 시스템 - 특정 행동 후, 오브젝트의 위치가 흔들리는 현상
/// </summary>
public class ReactionSystem : MonoBehaviour
{
    [SerializeField] private ReactionData reactionData;
    public bool IsReact = false;    // 반동 중인지 체크

    public void React()
    {
        var originLocalRot = transform.localRotation.eulerAngles;
        var targetLocalRot = originLocalRot + reactionData.ReactVec;

        DOTween.Sequence().SetAutoKill(true).OnStart(() => 
        {
            IsReact = true;
        })
        .Append(transform.DOLocalRotate(targetLocalRot, reactionData.ReactTime))
        .Append(transform.DOLocalRotate(originLocalRot, reactionData.ReactTime))
        .OnComplete(() => 
        {
            IsReact = false;
        });
    }
}
