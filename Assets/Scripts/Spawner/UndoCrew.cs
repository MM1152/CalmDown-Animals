using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UndoCrew : MonoBehaviour
{
    private CrewManager crewManager;
    private List<CrewRank> undoList = new List<CrewRank>();
    private List<PathTile> tiles = new List<PathTile>();

    public Button undoCrewButton;
    
    private void Awake()
    {
        crewManager = GetComponent<CrewManager>();
    }

    private void Start()
    {
        undoCrewButton.onClick.AddListener(() =>
        {
            crewManager.ClearAllCrews();

            for(int i =0; i < undoList.Count; i++)
            {
                if (crewManager.GetHireCount(undoList[i]) - crewManager.GetPlaceCount(undoList[i]) <= 0) continue;
                crewManager.CrewForcingSpawn(undoList[i] , tiles[i]);
            }
        });
    }

    public void UpdateUndoCrewList(in List<Crew> undoList)
    {
        this.undoList = undoList.Select(x => x.Rank).ToList();
        this.tiles = undoList.Select(x => x.UnderTile).ToList();
    }
}
