using CsvHelper;
using JetBrains.Annotations;
using UnityEngine;

public class PathTileRoad : MonoBehaviour
{
    public enum Sides
    {
        None = -1,
        TopRight,
        TopLeft,
        Left,
        BottomLeft,
        BottomRight,
        Right,
    }

    public PathTileRoad nextTile;

    public GameObject roundRoad;
    public GameObject cornerRoad;
    public GameObject straightRoad;

    private GameObject currentDrawRoad;

    public Sides prevSide;
    //미리 오브젝트 돌려주기 용 프로퍼티
    public Sides PrevSide
    {
        get => prevSide;
        set
        {
            prevSide = value;
            Vector3 angle = new Vector3(0f, 120f - (60f * (int)prevSide) , 0f);
            roundRoad.transform.eulerAngles = angle;
            cornerRoad.transform.eulerAngles = angle;
            straightRoad.transform.eulerAngles= angle;
        }
    }
    public void DrawRoad(PathTileRoad nTile)
    {
        if (nTile == null) return;

        this.nextTile = nTile;
        Sides nextSide = FindSide(transform.position, nTile.transform.position);
        SetNextTilePrevSide(nTile , nextSide);

        if(nextSide == PrevSide)
        {
            straightRoad.SetActive(true);
        }
        else
        {
            cornerRoad.SetActive(true);
           // cornerRoad.transform.eulerAngles = cornerRoad.transform.eulerAngles + new Vector3(0f, -120f, 0f); 
        }
    }

    public void SetNextTilePrevSide(PathTileRoad nTile , Sides nextSide)
    {
        nTile.PrevSide = nextSide;
    }

    public static Sides FindSide(Vector3 prevTile, Vector3 nTile)
    {
        Vector3 side = nTile - prevTile;
        Debug.Log(side);
        for(int i = 0; i < TileManager.neighborPosition.nextNeighborPos.Length; i++)
        {
            if(side == TileManager.neighborPosition.nextNeighborPos[i])
            {
                return (Sides)i;
            }
        }

        return Sides.None;
    }
    public void Clear()
    {
        roundRoad.SetActive(false);
        cornerRoad.SetActive(false);
        straightRoad.SetActive(false);
    }
}
