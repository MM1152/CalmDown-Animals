using UnityEngine;
using System.Collections.Generic;


public class PathTile : Tile
{
    public int G { get; set; } = 100000;
    public int H { get; set; }
    public int F => G + H;
    public List<PathTile> Neighbor = new List<PathTile>();

    public PathTile ParentTile { get; set; } = null;
    public DrawTile ArriveDrawTile { get; set; }
    public SpawnEnemyInfo EnemyInfo { get; set; }
    private float enablePercent = 0f;

    public bool IsSelectedPath { get; set; }

    public static int operator -(PathTile x , PathTile y)
    {
        return (int)Mathf.Round(Mathf.Abs(x.transform.position.x - y.transform.position.x) + Mathf.Abs(x.transform.position.z - y.transform.position.z));
    } 

    protected override void Awake()
    {
        base.Awake();
        Type = TileType.None;
    }

    public void UpdatePathTile(Map.DrawData data)
    {
        transform.position = data.Position;
        transform.eulerAngles = data.Rotation;
    }
}
