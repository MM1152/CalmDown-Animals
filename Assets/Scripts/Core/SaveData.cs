using System.Collections.Generic;
using UnityEngine;

public abstract class SaveData
{
    public int Version { get; set; }
    public abstract SaveData VersionUp();
}

public class SaveDataV1 : SaveData
{
    public SaveDataV1()
    {
        Version = 1;
    }

    public int mapid;
    public int mapSize;
    public Dictionary<CrewRank, List<Vector3>> crewSpawn = new Dictionary<CrewRank, List<Vector3>>();
    public Dictionary<CrewRank, (int , int)> employCrewCount = new Dictionary<CrewRank, (int , int)>();
    public List<Vector3> pathTile = new List<Vector3>();
    public int gold;
    public int wave;
    public override SaveData VersionUp()
    {
        throw new System.NotImplementedException();
    }
}