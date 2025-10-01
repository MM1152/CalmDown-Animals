using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class PathTile : Tile
{
    public int G { get; set; } = 100000;
    public int H { get; set; }
    public int F => G + H;
    public List<PathTile> Neighbor = new List<PathTile>();
    public Crew crew;

    public override TileType Type { 
        get => base.Type;
        set
        {
            base.Type = value;

            if(((Type & (TileType.Path | TileType.Blocked | TileType.Crew)) > 0) || !Variable.onTree)
            {
                ClearDesignTile();
            }
            else
            {
                ShowDesignTile();
            }
        }
    }

    private TileManager tileManager;
    public PathTile ParentTile { get; set; } = null;
    public DrawTile ArriveDrawTile { get; set; }
    public SpawnEnemyInfo EnemyInfo { get; set; }
    public TextMeshProUGUI changeToBlockedPercent;

    public bool IsSelectedPath { get; set; }
    public bool SetAbleBlockedTile { get; set; }
    public int CrewKillCount { get; set; }

    public GameObject[] designObjets;
    public int designIdx = -2;

    public static int operator -(PathTile x , PathTile y)
    {
        return (int)Mathf.Round(Mathf.Abs(x.transform.position.x - y.transform.position.x) + Mathf.Abs(x.transform.position.z - y.transform.position.z));
    } 

    protected override void Awake()
    {
        base.Awake();
    }

    public void UpdatePathTile(Map.DrawData data , TileManager tileManager)
    {
        designIdx = Random.Range(-1, designObjets.Length);
        Type = TileType.None;

        ShowDesignTile();

        transform.position = data.Position;
        transform.eulerAngles = data.Rotation;

        this.tileManager = tileManager;
    }

    private void ShowDesignTile()
    {
        if (designIdx <= -1 || !Variable.onTree) return;

        designObjets[designIdx].SetActive(true);
    }

    private void ClearDesignTile()
    {
        if (designIdx <= -1) return;

        designObjets[designIdx].SetActive(false);
    }

    public void ChangeColor()
    {
        if (Type == TileType.Path)
        {
            material[0].color = Color.green;
            
        }
    }
    public void ResetColor()
    {
        if (Type == TileType.Path)
        {
            material[0].color = Color.white;
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
                SetAbleBlockedTile = false;
                return;
            }
        }
    }
}
