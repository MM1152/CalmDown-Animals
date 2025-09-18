using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    //nextNeighborPos[0] = GetFloor(new Vector3((gridSize.x - gridSize.x* 0.5f) , 0, gridSize.y ));
    //    nextNeighborPos[1] = GetFloor(new Vector3(-(gridSize.x - gridSize.x* 0.5f), 0, gridSize.y));
    //    nextNeighborPos[2] = GetFloor(new Vector3(-gridSize.x, 0 ,0));
    //    nextNeighborPos[3] = GetFloor(new Vector3(-(gridSize.x - gridSize.x* 0.5f), 0, -gridSize.y));
    //    nextNeighborPos[4] = GetFloor(new Vector3((gridSize.x - gridSize.x* 0.5f), 0, -gridSize.y));
    //    nextNeighborPos[5] = GetFloor(new Vector3(gridSize.x, 0, 0));

    [Header("DEBUG")]
    public TileType tileType;
    public bool drawMode;
    public LayerMask layerMask;
    private int mapSize = -1;

    [Space(10)]
    [Header("DrawTile")]
    public PathTile prefabs;
    public DrawTile flagTilePrefabs;
    public int mapIdx;

    [Space(10)]
    [Header("PathFind")]
    public List<PathTile> startTile = new List<PathTile>();
    public PathTile arriveTile;

    [Space(10)]
    [Header("Reference")]
    public CrewManager crewSpawner;
    public EnemySpawner enemySpawner;
    public WindowManager windowManager;
    public GameManager gameManager;

    [Space(10)]
    [Header("Check")]
    public bool isChangedTile;

    private Dictionary<Vector3, PathTile> tileTable = new Dictionary<Vector3, PathTile>();
    private List<PathTile> tileList = new List<PathTile>();
    private LineRenderer lineRenderer;

    public static NeighborPosition neighborPosition;
    private PathFind pathFind = new PathFind();

    //실제 맵에 찍히는 타일들
    private DrawTile drawArriveTile;
    private List<DrawTile> drawStartTiles = new List<DrawTile>();
    private List<PathTile> editTiles = new List<PathTile>();


    private void Awake()
    {
        isChangedTile = false;

        Renderer sp = prefabs.GetComponent<MeshRenderer>();
        neighborPosition = new NeighborPosition(sp);

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    private void Start()
    {
        gameManager.endWave += DrawTiles;

        DrawTiles();
        FindPath(TileType.Path | TileType.None);
        SetInitPath();
    }

    private void Update()
    {
        SetTileType(tileType);
    }

    private void SetInitPath()
    {
        PathTile copyTile = startTile[0];
        while(copyTile != null)
        {
            copyTile.Type = TileType.Path;
            copyTile = copyTile.ParentTile;
        }

    }

    private void ResetInitPath()
    {
        if (isChangedTile) return;

        PathTile copyTile = startTile[0];
        while (copyTile != null)
        {
            if(copyTile != arriveTile && copyTile != startTile[0])
            {
                copyTile.Type = TileType.None;
            }
            copyTile = copyTile.ParentTile;
        }

        isChangedTile = true;
    }

    public bool FindPath(TileType type = TileType.Path)
    {
        bool isSuseccs = true;
        for(int i = 0; i < startTile.Count; i++)
        {
            if (!pathFind.Find(tileList, arriveTile, startTile[i], type))
            {
                isSuseccs = false;
            }
        }
     
        if(isSuseccs)
        {
            for(int i = 0; i < startTile.Count; i++)
            {
                var strTile = startTile[i];
                var drawStartTile = drawStartTiles[i];
                strTile.GetComponent<PathTileRoad>().PrevSide = PathTileRoad.FindSide(drawStartTile.InitPos, strTile.transform.position);
            }

            DrawRoads(startTile[0]);
        }
        return isSuseccs;
    }

    private void DrawRoads(PathTile startTile)
    {
        while(startTile != null)
        {
            if(startTile.ParentTile != null)
            {
                startTile.GetComponent<PathTileRoad>().DrawRoad(startTile.ParentTile.GetComponent<PathTileRoad>());
            }
            else
            {
                //Arrive 타일일 경우 Next 경로 강제로 설정
                startTile.GetComponent<PathTileRoad>().DrawRoad(startTile.ArriveDrawTile.InitPos);
            }
            startTile = startTile.ParentTile;
        }
    }

    public void SetTileType(TileType type)
    {
        if (!drawMode) return;

        ResetInitPath();
        
        // 한붓그리기 취소 됨
        //if(TouchManager.Phase == Phase.Up)
        //{
        //    bool isFail = FindPath();
        //    if(!isFail)
        //    {
        //        foreach(var editTile in editTiles)
        //        {
        //            editTile.Type = TileType.None;
        //        }
        //        editTiles.Clear();
        //    }
        //} 

        if (TouchManager.TouchType != TouchType.Drag && TouchManager.TouchType != TouchType.Tab) return;
        TileType targetTile = type == TileType.Path ? TileType.None : TileType.Path;
        var tile = GetTile();
        if(tile != null && tile.Type == targetTile && tile != arriveTile && !startTile.Contains(tile))
        {
            tile.Type = type;
            //editTiles.Add(tile);
        }
    }

    private PathTile GetTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            PathTile tile = hit.collider.GetComponent<PathTile>();
            if(tile != null)
            {
                return tile;
            }
            return null;
        }
        return null;
    }
    // Test 용 코드임

    public void DrawTiles()
    {
        int mapSize = DataTableManager.roundTable.Get(gameManager.Wave).Map_Size;
        if (mapSize == this.mapSize)
        {
            return;
        }
        this.mapSize = mapSize;
        var mapData = Map.Get(mapIdx);

        for(int i = 0; i < mapData.tiles[this.mapSize].Count; i++)
        {
            if (mapData.tiles[this.mapSize][i].DrawType == DrawType.Start || mapData.tiles[this.mapSize][i].DrawType == DrawType.Arrive)
            {
                var flagTile = Instantiate(flagTilePrefabs, transform);
                flagTile.UpdateDrawTile(mapData.tiles[this.mapSize][i]);
                if(flagTile.DrawType == DrawType.Arrive)
                {
                    drawArriveTile = flagTile;
                }
                else if (flagTile.DrawType == DrawType.Start)
                {
                    drawStartTiles.Add(flagTile);
                }
            }
            else
            {
                var pathTile = Instantiate(prefabs, transform);
                pathTile.UpdatePathTile(mapData.tiles[this.mapSize][i]);

                tileList.Add(pathTile);
                tileTable.Add(pathTile.transform.position, pathTile);
            }
        }

        for(int i = 0; i < drawStartTiles.Count; i++)
        {
            SetStartTile(drawStartTiles[i]);
        }

        SetArriveTile(drawArriveTile);
        FindNeighbor();
    }


    //public void DrawTiles()
    //{
    //    tileList.Clear();
    //    tileTable.Clear();

    //    for (int i = 0; i < height; i++)
    //    {
    //        for (int j = 0; j < width; j++)
    //        {
    //            PathTile tile = Instantiate(prefabs, transform);
    //            tile.gameObject.name = $"Tile_{i}_{j}";
    //            if (i % 2 != 0)
    //            {
    //                tile.transform.position = new Vector3(
    //                    NeighborPosition.GetFloor(neighborPosition.gridSize.x * j)
    //                    , 0
    //                    , NeighborPosition.GetFloor(neighborPosition.gridSize.y * i)
    //                    );
    //            }
    //            else
    //            {
    //                tile.transform.position = new Vector3(
    //                    NeighborPosition.GetFloor(neighborPosition.gridSize.x * j - neighborPosition.gridSize.x * 0.5f)
    //                    , 0
    //                    , NeighborPosition.GetFloor(neighborPosition.gridSize.y * i)

    //                    );
    //            }
    //            tileList.Add(tile);
    //            tileTable.Add(tile.transform.position, tile);
    //        }
    //    }
    //    FindNeighbor();
    //}

    private void FindNeighbor()
    {
        foreach(var tile in tileList)
        {
            for(int i = 0; i < neighborPosition.nextNeighborPos.Length; i++)
            {
                Vector3 nPos = NeighborPosition.GetFloor(tile.transform.position + neighborPosition.nextNeighborPos[i]);
                if(tileTable.ContainsKey(nPos))
                {
                    tile.Neighbor.Add(tileTable[nPos]);
                }
            }
        }
    }

    public void ClearRoad()
    {
        foreach(var tile in startTile)
        {
            var copyTile = tile;
            while(copyTile != null)
            {
                copyTile.GetComponent<PathTileRoad>().Clear();
                copyTile = copyTile.ParentTile;
            }
        }
    }

    // Test 용 코드임
    public PathTile[] GetEndTiles()
    {
        return startTile.ToArray();
    }

    // Test 용 코드임
    public void SetArriveTile(DrawTile drawTile)
    {
        arriveTile = tileTable[drawTile.ConnectPos];
        arriveTile.Type = TileType.Path;
        arriveTile.ArriveDrawTile = drawTile;
    }
    // Test 용 코드임
    public void SetStartTile(DrawTile drawTile)
    {
        //여기는 바닥에 찍힌 타일임
        var tile = tileTable[drawTile.ConnectPos];
        
        startTile.Add(tile);
        tile.Type = TileType.Path;

        Vector3 spawnPosition = drawTile.transform.position;
        Vector3 drawPosition = spawnPosition 
            + Vector3.Scale((spawnPosition - tile.transform.position).normalized ,new Vector3(neighborPosition.gridSize.y , 0 , neighborPosition.gridSize.x));

        enemySpawner.SettingSpawnInfoTile(startTile[startTile.Count - 1] , drawPosition, spawnPosition);
    }

}
