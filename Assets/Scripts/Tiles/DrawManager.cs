using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEngine.InputManagerEntry;
public class DrawManager : MonoBehaviour
{
    public enum DrawMode
    {
        Tile,
        Arrive,
        Start,
        Delete,
    }

    public GameObject prefabs;
    public DrawTile startTilePrefabs;
    public LayerMask mask;
    public TextMeshProUGUI modeText;
    public TMP_Dropdown mapLevelDropBox;
    public TMP_Dropdown mapDataDropBox;

    private List<DrawTile> tiles = new List<DrawTile>();
    private Dictionary<Vector3, DrawTile> tileTable = new Dictionary<Vector3, DrawTile>();

    private NeighborPosition neighborPosition;
    private Stack<DrawTile> drawTileUndoStack = new Stack<DrawTile>();

    private DrawTile startTile;
    private Stack<DrawTile> startTileUndoStack = new Stack<DrawTile>();

    private DrawTile arriveTile;
    private DrawTile prevArriveTile;

    private List<List<DrawTile>> waveToTiles = new List<List<DrawTile>>();
    private int level = 0;

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

            if (prevArriveTile != null)
            {
                Destroy(prevArriveTile.gameObject);
                prevArriveTile = null;
            }

            switch (mode)
            {
                case DrawMode.Tile:
                    modeText.text = "타일 그리기";
                    break;
                case DrawMode.Arrive:
                    modeText.text = "도착타일 설정";
                    break;
                case DrawMode.Start:
                    modeText.text = "시작타일 설정";
                    break;
                case DrawMode.Delete:
                    modeText.text = "삭제 모드";
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
        find.connectCount = 10000000;

        if (find != null)
        {
            find.Draw(level);
            tiles.Add(find);
            tileTable.Add(find.transform.position , find);

            waveToTiles.Add(new List<DrawTile>());
            waveToTiles[level].Add(find);

            SetAroundTile(find);
        }
    }

