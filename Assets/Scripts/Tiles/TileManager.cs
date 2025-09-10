using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public PathTile[] endTile;

    [SerializeField]
    private Vector2 gridSize;
    private Dictionary<Vector3, PathTile> tileTable = new Dictionary<Vector3, PathTile>();
    private List<PathTile> tileList = new List<PathTile>();
    private Vector3[] neighborPositions = new Vector3[6];
    private LineRenderer lineRenderer;

    private PathFind pathFind = new PathFind();

    private void Start()
    {
        SpriteRenderer sp = prefabs.GetComponent<SpriteRenderer>();
        gridSize = new Vector2(sp.bounds.size.x, sp.bounds.size.z);

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;

        neighborPositions[0] = new Vector3(gridSize.y, 0 , -(gridSize.x - gridSize.x * 0.5f) );
        neighborPositions[1] = new Vector3(gridSize.y, 0 , (gridSize.x - gridSize.x * 0.5f) );
        neighborPositions[2] = new Vector3(-gridSize.y, 0 , (gridSize.x - gridSize.x * 0.5f));
        neighborPositions[3] = new Vector3(-gridSize.y, 0 , -(gridSize.x - gridSize.x * 0.5f));
        neighborPositions[4] = new Vector3(0, 0 , gridSize.x);
        neighborPositions[5] = new Vector3(0, 0 , -gridSize.x);

        DrawTiles();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FindPath();
        }
        SetTileType();
    }

    public void FindPath()
    {
        for(int i = 0; i< endTile.Length; i++)
        {
            if (pathFind.Find(tileList, startTile, endTile[i]))
            {
                
            }
            else
            {
                Debug.Log("Find Fail");
            }
        }
        var copyTile = endTile[0];
        int idx = 0;
        while (copyTile != null)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(idx++ , copyTile.transform.position + Vector3.up);
            copyTile = copyTile.ParentTile;
        }
    }
    

    public void SetTileType()
    {
        if (!drawMode) return;
        if (TouchManager.TouchType != TouchType.Draw) return;

        var tile = GetTile();
        if(tile != null)
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
                    tile.transform.position = new Vector3(i * gridSize.y , 0f ,j * gridSize.x);
                }
                else
                {
                    tile.transform.position = new Vector3(i * gridSize.y, 0f ,j * gridSize.x - gridSize.x * 0.5f);
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
            for(int i = 0; i < neighborPositions.Length; i++)
            {
                Vector3 nPos = tile.transform.position + neighborPositions[i];
                if(tileTable.ContainsKey(nPos))
                {
                    tile.Neighbor.Add(tileTable[nPos]);
                }
            }
        }
    }
}
