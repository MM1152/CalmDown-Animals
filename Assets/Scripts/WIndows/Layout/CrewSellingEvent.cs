using UnityEngine;
using UnityEngine.EventSystems;

public class CrewSellingEvent : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public CrewSpawner spanwer;

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Crew.Sell();
    }
}
