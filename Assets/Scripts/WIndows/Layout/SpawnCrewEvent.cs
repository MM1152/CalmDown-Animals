using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnCrewEvent : MonoBehaviour , IEndDragHandler ,IDragHandler , IPointerClickHandler
{
    public CrewSpawner spawner;
    public bool spawn = false;

    public TextMeshProUGUI hireText;
    public TextMeshProUGUI placedText;

    public void Start()
    {
        Crew.Init(x => placedText.text = x.ToString()
                , x => hireText.text = x.ToString());
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Crew.hireCount - Crew.placeCount > 0 && TouchManager.TouchType == TouchType.Drag && !spawn)
        {
            spawner.Spawn();
            spawn = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        spawn = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        spawner.Hired();
    }

}
