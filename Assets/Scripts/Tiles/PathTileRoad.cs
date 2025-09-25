using System;
using UnityEngine;
public class PathTileRoad : MonoBehaviour
{
    public Mesh[] roadMeshs;
    public MeshFilter meshFillter;
    [Flags]
    public enum Sides
    {
        None = 0,
        TopRight = 1,
        TopLeft = 1 << 1,
        Left = 1 << 2,
        BottomLeft = 1 << 3,
        BottomRight = 1 << 4,
        Right = 1 << 5,
    }
    public PathTileRoad nextTile;

    public Sides prevSide;
    public Sides PrevSide
    {
        get => prevSide;
        set
        {
            prevSide = value;
        }
    }
    public Sides nextSide;
    public Sides NextSide
    {
        get => nextSide;
        set
        {
            nextSide = value;

            if (nextSide == Sides.TopRight)
                transform.eulerAngles = Vector3.up * -60f;
            if (nextSide == Sides.TopLeft)
                transform.eulerAngles = Vector3.up * -120f;
            if (nextSide == Sides.Left)
                transform.eulerAngles = Vector3.up * 180f;
            if (nextSide == Sides.BottomLeft)
                transform.eulerAngles = Vector3.up * 120f;
            if (nextSide == Sides.BottomRight)
                transform.eulerAngles = Vector3.up * 60f;
            if (nextSide == Sides.Right)
                transform.eulerAngles = Vector3.zero;
        }
            
    }
    //미리 오브젝트 돌려주기 용 프로퍼티
    private void Awake()
    {
        meshFillter = GetComponent<MeshFilter>();
    }

    public void DrawRoad(PathTileRoad nTile)
    {
        if (nTile == null) return;

        this.nextTile = nTile;

        NextSide = FindSide(transform.position, nTile.transform.position);
        SetNextTilePrevSide(nTile , NextSide);

        DrawRoads();
    }

    public void DrawRoad(Vector3 nPos)
    {
        NextSide = FindSide(transform.position, nPos);
        DrawRoads();
    }

    public void Clear()
    {
        prevSide = Sides.None;
        meshFillter.mesh = roadMeshs[0];
    }

