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
    private Vector2 gridSize;
    private Dictionary<Vector3, PathTile> tileTable = new Dictionary<Vector3, PathTile>();
    private List<PathTile> tileList = new List<PathTile>();
    private Vector3[] neighborPositions = new Vector3[6];
    private LineRenderer lineRenderer;

    //Test 용 코드임
    public bool isSelectStart;
    public bool isSelectEnd;
    public bool isSelectbutton;

    private PathFind pathFind = new PathFind();

    private void Start()
    {
        Renderer sp = prefabs.GetComponent<MeshRenderer>();
        gridSize = new Vector2(Mathf.Ceil(sp.bounds.size.x * 10f) / 10f, Mathf.Ceil(sp.bounds.size.z * 10f) / 10f);

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;

        neighborPositions[0] = GetFloor(new Vector3(gridSize.y, 0 , -(gridSize.x - gridSize.x * 0.5f)));
        neighborPositions[1] = GetFloor(new Vector3(gridSize.y, 0 , (gridSize.x - gridSize.x * 0.5f)));
        neighborPositions[2] = GetFloor(new Vector3(-gridSize.y, 0 , (gridSize.x - gridSize.x * 0.5f)));
        neighborPositions[3] = GetFloor(new Vector3(-gridSize.y, 0 , -(gridSize.x - gridSize.x * 0.5f)));
        neighborPositions[4] = GetFloor(new Vector3(0, 0 , gridSize.x));
        neighborPositions[5] = GetFloor(new Vector3(0, 0 , -gridSize.x));

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
                    tile.transform.position = new Vector3(GetFloor(i * gridSize.y) , 0f , GetFloor(j * gridSize.x));
                }
                else
                {
                    tile.transform.position = new Vector3(GetFloor(i * gridSize.y), 0f , GetFloor((j * gridSize.x) - (gridSize.x * 0.5f)));
                }
                tileList.Add(tile);
                tileTable.Add(tile.transform.position , tile);
            }
        }

        FindNeighbor();
    }
    private float GetFloor(float value)
    {
        return Mathf.Round(value * 10f) / 10f;
    }
    private Vector3 GetFloor(Vector3 value)
    {
        return new Vector3 (
            Mathf.Round(value.x * 10f) / 10f,
            Mathf.Round(value.y * 10f) / 10f,
            Mathf.Round(value.z * 10f) / 10f
        );
    }
    private void FindNeighbor()
    {
        foreach(var tile in tileList)
        {
            for(int i = 0; i < neighborPositions.Length; i++)
            {
                Vector3 nPos = GetFloor(tile.transform.position + neighborPositions[i]);
                Debug.Log(nPos);
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
