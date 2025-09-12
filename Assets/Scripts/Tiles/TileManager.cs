using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
public class TileManager : MonoBehaviour
{
    [Header("DEBUG")]
    public TileType tileType;
    public bool drawMode = false;
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
    public CrewSpawner crewSpawner;

    [SerializeField]
    private Dictionary<Vector3, PathTile> tileTable = new Dictionary<Vector3, PathTile>();
    private List<PathTile> tileList = new List<PathTile>();
    private LineRenderer lineRenderer;
    private NeighborPosition neighborPosition;


    //Test 용 코드임
    public bool isSelectStart;
    public bool isSelectEnd;
    public bool isSelectbutton;

    private PathFind pathFind = new PathFind();

    private void Start()
    {
        Renderer sp = prefabs.GetComponent<MeshRenderer>();
        neighborPosition = new NeighborPosition(sp);

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;

        DrawTiles();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FindPath();
        }
        SetTileType();

        if (isSelectStart)
        {
            if (TouchManager.TouchType == TouchType.Tab)
            {
                if (isSelectbutton)
                {
                    isSelectbutton = false;
                    return;
                }

                PathTile tile = GetTile();

                if (tile != null)
                {
                    startTile = tile;
                    tile.Type = TileType.Path;
                }
                isSelectStart = false;
            }
        }

        if (isSelectEnd)
        {
            if (TouchManager.TouchType == TouchType.Tab)
            {
                if (isSelectbutton)
                {
                    isSelectbutton = false;
                    return;
                }

                PathTile tile = GetTile();

                if (tile != null)
                {
                    if (endTile.Contains(tile)) return;
                    endTile.Add(tile);
                    tile.Type = TileType.Path;
                }

                isSelectEnd = false;
            }
        }
    }

    public void FindPath()
    {
        for(int i = 0; i< endTile.Count; i++)
        {
            if (pathFind.Find(tileList, startTile, endTile[i]))
            {
                
            }
            else
            {
                Debug.Log("Find Fail");
            }
        }
        if(endTile.Count > 0)
        {
            var copyTile = endTile[0];
            int idx = 0;
            while (copyTile != null)
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(idx++, copyTile.transform.position + Vector3.up);
                copyTile = copyTile.ParentTile;
            }
        }

    }
    public void SetTileType()
    {
        if (!drawMode) return;
        if (TouchManager.TouchType != TouchType.Drag) return;
        if (crewSpawner.IsSpawn) return;

        var tile = GetTile();
        if(tile != null && tile.Type == TileType.None)
        {
            tile.Type = tileType;
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
    public void SetStartTile()
    {
        isSelectStart = true;
        isSelectbutton = true;
    }
    // Test 용 코드임
    public void SetEndTile()
    {
        isSelectEnd = true;
        isSelectbutton = true;
    }
}
