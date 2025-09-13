using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnCrewEvent : MonoBehaviour , IEndDragHandler ,IDragHandler , IPointerClickHandler
{
    public CrewRank rank;

    public CrewManager spawner;
    public bool spawn = false;

    public TextMeshProUGUI hireText;
    public TextMeshProUGUI placedText;

    public void OnDrag(PointerEventData eventData)
    {
        if (spawner.GetHireCount(rank) - spawner.GetPlaceCount(rank) > 0 && TouchManager.TouchType == TouchType.Drag && !spawn)
        {
            spawner.Spawn(rank);
            spawn = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        spawn = false;
        placedText.text = spawner.GetPlaceCount(rank) + "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(TouchManager.TouchType == TouchType.Tab && spawner.CrewHire(rank))
        {
            hireText.text = spawner.GetHireCount(rank) + "";
        }
    }

}
