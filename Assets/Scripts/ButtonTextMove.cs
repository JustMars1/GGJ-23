using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonTextMove : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public float offset = 12;
    RectTransform text;

    Vector2 startPos;

    void Awake()
    {
        text = transform.GetChild(0).GetComponent<RectTransform>();
        startPos = text.anchoredPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        text.anchoredPosition = startPos;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        text.anchoredPosition = startPos + Vector2.down * offset;
    }
}
