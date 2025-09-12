using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CrewSpawner : MonoBehaviour
{
    public Crew prefabs;
    public Crew DragCrew { get; set; }
    public bool IsSpawn
    {
        get
        {
            if (DragCrew == null) return false;
            return DragCrew.DragAble; 
        }
    }
    public void Spawn()
    {
        if (Crew.hireCount < 0 && Crew.hireCount - Crew.placeCount <= 0) return;

        DragCrew = Instantiate(prefabs , transform);
        DragCrew.Spawn(this);
    }
    public void Hired()
    {
        Crew.Hire();
    }
}
