using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonTextMove : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    TextMeshProUGUI text;

    Vector2 startPos;

    void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        startPos = text.rectTransform.anchoredPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        text.rectTransform.anchoredPosition = startPos;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        text.rectTransform.anchoredPosition = startPos + Vector2.down * 12;
    }
}
