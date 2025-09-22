using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class PathTile : Tile
{
    public int G { get; set; } = 100000;
    public int H { get; set; }
    public int F => G + H;
    public List<PathTile> Neighbor = new List<PathTile>();

    private TileManager tileManager;
    public PathTile ParentTile { get; set; } = null;
    public DrawTile ArriveDrawTile { get; set; }
    public SpawnEnemyInfo EnemyInfo { get; set; }
    public TextMeshProUGUI changeToBlockedPercent;

    public bool IsSelectedPath { get; set; }
    public bool SetAbleBlockedTile { get; set; }
    public int CrewKillCount { get; set; }

    public static int operator -(PathTile x , PathTile y)
    {
        return (int)Mathf.Round(Mathf.Abs(x.transform.position.x - y.transform.position.x) + Mathf.Abs(x.transform.position.z - y.transform.position.z));
    } 

    protected override void Awake()
    {
        base.Awake();
        Type = TileType.None;
    }

    public void UpdatePathTile(Map.DrawData data , TileManager tileManager)
    {
        transform.position = data.Position;
        transform.eulerAngles = data.Rotation;

        this.tileManager = tileManager;
    }

    private void Update()
    {
        if(this.tileManager.InEditorWindow)
        {
            if(Type == TileType.Path)
            {
                material[0].color = Color.green;
            }
        }
        else
        {
            if (Type == TileType.Path)
            {
                material[0].color = Color.white;
            }
        }
    }

    public void UpdateBlockedTile()
    {
        foreach(var neighbor in Neighbor)
        {
            if (neighbor.SetAbleBlockedTile)
            {
                return;
            }
        }

        Type = TileType.Blocked;
        foreach (var neighbor in Neighbor)
        {
            if(neighbor.Type == TileType.Blocked)
            {
                neighbor.SetAbleBlockedTile = true;
                SetAbleBlockedTile = true;
                return;
            }
        }
    }

    public void DeleteBlockedTile()
    {
        Type = TileType.None;
        foreach (var neighbor in Neighbor)
        {
            if (neighbor.Type == TileType.Blocked)
            {
                neighbor.SetAbleBlockedTile = false;
                SetAbleBlockedTile = true;
                return;
            }
        }
    }
}
