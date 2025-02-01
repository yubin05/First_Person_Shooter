using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

[AddComponentMenu("UI/Selectable Behavior")]
[RequireComponent(typeof(Image), typeof(Button))]
public class SelectableBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("Base")]
    [SerializeField] private TextMeshProUGUI[] texts;
    [SerializeField] private CanvasGroup clickDot;

    [Header("Settings")]
    [SerializeField] private float clickDotSize = 1000;
    [SerializeField] private float clickDotMovementSpeed = 2;
    [SerializeField] private float clickDotImageFadeSpeed = 5;
    [SerializeField] private float clickDotFadeSpeed = 20;

    [Header("Colors")]
    [SerializeField] private Color textNormalColor = Color.white;
    [SerializeField] private Color textHighlightedColor = Color.gray;
    [Space]
    [SerializeField] private Color backgroundNormalColor = Color.white;
    [SerializeField] private Color backgroundHighlightedColor = Color.gray;

    public event Action PointerEnterEvent;
    public event Action ButtonDownEvent;

    private bool isOn;
    private Selectable selectable;
    private RectTransform clickDotTransform;
    private Image background;

    private void Awake()
    {
        selectable = GetComponent<Selectable>();
        background = GetComponent<Image>();

        isOn = false;
        PointerEnterEvent = null;
        ButtonDownEvent = null;        

        if (clickDot)
        {
            clickDotTransform = clickDot.GetComponent<RectTransform>();
            clickDotTransform.sizeDelta = Vector2.one * clickDotSize;
            clickDot.alpha = 0;
        }
    }
    private void Start()
    {
        foreach (TextMeshProUGUI text in texts)
        {
            if (text) text.color = textNormalColor;
        }
    }

    private void Update()
    {
        if (selectable && !selectable.interactable) return;

        if (isOn) Hightlight(); else ReturnToNormal();

        if (clickDot)
        {
            clickDotTransform.sizeDelta = Vector2.Lerp(clickDotTransform.sizeDelta, Vector2.one * clickDotSize, Time.unscaledDeltaTime * clickDotMovementSpeed);
            clickDot.alpha = Mathf.Lerp(clickDot.alpha, 0, Time.unscaledDeltaTime * clickDotImageFadeSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (selectable && !selectable.interactable) return;

        isOn = true;
        PointerEnterEvent?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (selectable && !selectable.interactable) return;

        isOn = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (selectable && !selectable.interactable) return;

        if (clickDot)
        {
            clickDotTransform.position = new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue());
            clickDotTransform.sizeDelta = Vector2.zero;
            clickDot.alpha = 1;
        }
        ButtonDownEvent?.Invoke();
    }

    private void Hightlight()
    {
        foreach (TextMeshProUGUI text in texts)
        {
            if (text)
                text.color = Color.Lerp(text.color, textHighlightedColor, Time.unscaledDeltaTime * clickDotFadeSpeed);
        }

        if(background)
            background.color = Color.Lerp(background.color, backgroundHighlightedColor, Time.unscaledDeltaTime * clickDotFadeSpeed);
    }

    private void ReturnToNormal()
    {
        foreach (TextMeshProUGUI text in texts)
        {
            if (text)
                text.color = Color.Lerp(text.color, textNormalColor, Time.unscaledDeltaTime * clickDotFadeSpeed);
        }

        if (background)
            background.color = Color.Lerp(background.color, backgroundNormalColor, Time.unscaledDeltaTime * clickDotFadeSpeed);
    }
}