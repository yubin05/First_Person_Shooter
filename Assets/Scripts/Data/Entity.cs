using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : Data
{
    public EntityObject MyObject { get; protected set; }
    public float LifeTime { get; protected set; } = -1f;

    public virtual void Init(EntityObject myObject)
    {
        MyObject = myObject;
        StartLifeTime(LifeTime);
    }

    private Coroutine LifeTimeCoroutine;
    public void StartLifeTime(float lifeTime)
    {
        if (lifeTime <= 0) return;  // 생명 시간이 0 이하면 잘못 들어온 값으로 판단하여 리턴
        LifeTime = lifeTime;

        if (LifeTimeCoroutine != null) MyObject.StopCoroutine(LifeTimeCoroutine);
        LifeTimeCoroutine = MyObject.StartCoroutine(LifeTimeProcess(lifeTime));
    }
    private IEnumerator LifeTimeProcess(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        RemoveData();
    }
}
