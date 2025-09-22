using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEvent : MonoBehaviour, IPointerClickHandler
{
    public event Action<PointerEventData> PointerClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        PointerClick?.Invoke(eventData);
    }
}
