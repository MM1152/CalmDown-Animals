using System;
using System.Collections.Generic;
using UnityEngine;

public class CrewManager : MonoBehaviour
{
    public LayerMask mask;

    public Crew prefabs;
    public Crew DragCrew { get; set; }
    private List<(int hire, int place)> unitInfomation = new List<(int hire, int place)>();

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
        int length = Enum.GetNames(typeof(CrewRank)).Length;
        for(int i = 0; i < length; i++)
        {
            unitInfomation.Add((0, 0));
        }
    }

    private void Update()
    {
        DragDrop();
        CrewDrag();
    }

    public void Spawn(CrewRank rank)
    {
        (int hire, int place) data = unitInfomation[(int)CrewRank.Intern];
        if (data.hire - data.place <= 0) return;

        DragCrew = Instantiate(prefabs , transform);
        DragCrew.Spawn(this);
    }

    public bool CrewHire(CrewRank rank)
    { 
        //골드로 판단 로직 넣기
        var info = unitInfomation[(int)CrewRank.Intern];
        info.hire++;

        unitInfomation[(int)CrewRank.Intern] = info;
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

    private void CrewDrag()
    {
        if (TouchManager.TouchType == TouchType.Tab)
        {
            if (DragCrew != null) DragCrew.SetUnderTile(null);

            Ray ray = Camera.main.ScreenPointToRay(TouchManager.GetDragPos());
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                
                var find = hit.collider.GetComponent<Crew>();
                if (find != null)
                {
                    var info = unitInfomation[(int)CrewRank.Intern];
                    info.place--;
                    unitInfomation[(int)CrewRank.Intern] = info;

                    find.ResetUnderTile();
                    DragCrew = find;
                }
            }
        }
    }

    private void DragDrop()
    {
        if (DragCrew != null)
        {
            Vector3 touchPosition = Vector3.zero;

            if (TouchManager.TouchType == TouchType.Drag)
            {
                touchPosition = TouchManager.GetDragWorldPosition();
                DragCrew.transform.position = new Vector3(touchPosition.x, 1, touchPosition.z);
                isSpawn = true;
            }
            else if (TouchManager.TouchType == TouchType.None && isSpawn)
            {
                Ray ray = Camera.main.ScreenPointToRay(TouchManager.GetDragPos());
                var info = unitInfomation[(int)CrewRank.Intern];

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask))
                {
                    var underTile = hit.collider.GetComponent<PathTile>();
                    if (underTile != null)
                    {
                        if (underTile.Type == TileType.None)
                        {
                            DragCrew.transform.position = underTile.transform.position + Vector3.up * 0.5f;
                            DragCrew.SetUnderTile(underTile);
                            info.place++;
                        }
                        else
                        {
                            info.place--;
                            Destroy(DragCrew.gameObject);
                        }
                    }
                }
                else
                {
                    info.place--;
                    Destroy(DragCrew.gameObject);
                }
                DragCrew = null;
                isSpawn = false;
                unitInfomation[(int)CrewRank.Intern] = info;
            }
        }
    }
}
