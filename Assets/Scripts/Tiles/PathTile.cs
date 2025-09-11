using UnityEngine;
using System.Collections.Generic;


public class PathTile : Tile
{
    public int G { get; set; } = 100000;
    public int H { get; set; }
    public int F => G + H;
    public List<PathTile> Neighbor  = new List<PathTile>();
    public PathTile ParentTile { get; set; } = null;
    

    private float enablePercent = 0f;

    public static int operator -(PathTile x , PathTile y)
    {
        return (int)Mathf.Round(Mathf.Abs(x.transform.position.x - y.transform.position.x) + Mathf.Abs(x.transform.position.z - y.transform.position.z));
    } 

    protected override void Awake()
    {
        base.Awake();
        Type = TileType.None;
    }
}
