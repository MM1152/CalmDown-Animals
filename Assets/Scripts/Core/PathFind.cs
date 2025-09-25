using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class PathFind
{
    private PriorityQueue<PathTile> openList = new PriorityQueue<PathTile>(Comparer<PathTile>.Create((x, y)=> x.F.CompareTo(y.F)));
    private List<PathTile> closeList = new List<PathTile>();


    public bool Find(List<PathTile> map , PathTile startTile , PathTile endTile , TileType checkTile = TileType.Path)
    {
        openList.Clear();
        closeList.Clear();

        startTile.G = 0;
        startTile.H = endTile - startTile;

        openList.EnQueue(startTile);

        while(!openList.Empty())
        {
            PathTile curTile = openList.Dequeue();

            if (curTile == endTile)
            {
                return true;
            }
            if (closeList.Contains(curTile)) continue;

            closeList.Add(curTile);
            
            foreach(var nTile in curTile.Neighbor)
            {
                if((nTile.Type & checkTile) > 0 && !closeList.Contains(nTile))
                {
                    int G = curTile.G + 1;
                    int H = endTile - nTile;
                    int F = G + H;

                    if(openList.Contains(nTile) && nTile.F <= F) continue;

                    nTile.G = G;
                    nTile.H = H;

                    nTile.ParentTile = curTile;
                    curTile.IsSelectedPath = true;
                    openList.EnQueue(nTile);
                } 
            }
        }
        return false;
    }

    public void FindMinCost(List<PathTile> startTile , PathTile endTile)
    {
        TileType checkTile = TileType.None | TileType.Path | TileType.Blocked | TileType.Blocked;
        PriorityQueue<PathTile> openList = new PriorityQueue<PathTile>(Comparer<PathTile>.Create((x, y) => x.F.CompareTo(y.F)));
        List<PathTile> closeList = new List<PathTile>();


        for (int i = 0; i < startTile.Count; i++)
        {
            openList.EnQueue(startTile[i]);
            int min = int.MaxValue;

            startTile[i].G = 0;
            startTile[i].H = endTile - startTile[i];

            while (!openList.Empty())
            {
                PathTile curTile = openList.Dequeue();

                if (curTile.F > min) continue;

                if (curTile == endTile)
                {
                    min = curTile.F;
                    Debug.Log(min);
                }

                if (closeList.Contains(curTile)) continue;

                Debug.Log($"G : {curTile.G}, H : {curTile.H}", curTile);
                closeList.Add(curTile);
                curTile.testType = TestType.Path;

                foreach (var nTile in curTile.Neighbor)
                {
                    if ((nTile.Type & checkTile) > 0 && !closeList.Contains(nTile))
                    {
                        int G = curTile.G + 1;
                        int H = endTile - nTile;
                        int F = G + H;

                        if (openList.Contains(nTile) && nTile.F <= F) continue;
                        if (F > min) continue;
                        if (nTile.testType == TestType.Path)
                        {
                            min = F;
                            nTile.testCost += curTile.testCost;
                            continue;
                        }

                        nTile.G = G;
                        nTile.H = H;

                        nTile.testCost = curTile.testCost + 50;

                        openList.EnQueue(nTile);
                    }
                }
            }
            openList.Clear();
            closeList.Clear();
        }
    }
}
