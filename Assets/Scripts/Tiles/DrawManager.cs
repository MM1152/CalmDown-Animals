using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
public class DrawManager : MonoBehaviour
{
    public enum DrawMode
    {
        Tile,
        Arrive,
        Start,
    }

    public GameObject prefabs;
    public DrawTile startTilePrefabs;
    public LayerMask mask;
    public TextMeshProUGUI modeText;

    private List<DrawTile> tiles = new List<DrawTile>();
    private Dictionary<Vector3, DrawTile> tileTable = new Dictionary<Vector3, DrawTile>();

    private NeighborPosition neighborPosition;
    private Stack<DrawTile> drawTileUndoStack = new Stack<DrawTile>();

    private DrawTile startTile;
    private Stack<DrawTile> startTileUndoStack = new Stack<DrawTile>();

    private DrawMode mode = DrawMode.Tile;
    public DrawMode Mode
    {
        get => mode;
        set
        {
            if (mode == value) return;
            mode = value;

            if(startTile != null)
            {
                Destroy(startTile.gameObject);
                startTile = null;
            }

            switch (mode)
            {
                case DrawMode.Tile:
                    modeText.text = "Ÿ�� �׸���";
                    break;
                case DrawMode.Arrive:
                    modeText.text = "����Ÿ�� ����";
                    break;
                case DrawMode.Start:
                    modeText.text = "����Ÿ�� ����";
                    break;
            }
        }
    }
    private void Awake()
    {
        Renderer sp = prefabs.GetComponent<Renderer>();
        neighborPosition = new NeighborPosition(sp);

        var tile = Instantiate(prefabs, transform);
        tile.transform.position = Vector3.zero;

        var find = tile.GetComponent<DrawTile>();
        if(find != null)
        {
            find.Draw();
            tiles.Add(find);
            tileTable.Add(find.transform.position , find);
            SetAroundTile(find);
        }
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.X))
        {
            if (mode == DrawMode.Tile && drawTileUndoStack.Count > 0)
            {
                var undoTile = drawTileUndoStack.Pop();
                DeleteAroundTile(undoTile);
            }
            if(mode == DrawMode.Start && startTileUndoStack.Count > 0)
            {
                var undoStartTile = startTileUndoStack.Pop();
                UndoStartTile(undoStartTile);
            }
        }

        

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D))
        {
            Mode = DrawMode.Tile;
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.A))
        {
            Mode = DrawMode.Start;
        }

        switch (mode)
        {
            case DrawMode.Tile:
                UpdateDrawTile();
                break;
            case DrawMode.Arrive:
                
                break;
            case DrawMode.Start:
                UpdateDrawStartTile();
                break;
        }
    }

    private void UpdateDrawStartTile()
    {
        if (startTile == null)
        {
            startTile = Instantiate(startTilePrefabs, transform);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask))
        {
            DrawTile tile = hit.collider.GetComponent<DrawTile>();
            if(tile != null && tile.drawType == DrawType.None && tile.IsDraw)
            {
                Vector3 mousePos = Input.mousePosition;
                Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));

                int sector = FindHexaSector(hit.transform.position, mousePosInWorld);
                Debug.Log(sector);

                if (tile.AroundTile[sector].IsDraw || !tile.AroundTile[sector].gameObject.activeSelf) return;

                startTile.transform.position = tile.AroundTile[sector].transform.position;
                startTile.transform.rotation = Quaternion.Euler(-90f, (sector + 1) * -60f, 0f);

                if(Input.GetMouseButtonDown(0))
                {
                    var find = tile.AroundTile[sector];
                    find.gameObject.SetActive(false);

                    startTile.UnderTile = find;
                    tile.ConnectStartTiles.Add(startTile);
                    startTileUndoStack.Push(startTile);
                    startTile = null;
                }
            }
        }
    }

    private void UndoStartTile(DrawTile undoStartTile)
    {
        undoStartTile.UnderTile.gameObject.SetActive(true);
        Destroy(undoStartTile.gameObject);
    }

    private void UpdateDrawTile()
    {
#if UNITY_EDITOR             
        if (!Input.GetMouseButtonDown(0)) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

#elif UNITY_ANDROID || UNITY_IOS
        if (TouchManager.TouchType != TouchType.Tab) return;
        Ray ray = Camera.main.ScreenPointToRay(TouchManager.GetDragPos());
#endif

        if (FindTile(ray , out DrawTile find)) 
        {
            find.Draw();
            drawTileUndoStack.Push(find);
            SetAroundTile(find);
        }
    }
    private bool FindTile(Ray ray , out DrawTile drawTile)
    {
        drawTile = null;

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask))
        {
            var find = hit.collider.GetComponent<DrawTile>();
            if (find != null && !find.IsDraw && find.drawType == DrawType.None)
            {
                drawTile = find;
                return true;
            }
        }

        return false;
    }
    private void DeleteAroundTile(DrawTile tile)
    {
        if (drawTileUndoStack.Count <= 0) return;

        //����� ����Ÿ�� ����
        if (tile.ConnectStartTiles.Count > 0)
        {
            foreach (var connectTile in tile.ConnectStartTiles)
            {
                connectTile.UnderTile.gameObject.SetActive(true);
                connectTile.gameObject.SetActive(false);
                Destroy(connectTile);
            }

            var findTile = startTileUndoStack.Where(x => x.gameObject.activeSelf);
            startTileUndoStack = new Stack<DrawTile>(findTile);

            tile.ConnectStartTiles.Clear();
        }

        //�ֺ� ����� Ÿ�� ����
        for (int i = 0; i < tile.AroundTile.Count; i++)
        {
            tile.AroundTile[i].connectCount--;
            if (tile.AroundTile[i].connectCount == 0)
            {
                
                tileTable.Remove(tile.AroundTile[i].transform.position);
                Destroy(tile.AroundTile[i].gameObject);
            }
        }
        tile.Undo();
    }

    private void SetAroundTile(DrawTile tile)
    {
        for(int i = 0; i < neighborPosition.nextNeighborPos.Length; i++)
        {
            Vector3 newPosition = NeighborPosition.GetFloor(neighborPosition.nextNeighborPos[i] + tile.transform.position);
            
            if(!tileTable.ContainsKey(newPosition))
            {
                var newTile = Instantiate(prefabs, transform).GetComponent<DrawTile>();
                newTile.transform.position = newPosition;
                newTile.connectCount++;
                tile.AroundTile.Add(newTile);

                if (newTile != null)
                {
                    tileTable.Add(newTile.transform.position, newTile);
                }
            }else
            {
                tile.AroundTile.Add(tileTable[newPosition]);
                tileTable[newPosition].connectCount++;
            }
        }
    }

    private int FindHexaSector(Vector3 center , Vector3 point)
    {
        float dx = point.x - center.x;
        float dz = point.z - center.z;

        float angle = Mathf.Atan2(dz, dx);
        if (angle < 0) angle += Mathf.PI * 2f;
        float sectorSize = Mathf.PI * 2f / 6f;

        return Mathf.FloorToInt(angle / sectorSize) % 6;
    }
}
