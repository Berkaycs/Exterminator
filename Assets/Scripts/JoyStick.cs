using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public delegate void OnStickInputValueChanged(Vector2 inputValue);

    public event OnStickInputValueChanged OnStickValueChanged;

    [SerializeField] private RectTransform _thumbStickTransform;
    [SerializeField] private RectTransform _backgroundTransform;
    [SerializeField] private RectTransform _centerTransform;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchPos = eventData.position;
        Vector2 centerPos = _backgroundTransform.position;

        // restrict the movement of stick
        Vector2 localOffset = Vector2.ClampMagnitude(touchPos - centerPos, _backgroundTransform.sizeDelta.x / 2);

        Vector2 inputValue = localOffset / _backgroundTransform.sizeDelta.x / 2; // normalized the input value

        _thumbStickTransform.position = centerPos + localOffset;
        OnStickValueChanged?.Invoke(inputValue);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _backgroundTransform.position = eventData.position;
        _thumbStickTransform.position = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _backgroundTransform.position = _centerTransform.position;
        _thumbStickTransform.position = _backgroundTransform.position;

        OnStickValueChanged?.Invoke(Vector2.zero);
    }
}
