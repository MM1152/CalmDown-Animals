using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class TileManager : MonoBehaviour
{

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
    private NeighborPosition neighborPosition;

    private PathFind pathFind = new PathFind();
    private DrawTile drawArriveTile;
    private List<DrawTile> startTiles = new List<DrawTile>();
    private List<PathTile> editTiles = new List<PathTile>();

    private void Start()
    {
        isChangedTile = false;

        Renderer sp = prefabs.GetComponent<MeshRenderer>();
        neighborPosition = new NeighborPosition(sp);

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;

        gameManager.endWave += DrawTiles;

        DrawTiles();
        FindPath(TileType.Path | TileType.None);

        SetInitPath();
    }

    private void Update()
    {
        SetTileType();
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

        return isSuseccs;
    }

    public void SetTileType()
    {
        if (!drawMode) return;

        ResetInitPath();

        if(TouchManager.Phase == Phase.Up)
        {
            bool isFail = FindPath();
            if(!isFail)
            {
                foreach(var editTile in editTiles)
                {
                    editTile.Type = TileType.None;
                }

                editTiles.Clear();
            }
        } 

        if (TouchManager.TouchType != TouchType.Drag) return;

        var tile = GetTile();
        if(tile != null && tile.Type == TileType.None)
        {
            tile.Type = TileType.Path;
            editTiles.Add(tile);
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
        int mapSize = DataTableManager.Get<RoundTable>(DataTableIds.RoundTableIds).Get(gameManager.wave).Map_Size;
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
                    startTiles.Add(flagTile);
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

        for(int i = 0; i < startTiles.Count; i++)
        {
            SetStartTile(startTiles[i].ConnectPos);
        }

        SetArriveTile(drawArriveTile.ConnectPos);

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
    // Test 용 코드임
    public PathTile[] GetEndTiles()
    {
        return startTile.ToArray();
    }

    // Test 용 코드임
    public void SetArriveTile(Vector3 pos)
    {
        arriveTile = tileTable[pos];
        tileTable[pos].Type = TileType.Path;
    }
    // Test 용 코드임
    public void SetStartTile(Vector3 pos)
    {
        startTile.Add(tileTable[pos]);
        tileTable[pos].Type = TileType.Path;
        enemySpawner.SettingSpawnInfoTile(startTile[startTile.Count - 1] , neighborPosition.gridSize);
    }
}
