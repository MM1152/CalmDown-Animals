using JetBrains.Annotations;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class PathFind
{
    private float weight = 1.5f;
    private PriorityQueue<PathTile> openList = new PriorityQueue<PathTile>(Comparer<PathTile>.Create((x, y)=> x.F.CompareTo(y.F)));
    private List<PathTile> closeList = new List<PathTile>();
#if UNITY_EDITOR
    private Stopwatch stopwatch = new Stopwatch();
#endif
    public bool Find(List<PathTile> map , PathTile startTile , PathTile endTile , TileType checkTile = TileType.Path)
    {
        openList.Clear();
        closeList.Clear();

        startTile.G = 0;
        startTile.H = endTile - startTile;

        openList.EnQueue(startTile);
#if UNITY_EDITOR
        stopwatch.Start();
#endif
        while (!openList.Empty())
        {
            PathTile curTile = openList.Dequeue();

            if (curTile == endTile)
            {
#if UNITY_EDITOR
                stopwatch.Stop();
                UnityEngine.Debug.Log($"PathFind TIme : {stopwatch.ElapsedMilliseconds} ms");
#endif
                return true;
            }
            if (closeList.Contains(curTile)) continue;

            closeList.Add(curTile);
            
            foreach(var nTile in curTile.Neighbor)
            {
                if((nTile.Type & checkTile) > 0 && !closeList.Contains(nTile))
                {
                    int G = curTile.G + 10;
                    int H = Mathf.FloorToInt((endTile - nTile) * weight * 10);
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

}
