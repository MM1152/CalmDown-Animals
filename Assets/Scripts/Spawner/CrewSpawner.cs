using UnityEngine;

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
        movedCrew = Instantiate(prefabs , transform);
        movedCrew.Spawn();
    }
}
