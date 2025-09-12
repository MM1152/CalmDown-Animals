using UnityEngine;
using System.Collections.Generic;

public class CrewSpawner : MonoBehaviour
{
    public Crew prefabs;
    private Crew movedCrew;

    public bool IsSpawn
    {
        get
        {
            if (movedCrew == null) return false;
            return movedCrew.DragAble; 
        }
    }
    public void Spawn()
    {
        if (Crew.hireCount < 0 && Crew.hireCount - Crew.placeCount <= 0) return;

        movedCrew = Instantiate(prefabs , transform);
        movedCrew.Spawn();
    }
    public void Hired()
    {
        Crew.Hire();
    }
}
