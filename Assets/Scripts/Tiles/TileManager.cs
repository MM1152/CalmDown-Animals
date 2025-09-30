using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    public bool deleteMode;
    public int tilePrice;
    public bool InEditorWindow { get; set; }
    public LayerMask layerMask;
    public LayerMask infoMask;
    public int allAnimalSpawnCount;
    public int mapSize = -1;
    public float changeToPathTileInBlock;
    public float changeToCrewTileInBlock;
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
    public PopupManager popupManager;
    public GameManager gameManager;

    [Space(10)]
    [Header("Check")]
    public bool isChangedTile;

    private Dictionary<Vector3, PathTile> tileTable = new Dictionary<Vector3, PathTile>();
    public List<PathTile> tileList = new List<PathTile>();
    private LineRenderer lineRenderer;

    public static NeighborPosition neighborPosition;
    private PathFind pathFind = new PathFind();
    
    //실제 맵에 찍히는 타일들
    private DrawTile drawArriveTile = null;
    private List<DrawTile> drawStartTiles = new List<DrawTile>();
    private List<PathTile> editTiles = new List<PathTile>();

    public Action OnClickInfoTileInTutorial;

    private int[] shortPathCost = new int[]
    {
        0,
        200,
        350,
        550
    };

    //x : left , y : top , z : width , w : height 
    public Vector4 DragAbleRect => dragAblePos;
    private Vector4 dragAblePos = new Vector4(float.MaxValue, float.MaxValue, 0, 0);
    private void Awake()
    {
        isChangedTile = false;

        Renderer sp = prefabs.GetComponent<MeshRenderer>();
        neighborPosition = new NeighborPosition(sp);

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;

        mapIdx = UnityEngine.Random.Range(0, Map.Count());
    }

    private void Start()
    {
        gameManager.endWave += DrawTiles;
        gameManager.endWave += () => ChangeToBlockedTile(gameManager.AllAnimalSpawnCount);
    }

    public void DataLoadFail()
    {
        DrawTiles();
        FindPathAndDrawRoads(TileType.Path | TileType.None);
        SetInitPath();
    }

    private void Update()
    {
        SetTileType(tileType);
        ShowAnimalInfomation();
        DeleteBlockedTile();
    }

    public void SetDragFailTileInTutorial()
    {
        //2.2 , 3.8
        //4.4 , 0
        ///6.6 , 0
        tileTable[new Vector3(2.2f, 0f, 3.8f)].Type = TileType.Path;
        tileTable[new Vector3(4.4f, 0f, 0f)].Type = TileType.Path;
        tileTable[new Vector3(6.6f, 0f, 3.8f)].Type = TileType.Path;

        ChangeToColorPathTiles();
    }
    public void ClearAllTileInTutorial()
    {
        foreach(var tile in tileList)
        {
            if(tile != arriveTile && tile != startTile[0])
            {
                tile.Type = TileType.None;
            }
        }
    }
    public void DrawTwoRoadTilesInTutorial()
    {
        ClearAllTileInTutorial();
        tileTable[new Vector3(2.2f, 0f, 0f)].Type = TileType.Path;
        tileTable[new Vector3(4.4f, 0f, 0f)].Type = TileType.Path;
        tileTable[new Vector3(6.6f, 0f, 0f)].Type = TileType.Path;

        tileTable[new Vector3(1.1f, 0f, 1.9f)].Type = TileType.Path;
        tileTable[new Vector3(2.2f, 0f, 3.8f)].Type = TileType.Path;
        tileTable[new Vector3(4.4f, 0f, 3.8f)].Type = TileType.Path;
        tileTable[new Vector3(6.6f, 0f, 3.8f)].Type = TileType.Path;
        tileTable[new Vector3(7.7f, 0f, 1.9f)].Type = TileType.Path;

        ChangeToColorPathTiles();
        //2.2 , 0 , 0
        // 4.4 , 0 , 0
        // 6.6 , 0 , 0

        //1.1 ,  0,  1.9
        //2.2 , 0 , 3.8
        //4.4 , 0 , 3.8
        //6.6, 0 ,3.8
        //7.7, 0 , 1.9
    }

    public PathTile GetTileInTutorial()
    {
        return tileTable[new Vector3(7.7f, 0f, 1.9f)];
    }

    public void ChangeVariableOnTree()
    {
        foreach(var tile in tileList)
        {
            tile.Type = tile.Type;
        }
    }

    public bool CheckClearAllTileInTutorial()
    {
        for(int i = 0; i < tileList.Count; i++)
        {
            if (tileList[i].Type == TileType.Path && tileList[i] != arriveTile && tileList[i] != startTile[0])
            {
                return false;
            }
        }

        return true;
    }

    private void ShowAnimalInfomation()
    {
        if (!Status.ShowAnimalInfo) return;
        if (TouchManager.touchType == TouchType.Tab && !TouchManager.TouchStartInUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(TouchManager.GetDragPos());
            if(Physics.Raycast(ray, out RaycastHit hit , Mathf.Infinity , infoMask))
            {
                var collider = hit.collider.GetComponent<SpawnEnemyInfo>();

                if(collider != null)
                {
                    var popup = popupManager.Open(Popup.AnimalInfoPopup) as AnimalInfoPopup;
                    popup.AnimalInfomation = collider.GetAnimalData();
                    popup.transform.position = Camera.main.WorldToScreenPoint(collider.transform.position);
                    OnClickInfoTileInTutorial?.Invoke();
                }
            }
        }
    }

    private void DeleteBlockedTile()
    {
        if(deleteMode)
        {
            if(!TouchManager.TouchStartInUI() && (TouchManager.touchType == TouchType.Drag
                || TouchManager.touchType == TouchType.Tab))
            {
                var tile = GetTile();
                if(tile.Type == TileType.Blocked)
                {
                    if(gameManager.Gold < 250)
                    {
                        var popup = popupManager.Open(Popup.TextPopUp) as StringPopUp;
                        popup.Id = 3;
                        return;
                    }
                    tile.DeleteBlockedTile();
                }
            }
        }
    }

    private void SetInitPath()
    {
        PathTile copyTile = startTile[0];
        for(int i = 0; i < copyTile.Neighbor.Count; i++)
        {
            if(copyTile.Neighbor[i].Type != TileType.Path)
            {
                crewSpawner.SetStartUnit(CrewRank.Intern, copyTile.Neighbor[i]);
                break;
            }
        }
        while(copyTile != null)
        {
            copyTile.Type = TileType.Path;
            copyTile = copyTile.ParentTile;
        }

        //처음 크루 설치
        // 공짜로 설치 하고, 타일 위치는 -0.6 , 1.1
       
    }

    public void ResetInitPath()
    {
        if (isChangedTile) return;

        PathTile copyTile = startTile[0];
        while (copyTile != null)
        {
            if(copyTile != arriveTile && copyTile != startTile[0])
            {
                copyTile.Type = TileType.None;
                gameManager.Gold += tilePrice;
            }
            copyTile = copyTile.ParentTile;
        }

        isChangedTile = true;
    }

    public bool FindPath(TileType type = TileType.Path)
    {
        for (int i = 0; i < startTile.Count; i++)
        {
            if (pathFind.Find(tileList, arriveTile, startTile[i], type))
            {
                return true;
            }
        }
        return false;
    }

    public bool FindPathAndDrawRoads(TileType type = TileType.Path)
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
            for (int i = 0; i < startTile.Count; i++)
            {
                var strTile = startTile[i];
                var drawStartTile = drawStartTiles[i];
                strTile.GetComponent<PathTileRoad>().PrevSide |= PathTileRoad.FindSide(drawStartTile.InitPos, strTile.transform.position);
                DrawRoads(startTile[i]);
            }

            SaveLoadManager.Data.pathTile = editTiles.Select(x => x.transform.position).ToList();

            foreach (var tile in tileList)
            {
                if (tile.GetComponent<PathTileRoad>().PrevSide == PathTileRoad.Sides.None && tile.Type == TileType.Path)
                {
                    tile.Type = TileType.None;
                    gameManager.Gold += tilePrice;
                }
            }

            editTiles.Clear();
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
            if(!editTiles.Contains(startTile))
            {
                editTiles.Add(startTile);
            }
            startTile = startTile.ParentTile;
        }

        SoundManager.Instance.PlayOneShot(SFX.DrawTileSound);
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

        if (TouchManager.touchType != TouchType.Drag && TouchManager.touchType != TouchType.Tab) return;
        TileType targetTile = type == TileType.Path ? TileType.None : TileType.Path;
        var tile = GetTile();
        if(tile != null && tile.Type == targetTile && tile != arriveTile && !startTile.Contains(tile))
        {
            if (type == TileType.Path && gameManager.Gold < tilePrice)
            {
                var popup = popupManager.Open(Popup.TextPopUp) as StringPopUp;
                popup.Id = 3;
                return;
            }


            tile.Type = type;
            if (tile.Type == TileType.Path)
            {
                tile.ChangeColor();
                gameManager.Gold -= tilePrice;
            }
            if(tile.Type == TileType.None)
            {
                gameManager.Gold += tilePrice;
            }
        }
    }

    public void ChangeToColorPathTiles()
    {
        for (int i = 0; i < tileList.Count; i++)
        {
            if (tileList[i].Type == TileType.Path)
            {
                tileList[i].ChangeColor();
            }
        }
    }

    public void ResetToColorPathTiles()
    {
        for (int i = 0; i < tileList.Count; i++)
        {
            if (tileList[i].Type == TileType.Path)
            {
                tileList[i].ResetColor();
            }
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

    public void DrawTiles(int mapSize)
    {
        this.mapSize = mapSize;
        var mapData = Map.Get(0);
        for (int i = 0; i <= mapSize; i++)
        {
            for (int j = 0; j < mapData.tiles[i].Count; j++)
            {
                if ((i == mapSize && mapData.tiles[i][j].DrawType == DrawType.Start) || mapData.tiles[i][j].DrawType == DrawType.Arrive)
                {
                    var flagTile = Instantiate(flagTilePrefabs, transform);
                    flagTile.UpdateDrawTile(mapData.tiles[i][j]);
                    if (drawArriveTile == null && flagTile.DrawType == DrawType.Arrive)
                    {
                        drawArriveTile = flagTile;
                    }
                    else if (flagTile.DrawType == DrawType.Start)
                    {
                        drawStartTiles.Add(flagTile);
                    }
                }
                else if (mapData.tiles[i][j].DrawType == DrawType.None)
                {
                    var pathTile = Instantiate(prefabs, transform);
                    pathTile.UpdatePathTile(mapData.tiles[i][j], this);

                    tileList.Add(pathTile);
                    tileTable.Add(pathTile.transform.position, pathTile);

                    FindRect(new Vector2(pathTile.transform.position.x, pathTile.transform.position.z));
                }
                FindNeighbor();
            }
            
        }

        crewSpawner.unitInfomation = SaveLoadManager.Data.employCrewCount;
        
        foreach(var rank in SaveLoadManager.Data.crewSpawn.Keys)
        {
            foreach(var pos in SaveLoadManager.Data.crewSpawn[rank])
            {
                crewSpawner.Spawn(rank);
                crewSpawner.CrewForcingSpawn(rank , tileTable[pos]);
            }
        }

        if (arriveTile == null)
        {
            SetArriveTile(drawArriveTile);
        }
        for (int i = 0; i < drawStartTiles.Count; i++)
        {
            SetStartTile(drawStartTiles[i]);
        }

        foreach(var pos in SaveLoadManager.Data.pathTile)
        {
            tileTable[pos].Type = TileType.Path;
        }

        FindPathAndDrawRoads();
    }

    public void DrawTiles()
    {
        int mapSize = DataTableManager.roundTable.Get(gameManager.Wave).Map_Size;
        if (mapSize == this.mapSize)
        {
            return;
        }

        if(shortPathCost[mapSize] > gameManager.Gold)
        {
            int sellingCrew = crewSpawner.GetHireCount(CrewRank.Intern) * DataTableManager.crewTable.Get(CrewRank.Intern).crewCost;
            sellingCrew += crewSpawner.GetHireCount(CrewRank.Newbie) * DataTableManager.crewTable.Get(CrewRank.Newbie).crewCost;
            sellingCrew += crewSpawner.GetHireCount(CrewRank.Senior) * DataTableManager.crewTable.Get(CrewRank.Senior).crewCost;
            sellingCrew += crewSpawner.GetHireCount(CrewRank.Ace) * DataTableManager.crewTable.Get(CrewRank.Ace).crewCost;

            if(gameManager.Gold + sellingCrew < shortPathCost[mapSize])
            {
                gameManager.EndWave(true);
                return;
            }
        }

        for (int i = 0; i < drawStartTiles.Count; i++)
        {
            Destroy(drawStartTiles[i].gameObject);
        }
        drawStartTiles.Clear();

        ClearRoad();

        for (int i = 0; i < startTile.Count; i++)
        {
            startTile[i].Type = TileType.None;
            enemySpawner.RemoveInfoTile(startTile[i].EnemyInfo);
        }
        startTile.Clear();

        this.mapSize = mapSize;
        var mapData = Map.Get(mapIdx);

        for(int i = 0; i < mapData.tiles[this.mapSize].Count; i++)
        {
            if (mapData.tiles[this.mapSize][i].DrawType == DrawType.Start || mapData.tiles[this.mapSize][i].DrawType == DrawType.Arrive)
            {
                var flagTile = Instantiate(flagTilePrefabs, transform);
                flagTile.UpdateDrawTile(mapData.tiles[this.mapSize][i]);
                if(drawArriveTile == null && flagTile.DrawType == DrawType.Arrive)
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
                pathTile.UpdatePathTile(mapData.tiles[this.mapSize][i] , this);

                tileList.Add(pathTile);
                tileTable.Add(pathTile.transform.position, pathTile);

                FindRect(new Vector2(pathTile.transform.position.x, pathTile.transform.position.z));
            }
        }

        for (int i = 0; i < drawStartTiles.Count; i++)
        {
            SetStartTile(drawStartTiles[i]);
        }

        if(arriveTile == null)
        {
            SetArriveTile(drawArriveTile);
        }
        FindNeighbor();
        //ClearAllTestTile();
        //pathFind.FindMinCost(startTile , arriveTile);
    }

    public void ClearAllTiles()
    {
        isChangedTile = true;
        foreach (var tile in tileList)
        {
            if(!startTile.Contains(tile) && arriveTile != tile && tile.Type == TileType.Path)
            {
                tile.Type = TileType.None;
                gameManager.Gold += tilePrice;
            }
        }
    }

    private void FindRect(Vector2 a)
    {
        if(dragAblePos.x > a.x)
        {
            dragAblePos.x = a.x;
        }
        if(dragAblePos.z < a.x)
        {
            dragAblePos.z = a.x;
        }
        if(dragAblePos.y > a.y)
        {
            dragAblePos.y = a.y;
        }
        if(dragAblePos.w < a.y)
        {
            dragAblePos.w = a.y;
        }
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
                if (tileTable.ContainsKey(nPos) && !tile.Neighbor.Contains(tileTable[nPos]))
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
        SoundManager.Instance.PlayOneShot(SFX.DestroySound);
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

        var spawner = enemySpawner.SettingSpawnInfoTile(startTile[startTile.Count - 1] , drawPosition, spawnPosition);
        tile.EnemyInfo = spawner;
    }

    public void ChangeToBlockedTile(int allEnemySpawnCount)
    {
        if (!DataTableManager.roundTable.Get(gameManager.Wave).isUnavail) return;

        float percent = UnityEngine.Random.Range(0f, 1f);

        foreach (var tile in tileList)
        {
            var intileAnimal = tile.GetComponent<InTileAnimal>();
            float changePercent = intileAnimal.killStack / (float)allEnemySpawnCount * changeToPathTileInBlock;
            tile.changeToBlockedPercent.text = Mathf.RoundToInt(changePercent * 100f) + "%";

            if(percent <= changePercent)
            {
                tile.UpdateBlockedTile();
                intileAnimal.killStack = 0;
                continue;
            }

            //changePercent = tile.CrewKillCount / (float)allEnemySpawnCount * changeToCrewTileInBlock;

            //if(percent <= changePercent)
            //{
            //    tile.UpdateBlockedTile();
            //    tile.CrewKillCount = 0;
            //    continue;
            //}
        }
    }
}
