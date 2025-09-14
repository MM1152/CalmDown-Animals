using System;
using System.Collections;
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

    private Coroutine co;

    public void Start()
    {
        spawner.changeUnitCount += SetTexts;
    }

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
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        spawner.CrewHire(rank);
    }

    private void SetTexts()
    {
        placedText.text = spawner.GetPlaceCount(rank) + "";
        hireText.text = spawner.GetHireCount(rank) + "";
    }
}
