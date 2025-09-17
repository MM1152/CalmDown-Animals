using CsvHelper;
using JetBrains.Annotations;
using System;
using UnityEngine;

public class PathTileRoad : MonoBehaviour
{
    public enum Sides
    {
        None = -1,
        TopRight ,
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
    public Sides nextSide;
    //미리 오브젝트 돌려주기 용 프로퍼티
    public Sides PrevSide
    {
        get => prevSide;
        set
        {
            prevSide = value;
            Vector3 angle = new Vector3(0f, 120 - (60f * (int)prevSide), 0f);
            straightRoad.transform.eulerAngles = angle;
        }
    }
    public void DrawRoad(PathTileRoad nTile)
    {
        //LEFT = 120
        //BOTTOMLEFT = 180
        if (nTile == null) return;

        this.nextTile = nTile;
        nextSide = FindSide(transform.position, nTile.transform.position);
        SetNextTilePrevSide(nTile , nextSide);

        DrawRoads();
    }

    public void DrawRoad(Vector3 nPos)
    {
        nextSide = FindSide(transform.position, nPos);
        DrawRoads();
    }

    private void DrawRoads()
    {
        if (nextSide == PrevSide)
        {
            straightRoad.SetActive(true);
        }
        else
        {
            // LEFT -> BOTTOMLEFT = 0
            // BOTTOMLEFT -> LEFT = 180
            // LEFT -> TOPLEFT = 240
            // TOPLEFT -> TOPRIGHT = 300
            // TOPRIGHT -> TOPLEFT = 120
            // TOPLEFT -> LEFT = 60

            if (prevSide == Sides.BottomLeft && nextSide == Sides.Left)
            {
                cornerRoad.transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else if (prevSide == Sides.Left && nextSide == Sides.TopLeft)
            {
                cornerRoad.transform.eulerAngles = new Vector3(0, 240, 0);
            }
            else if (prevSide == Sides.TopLeft && nextSide == Sides.TopRight)
            {
                cornerRoad.transform.eulerAngles = new Vector3(0, 300, 0);
            }
            else if (prevSide == Sides.TopRight && nextSide == Sides.TopLeft)
            {
                cornerRoad.transform.eulerAngles = new Vector3(0, 120, 0);
            }
            else if (prevSide == Sides.TopLeft && nextSide == Sides.Left)
            {
                cornerRoad.transform.eulerAngles = new Vector3(0, 60, 0);
            }


            cornerRoad.SetActive(true);
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