    private void Start()
    {
        SettingMapDataDropBox();
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
                DeleteStartTile(undoStartTile);
            }
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            Mode = DrawMode.Tile;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            Mode = DrawMode.Start;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            Mode = DrawMode.Arrive;
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            Mode = DrawMode.Delete;
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            var data = Map.CreateMapData(DataTableIds.MapDataIds[0], waveToTiles);
            Map.Save(0, data);
        }

        switch (mode)
        {
            case DrawMode.Tile:
                UpdateDrawTile();
                break;
            case DrawMode.Arrive:
                UpdateArriveTile();
                break;
            case DrawMode.Start:
                UpdateDrawStartTile();
                break;
            case DrawMode.Delete:
                UpdateDeleteTile();
                break;
        }
    }
    private void UpdateDeleteTile(){
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(FindTile(ray , out DrawTile find))
        {
            if(find != null && level == find.layer)
            {
                if (find == tiles[0]) return;

                var enumer = drawTileUndoStack.Where(x => x != find);
                drawTileUndoStack = new Stack<DrawTile>(enumer);

                DeleteAroundTile(find);
            }
        }
    }
    private void UpdateArriveTile()
    {
        if(prevArriveTile == null)
        {
            prevArriveTile = Instantiate(startTilePrefabs, transform);
            prevArriveTile.DrawType = DrawType.Arrive;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (FindTile(ray, out DrawTile tile))
        {
            if (tile != null && tile.DrawType == DrawType.None && tile.IsDraw)
            {
                Vector3 mousePos = Input.mousePosition;
                Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));

                int sector = FindHexaSector(tile.transform.position, mousePosInWorld);
                Debug.Log(sector);

                if (tile.AroundTile[sector].IsDraw || !tile.AroundTile[sector].gameObject.activeSelf) return;

                prevArriveTile.transform.position = tile.AroundTile[sector].transform.position;
                prevArriveTile.transform.rotation = Quaternion.Euler(-90f, (sector + 1) * -60f, 0f);

                if (Input.GetMouseButtonDown(0))
                {
                    var find = tile.AroundTile[sector];
                    find.gameObject.SetActive(false);

                    prevArriveTile.UnderTile = find;
                    prevArriveTile.ConnectTile = tile;
                    tile.ConnectStartTiles.Add(prevArriveTile);

                    if (arriveTile != null)
                    {
                        arriveTile.ConnectStartTiles.Remove(arriveTile);
                        arriveTile.UnderTile.gameObject.SetActive(true);
                        Destroy(arriveTile.gameObject);
                    }

                    arriveTile = prevArriveTile;
                    prevArriveTile = null;
                }
            }
        }
    }
    private void UpdateDrawStartTile()
    {
        if (startTile == null)
        {
            startTile = Instantiate(startTilePrefabs, transform);
            startTile.DrawType = DrawType.Start;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(FindTile(ray , out DrawTile tile))
        {
            if (tile != null && tile.DrawType == DrawType.None && tile.IsDraw)
            {
                Vector3 mousePos = Input.mousePosition;
                Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));

                int sector = FindHexaSector(tile.transform.position, mousePosInWorld);
                Debug.Log(sector);

                if (tile.AroundTile[sector].IsDraw || !tile.AroundTile[sector].gameObject.activeSelf) return;

                startTile.transform.position = tile.AroundTile[sector].transform.position;
                startTile.transform.rotation = Quaternion.Euler(-90f, (sector + 1) * -60f, 0f);

                if (Input.GetMouseButtonDown(0))
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
    private void DeleteStartTile(DrawTile undoStartTile)
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
            if (find.IsDraw) return;
            find.Draw(level);
            tiles.Add(find);

            waveToTiles[level].Add(find);

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
            if (find != null && find.DrawType == DrawType.None)
            {
                drawTile = find;
                return true;
            }
        }
        return false;
    }
    private void DeleteAroundTile(DrawTile tile , bool allTileClear = false)
    {
        //연결된 시작타일 제거
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

        //주변 연결된 타일 제거
        for (int i = 0; i < tile.AroundTile.Count; i++)
        {
            tile.AroundTile[i].connectCount--;
            if (tile.AroundTile[i].connectCount <= 0 && !tile.AroundTile[i].IsDraw)
            {
                tileTable.Remove(tile.AroundTile[i].transform.position);
                Destroy(tile.AroundTile[i].gameObject);
            }
        }
        if(tile.connectCount <= 0 || allTileClear)
        {
            tiles.Remove(tile);
            waveToTiles[level].Remove(tile);
            Destroy(tile.gameObject);
        }else
        {
            tile.Undo();
        }
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
    public void DrawTileToLevel(int level)
    {
        if(this.level != level)
        {
            drawTileUndoStack.Clear();
        }

        this.level = level;
        int idx = -1;

        for(int i = 0; i <= this.level; i++)
        {
            if(waveToTiles.Count == i)
            {
                idx = i;
                break;
            }
        }
        
        if(idx != -1)
        {
            waveToTiles.Add(new List<DrawTile>());
            mapLevelDropBox.value = idx;
            mapLevelDropBox.RefreshShownValue();
        }

        foreach(var tile in tiles)
        {
            if(tile.layer == level)
            {
                tile.SetActive(true);
            }
            else
            {
                tile.SetActive(false);
            } 
        }
    }

    private void SettingMapDataDropBox()
    {
        var datas = Map.mapDatas;
        List<string> optionList = new List<string>();
        for(int i = 0; i < datas.Count; i++)
        {
            optionList.Add(datas[i].Id);
        }

        optionList.Add(" ");

        mapDataDropBox.AddOptions(optionList);
    }

    public void ChangeMapToMapData(int mapIndex)
    {
        Clear();
        level = 0;
        mapLevelDropBox.value = level;
        mapLevelDropBox.RefreshShownValue();

        if (Map.mapDatas.Count <= mapIndex)
        {
            var tile = Instantiate(prefabs, transform).GetComponent<DrawTile>();
            tile.layer = level;
            tile.Draw(level);

            tiles.Add(tile);
            tileTable.Add(tile.transform.position, tile);

            waveToTiles.Add(new List<DrawTile>());
            waveToTiles[level].Add(tile);

            SetAroundTile(tile);
            return;

        }

        var datas = Map.mapDatas[mapIndex];

        List<DrawTile> startTiles = new List<DrawTile>();
        DrawTile arriveTile = null;

        for(int i = 0; i < datas.tiles.Count; i++)
        {
            waveToTiles.Add(new List<DrawTile>());
            List<DrawTile> waveTiles = new List<DrawTile>();
            for (int j = 0; j < datas.tiles[i].Count; j++)
            {
                var tileData = datas.tiles[i][j];
                DrawTile tile;
                if (tileData.DrawType == DrawType.Start || tileData.DrawType == DrawType.Arrive)
                {
                    tile = Instantiate(startTilePrefabs, transform);
                } 
                else
                {
                    if(!tileTable.ContainsKey(tileData.Position))
                    {
                        tile = Instantiate(prefabs, transform).GetComponent<DrawTile>();
                        tileTable.Add(tileData.Position, tile);
                        waveTiles.Add(tile);
                        if(tileData.Position == Vector3.zero)
                        {
                            tile.connectCount = 10000000;
                        }
                    }else
                    {
                        tile = tileTable[tileData.Position];
                    }
                }

                if (tile != null)
                {
                    tile.UpdateDrawTile(tileData);
                    tile.Draw(i);
                    SetAroundTile(tile);
                    tiles.Add(tile);

                    if (tileData.DrawType == DrawType.Start)
                    {
                        tile.UnderTile = tileTable[tile.transform.position];
                        startTiles.Add(tile);
                    }
                    else if(tileData.DrawType == DrawType.Arrive)
                    {
                        tile.UnderTile = tileTable[tile.transform.position];
                        arriveTile = tile;
                    }
                }
            }
            waveToTiles.Add(waveTiles);
        }

        if (arriveTile != null && tileTable.ContainsKey(arriveTile.ConnectPos))
        {
            startTile.ConnectTile = tileTable[arriveTile.ConnectPos];
        }

        for (int i = 0; i < startTiles.Count; i++)
        {
            if (tileTable.ContainsKey(startTiles[i].ConnectPos))
            {
                startTile.ConnectTile = tileTable[startTiles[i].ConnectPos]; 
            }
        }

        DrawTileToLevel(level);
    }

    private void Clear()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            DeleteAroundTile(tiles[i] , true);
        }

        waveToTiles.Clear();
        tiles.Clear();
        tileTable.Clear();
        drawTileUndoStack.Clear();
        startTileUndoStack.Clear();
    }
}
