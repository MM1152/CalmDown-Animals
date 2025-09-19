using System;
using System.Collections.Generic;
using UnityEngine;

public class CrewManager : MonoBehaviour
{
    public LayerMask mask;

    public Crew prefabs;
    private Crew dragCrew;
    public Crew DragCrew { 
        get => dragCrew;
        set {
            dragCrew = value;
            if (dragCrew == null) return;

            crewSellingEvent.goldText.text = dragCrew.GetCost().ToString();
        }
    }
    public CrewSellingEvent crewSellingEvent;
    public PopupManager popupManager;
    private Dictionary<int , (int hire, int place)> unitInfomation = new Dictionary<int, (int hire, int place)>();

    public event Action changeUnitCount;

    public bool IsDrag
    {
        get
        {
            if (DragCrew == null) return false;
            return true; 
        }
    }

    private bool isSpawn;
    private GameManager gamemanager;

    private void Awake()
    { 
        foreach(int crewRank in Enum.GetValues(typeof(CrewRank)))
        {
            unitInfomation.Add(crewRank, (0, 0));
        }
    }

    private void Start()
    {
        gamemanager = GameObject.FindWithTag(TagIds.GameManagerTag)?.GetComponent<GameManager>();
    }

    private void Update()
    {
        DragDrop();
        CrewDrag();
    }

    public void Spawn(CrewRank rank) 
    {
        (int hire, int place) data = unitInfomation[(int)rank];
        if (data.hire - data.place <= 0) return;

        var spawnCrew = Instantiate(prefabs , transform);
        spawnCrew.Spawn(this, DataTableManager.crewTable.Get(rank));
        DragCrew = spawnCrew;
    }

    public void SetStartUnit(CrewRank rank, PathTile underTile)
    {
        var spawnCrew = Instantiate(prefabs, transform);
        spawnCrew.Spawn(this, DataTableManager.crewTable.Get(rank));
        spawnCrew.SetUnderTile(underTile);

        SetHireCount(rank, 1);
        SetPlaceCount(rank, 1);
    }

    public bool CrewHire(CrewRank rank)
    {
        var data = DataTableManager.crewTable.Get(rank);
        if(data != null)
        {
            if(gamemanager.Gold < data.crewCost)
            {
                var popup = (StringPopUp)popupManager.Open(Popup.TextPopUp);
                popup.Id = 3;
                return false;
            }   
        }
        //골드로 판단 로직 넣기
        gamemanager.Gold -= data.crewCost;
        SetHireCount(rank, GetHireCount(rank) + 1);
        return true;
    }

    public int GetHireCount(CrewRank rank)
    {
        return unitInfomation[(int)rank].hire;
    }

    public int GetPlaceCount(CrewRank rank)
    {
        return unitInfomation[(int)rank].place;
    }

    private void SetHireCount(CrewRank rank,int hireCount)
    {
        var info = unitInfomation[(int)rank];
        info.hire = hireCount;
        unitInfomation[(int)rank] = info;
        changeUnitCount?.Invoke();
    }

    private void SetPlaceCount(CrewRank rank, int placeCount)
    {
        var info = unitInfomation[(int)rank];
        info.place = placeCount;
        unitInfomation[(int)rank] = info;
        changeUnitCount?.Invoke();
    }

    private void CrewDrag()
    {
        if (!DragAble.CrewDrag) return;


        // 이미 필드에 소환되어있는 대원 선택시
        if (TouchManager.TouchType == TouchType.Tab)
        {
            // 기존에 드래그하던 대원이 있다면 대원 취소
            if (DragCrew != null)
            {
                DragCrew.SetUnderTile(null);
                SetPlaceCount(DragCrew.Rank, GetPlaceCount(DragCrew.Rank) + 1);
                DragCrew = null;
            }

            Ray ray = Camera.main.ScreenPointToRay(TouchManager.GetDragPos());
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity , ~0 , QueryTriggerInteraction.Ignore))
            {
                // 대원 찾아서 드래그 가능한 상태로 변경
                var find = hit.collider.GetComponent<Crew>();
                if (find != null)
                {
                    find.ResetUnderTile();
                    DragCrew = find;
                    SetPlaceCount(DragCrew.Rank, GetPlaceCount(DragCrew.Rank) - 1);
                }
            }
        }

    }

    private void DragDrop()
    {
        if (!DragAble.CrewDrag) return;

        if (DragCrew != null)
        {
            DragAble.CameraDrag = false;
            Vector3 touchPosition = Vector3.zero;

            if (TouchManager.TouchType == TouchType.Drag)
            {
                touchPosition = TouchManager.GetDragWorldPosition();
                DragCrew.transform.position = new Vector3(touchPosition.x, 1, touchPosition.z);
                isSpawn = true;
            }
            else if (TouchManager.TouchType == TouchType.None && isSpawn)
            {
                DragAble.CameraDrag = true;

                // Check if it is sellable
                if (crewSellingEvent.SellAble)
                {
                    SetHireCount(DragCrew.Rank, GetHireCount(DragCrew.Rank) - 1);
                    gamemanager.Gold += DragCrew.GetCost();
                    Destroy(DragCrew.gameObject);
                    return;
                }

                Ray ray = Camera.main.ScreenPointToRay(TouchManager.GetDragPos());

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask))
                {
                    var underTile = hit.collider.GetComponent<PathTile>();
                    if (underTile != null)
                    {
                        if (underTile.Type == TileType.None)
                        {
                            DragCrew.SetUnderTile(underTile);
                            SetPlaceCount(DragCrew.Rank, GetPlaceCount(DragCrew.Rank) + 1);
                        }
                        else
                        {
                            Destroy(DragCrew.gameObject);
                        }
                    }
                }
                else
                {
                    Destroy(DragCrew.gameObject);
                }
                DragCrew = null;
                isSpawn = false;
            }
        }
    }
}
