using System;
using System.Collections.Generic;
using UnityEngine;

public class CrewManager : MonoBehaviour
{
    public LayerMask mask;

    public Crew prefabs;
    public Crew DragCrew { get; set; }
    public CrewSellingEvent crewSellingEvent;
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

    private void Awake()
    { 
        foreach(int crewRank in Enum.GetValues(typeof(CrewRank)))
        {
            unitInfomation.Add(crewRank, (0, 0));
        }
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

        DragCrew = Instantiate(prefabs , transform);
        DragCrew.Spawn(this, DataTableManager.crewTable.Get(rank));
    }

    public bool CrewHire(CrewRank rank)
    {
        //골드로 판단 로직 넣기
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

        if (TouchManager.TouchType == TouchType.Tab)
        {
            if (DragCrew != null)
            {
                DragCrew.SetUnderTile(null);
                SetPlaceCount(DragCrew.Rank, GetPlaceCount(DragCrew.Rank) + 1);
                DragCrew = null;
            }

            Ray ray = Camera.main.ScreenPointToRay(TouchManager.GetDragPos());
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity , ~0 , QueryTriggerInteraction.Ignore))
            {
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
                if (crewSellingEvent.SellAble)
                {
                    
                    SetHireCount(DragCrew.Rank, GetHireCount(DragCrew.Rank) - 1);
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
                            DragCrew.transform.position = underTile.transform.position + Vector3.up * 0.5f;
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