    private void DrawRoads()
    {
        meshFillter.sharedMesh = roadMeshs[1];
        if (PrevSide == NextSide) return;

        int count = 0;
        for (int i = 0; i <= 5; i++)
        {
            if ((prevSide & (Sides)(1 << i)) > 0)
            {
                count++;
            }
        }

        if (count == 1)
        {
            
            //if((NextSide == Sides.TopRight && PrevSide == Sides.TopLeft)
            //    || (NextSide == Sides.Left && PrevSide == Sides.TopLeft)
            //    || (NextSide == Sides.Left && PrevSide == Sides.BottomLeft)
            //    || (NextSide == Sides.BottomLeft && PrevSide == Sides.BottomRight)
            //    || (NextSide == Sides.BottomRight && PrevSide == Sides.Right))
            //{
            //    meshFillter.sharedMesh = roadMeshs[7];
            //    return;
            //}

            if(PrevSide == Sides.Left && NextSide == Sides.BottomRight)
            {
                meshFillter.sharedMesh = roadMeshs[7];
                transform.eulerAngles += new Vector3(0f , -60f, 0f);
                return;
            }
            if (PrevSide == Sides.Left && NextSide == Sides.TopRight)
            {
                meshFillter.sharedMesh = roadMeshs[7];
                return;
            }

            if (!((NextSide == Sides.TopRight && prevSide == Sides.TopLeft) 
                || (NextSide == Sides.TopLeft && prevSide == Sides.Left)
                || (NextSide == Sides.Left && prevSide == Sides.BottomLeft)
                /*|| (nextSide == Sides.BottomRight && prevSide == Sides.BottomLeft)*/
                /*|| (NextSide == Sides.BottomLeft && prevSide == Sides.Right)*/
                || (NextSide == Sides.BottomLeft && prevSide == Sides.BottomRight)
                || (NextSide == Sides.Right && prevSide == Sides.TopRight)
                || (NextSide == Sides.BottomRight && prevSide == Sides.Right)))
            {
                transform.eulerAngles += new Vector3(0f, 240f, 0f);
            }
            meshFillter.sharedMesh = roadMeshs[2];
        }
        if(count == 2)
        {
            if((nextSide == Sides.BottomRight && prevSide == (Sides.Left | Sides.BottomRight)) ||
                nextSide == Sides.Left && prevSide == (Sides.Left | Sides.TopRight))
            {
                meshFillter.sharedMesh = roadMeshs[4];
                transform.eulerAngles += new Vector3(0f, 180f, 0f);
                return;
            }
            if(nextSide == Sides.TopRight && prevSide == (Sides.Left | Sides.TopRight))
            {
                meshFillter.sharedMesh = roadMeshs[5];
                transform.eulerAngles += new Vector3(0f, 180f, 0f);
                return;
            }
            if (nextSide == Sides.BottomRight && prevSide == (Sides.Right | Sides.Left))
            {
                meshFillter.sharedMesh = roadMeshs[5];
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
                return;
            }
            if (nextSide == Sides.TopRight && prevSide == (Sides.Left | Sides.Right))
            {
                meshFillter.sharedMesh = roadMeshs[5];
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
                return;
            }
            if (nextSide == Sides.TopRight && prevSide == (Sides.Left | Sides.TopRight))
            {
                meshFillter.sharedMesh = roadMeshs[5];
                transform.eulerAngles += new Vector3(0f, 180f, 0f);
                return;
            }
            if (nextSide == Sides.Right && prevSide == (Sides.Right | Sides.BottomLeft))
            {
                meshFillter.sharedMesh = roadMeshs[4];
                transform.eulerAngles += new Vector3(0f, 180f, 0f);
                return;
            }

            if (nextSide == Sides.TopLeft && prevSide == (Sides.Left | Sides.BottomLeft) ||
                (nextSide == Sides.BottomRight && prevSide == (Sides.Left | Sides.BottomLeft)))
            {
                meshFillter.sharedMesh = roadMeshs[8];
                transform.eulerAngles += new Vector3(0f, -60f, 0f);
                return;
            }
            if (nextSide == Sides.TopRight && prevSide == (Sides.Left | Sides.TopLeft))
            {
                meshFillter.sharedMesh = roadMeshs[8];
                transform.eulerAngles += new Vector3(0f, 60f, 0f);
                return;
            }
            if (nextSide == Sides.Right && prevSide == (Sides.BottomLeft | Sides.TopRight))
            {
                meshFillter.sharedMesh = roadMeshs[5];
                transform.eulerAngles += new Vector3(0f, 120f, 0f);
                return;
            }

            bool prevBitChecker = false;
            for(int i = 0; i < 5; i++)
            {
                bool curBitChecker = false;

                if (((int)prevSide & (1 << i)) > 0)
                {
                    curBitChecker = true;
                }  

                if(prevBitChecker && curBitChecker)
                {
                    int converseSide = (int)(prevSide & ~nextSide);

                    if(converseSide > (int)nextSide)
                    {
                        meshFillter.sharedMesh = roadMeshs[4];
                    }
                    else
                    {
                        meshFillter.sharedMesh = roadMeshs[5];
                    }
                    return;
                }

                prevBitChecker = curBitChecker;
            }
            meshFillter.sharedMesh = roadMeshs[3];
        }
        if(count == 3)
        {
            meshFillter.sharedMesh = roadMeshs[6];
            transform.eulerAngles += Vector3.up * 180f;
        }
        
    }
        

    public void SetNextTilePrevSide(PathTileRoad nTile , Sides nextSide)
    {
        nTile.PrevSide |= nextSide;
    }

    public static Sides FindSide(Vector3 prevTile, Vector3 nTile)
    {
        Vector3 side = nTile - prevTile;
        for(int i = 0; i < TileManager.neighborPosition.nextNeighborPos.Length; i++)
        {
            if(side == TileManager.neighborPosition.nextNeighborPos[i])
            {
                return (Sides)(1 << i);
            }
        }

        return Sides.None;
    }
}
