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

    [Space(10)]
    [Header("DrawTile")]
    public PathTile prefabs;
    public int width;
    public int height;

    [Space(10)]
    [Header("PathFind")]
    public PathTile startTile;
    public List<PathTile> endTile;


    [Space(10)]
    [Header("Reference")]
    public CrewManager crewSpawner;
    public WindowManager windowManager;

    [Space(10)]
    [Header("Check")]
    public bool isChangedTile;

    private Dictionary<Vector3, PathTile> tileTable = new Dictionary<Vector3, PathTile>();
    private List<PathTile> tileList = new List<PathTile>();
    private LineRenderer lineRenderer;
    private NeighborPosition neighborPosition;

    private PathFind pathFind = new PathFind();

    private void Start()
    {
        isChangedTile = false;

        Renderer sp = prefabs.GetComponent<MeshRenderer>();
        neighborPosition = new NeighborPosition(sp);

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;

        DrawTiles();
        SetStartTile(new Vector3(-0.6f, 0f, 2.2f));
        SetEndTile(new Vector3(4.2f, 0f, 2.2f));

        FindPath(TileType.Path | TileType.None);

        SetInitPath();
    }

    private void Update()
    {
        SetTileType();
    }

    private void SetInitPath()
    {
        PathTile copyTile = endTile[0];
        while(copyTile != null)
        {
            copyTile.Type = TileType.Path;
            copyTile = copyTile.ParentTile;
        }
    }

    private void ResetInitPath()
    {
        if (isChangedTile) return;

        PathTile copyTile = endTile[0];
        while (copyTile != null)
        {
            if(copyTile != startTile && copyTile != endTile[0])
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
        for(int i = 0; i < endTile.Count; i++)
        {
            if (!pathFind.Find(tileList, startTile, endTile[i], type))
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

        if (TouchManager.TouchType != TouchType.Drag) return;

        var tile = GetTile();
        if(tile != null && tile.Type == TileType.None)
        {
            tile.Type = TileType.Path;
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
        tileList.Clear();
        tileTable.Clear();

        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                PathTile tile = Instantiate(prefabs, transform);
                tile.gameObject.name = $"Tile_{i}_{j}";
                if (i % 2 != 0)
                {
                    tile.transform.position = new Vector3(
                        NeighborPosition.GetFloor(neighborPosition.gridSize.x * j)
                        , 0
                        , NeighborPosition.GetFloor(neighborPosition.gridSize.y * i)
                        
                        );
                }
                else
                {
                    tile.transform.position = new Vector3(
                        NeighborPosition.GetFloor(neighborPosition.gridSize.x * j - neighborPosition.gridSize.x * 0.5f)
                        , 0
                        ,NeighborPosition.GetFloor(neighborPosition.gridSize.y * i)
                         
                        );
                }
                tileList.Add(tile);
                tileTable.Add(tile.transform.position , tile);
            }
        }

        FindNeighbor();
    }

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
        return endTile.ToArray();
    }

    // Test 용 코드임
    public void SetStartTile(Vector3 pos)
    {
        startTile = tileTable[pos];
        tileTable[pos].Type = TileType.Path;
    }
    // Test 용 코드임
    public void SetEndTile(Vector3 pos)
    {
        endTile.Add(tileTable[pos]);
        tileTable[pos].Type = TileType.Path;
    }
}
