using UnityEngine;
using UnityEngine.EventSystems;

public class CrewSellingEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CrewManager spanwer;
    public bool SellAble { get; private set; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SellAble = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SellAble = false;
    }
}
