using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputManagerEntry;



public class DrawManager : MonoBehaviour
{
    private enum DrawMode
    {
        Tile,
        Arrive,
        Start,
    }

    public GameObject prefabs;
    public LayerMask mask;
    public TextMeshProUGUI modeText;

    private List<DrawTile> tiles = new List<DrawTile>();
    private Dictionary<Vector3, DrawTile> tileTable = new Dictionary<Vector3, DrawTile>();
    private NeighborPosition neighborPosition;
    private Stack<DrawTile> undoStack = new Stack<DrawTile>();
    private DrawMode mode = DrawMode.Tile;

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
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z) && mode == DrawMode.Tile)
        {
            if(undoStack.Count > 0)
            {
                var undoTile = undoStack.Pop();
                DeleteAroundTile(undoTile);
            }
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D))
        {
            mode = DrawMode.Tile;
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.A))
        {
            mode = DrawMode.Start;
        }

        switch (mode)
        {
            case DrawMode.Tile:
                SelectTile();
                break;
            case DrawMode.Arrive:
                break;
            case DrawMode.Start:
                break;
        }
    }

    private void SelectTile()
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
            undoStack.Push(find);
            SetAroundTile(find);
        }
    }
    private bool FindTile(Ray ray , out DrawTile drawTile)
    {
        drawTile = null;

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask))
        {
            var find = hit.collider.GetComponent<DrawTile>();
            if (find != null && !find.IsDraw)
            {
                drawTile = find;
                return true;
            }
        }

        return false;
    }
    private void DeleteAroundTile(DrawTile tile)
    {
        for(int i = 0; i < tile.AroundTile.Count; i++)
        {
            if (tileTable.ContainsKey(tile.AroundTile[i].transform.position))
            {
                tileTable.Remove(tile.AroundTile[i].transform.position);
            }
            Destroy(tile.AroundTile[i].gameObject);
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
                tile.AroundTile.Add(newTile);

                if (newTile != null)
                {
                    tileTable.Add(newTile.transform.position, newTile);
                }
            }
        }
    }
}
