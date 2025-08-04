using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffecter : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeSpeed = 2f;

    private void Update()
    {
        if (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, Time.deltaTime * fadeSpeed);
        }
    }

    public void Show(float alpha)
    {
        canvasGroup.alpha = alpha;
    }
}
