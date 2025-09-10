using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFind
{
    private PriorityQueue<PathTile> openList = new PriorityQueue<PathTile>(Comparer<PathTile>.Create((x, y)=> x.F.CompareTo(y.F)));
    private List<PathTile> closeList = new List<PathTile>();


    public bool Find(List<PathTile> map , PathTile startTile , PathTile endTile)
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
                if(nTile.Type == TileType.Path && !closeList.Contains(nTile))
                {
                    int G = curTile.G + 1;
                    int H = endTile - nTile;
                    int F = G + H;

                    if(openList.Contains(nTile) && nTile.F <= F) continue;

                    nTile.G = G;
                    nTile.H = H;

                    nTile.ParentTile = curTile;
                    openList.EnQueue(nTile);
                } 
            }
        }

        return false;
    }
}
