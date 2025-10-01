using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CrewManager : MonoBehaviour
{
    private readonly string FormatingPath = "Enemy/{0}";
    private readonly string Intern = "InternCrew";
    private readonly string Rookie = "RookieCrew";
    private readonly string Senior = "SeniorCrew";
    private readonly string Ace = "AceCrew";
    
    public LayerMask mask;

    public Dictionary<CrewRank , Crew> prefabs = new Dictionary<CrewRank, Crew>();
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

    public Dictionary<CrewRank , (int hire, int place)> unitInfomation = new Dictionary<CrewRank, (int hire, int place)>();
    private Dictionary<CrewRank, List<Vector3>> placePosition = new Dictionary<CrewRank, List<Vector3>>();

    public event Action changeUnitCount;

    public event Action CrewPlaceInTutorial;
    public event Action OnClickCrew;
    public event Action ReturnCrewInTutorial;
    public event Action SellingEventInTutorial;

    private List<Crew> placedCrews = new List<Crew>();
    private int Payment {
        get => gamemanager.Payment;
        set
        {
            gamemanager.Payment = value;
        }
    }

    public bool IsDrag
    {
        get
        {
            if (DragCrew == null) return false;
            return true; 
        }
    }

    private bool isSpawn;
    public GameManager gamemanager;

    private void Awake()
    { 
        foreach(var crewRank in Enum.GetValues(typeof(CrewRank)))
        {
            unitInfomation.Add((CrewRank)crewRank, (0, 0));
            placePosition.Add((CrewRank)crewRank, new List<Vector3>());
        }

        prefabs.Add(CrewRank.Intern, Resources.Load<Crew>(string.Format(FormatingPath, Intern)));
        prefabs.Add(CrewRank.Newbie, Resources.Load<Crew>(string.Format(FormatingPath, Rookie)));
        prefabs.Add(CrewRank.Senior, Resources.Load<Crew>(string.Format(FormatingPath, Senior)));
        prefabs.Add(CrewRank.Ace, Resources.Load<Crew>(string.Format(FormatingPath, Ace)));
    }

    private void Start()
    {
        gamemanager.endWave += () =>
        {
            SaveLoadManager.Data.employCrewCount = unitInfomation;
            SaveLoadManager.Data.crewSpawn = placePosition;
        };
    }

    public void UpdateCrewStatus()
    {
        for(int i = 0; i < placedCrews.Count; i++)
        {
            if (placedCrews[i].CheckUnderTile())
            {
                Destroy(placedCrews[i].gameObject);
                placedCrews.Remove(placedCrews[i]);
                i--;
            }
        }
    }

    private void Update()
    {
        DragDrop();
        CrewDrag();
    }

    public Crew GetCrewInTutorial()
    {
        return placedCrews[0];
    }

    public int GetPlaceCrewCount()
    {
        return placedCrews.Count;
    }

    public void Spawn(CrewRank rank) 
    {
        (int hire, int place) data = unitInfomation[rank];
        if (data.hire - data.place <= 0) return;

        var spawnCrew = Instantiate(prefabs[rank] , transform);
        spawnCrew.Spawn(this, DataTableManager.crewTable.Get(rank));
        DragCrew = spawnCrew;
    }

    public void CrewForcingSpawn(CrewRank rank , PathTile underTile)
    {
        var spawnCrew = Instantiate(prefabs[rank], transform);
        spawnCrew.Spawn(this, DataTableManager.crewTable.Get(rank));
        spawnCrew.SetUnderTile(underTile);

        placedCrews.Add(spawnCrew);
        spawnCrew.UnShowAttackRadius();
        DragCrew = null;
    }

    public void SetStartUnit(CrewRank rank, PathTile underTile)
    {
        var spawnCrew = Instantiate(prefabs[rank], transform);
        spawnCrew.Spawn(this, DataTableManager.crewTable.Get(rank));
        spawnCrew.SetUnderTile(underTile);

        SetHireCount(rank, 1);
        SetPlaceCount(rank, 1);
        placedCrews.Add(spawnCrew);
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
        return unitInfomation[rank].hire;
    }

    public int GetPlaceCount(CrewRank rank)
    {
        return unitInfomation[rank].place;
    }

    private void SetHireCount(CrewRank rank,int hireCount)
    {
        var info = unitInfomation[rank];
        info.hire = hireCount;
        unitInfomation[rank] = info;
        changeUnitCount?.Invoke();

    }

    private void SetPlaceCount(CrewRank rank, int placeCount)
    {
        var info = unitInfomation[rank];

        bool more = placeCount > info.place;

        info.place = placeCount;
        unitInfomation[rank] = info;
        changeUnitCount?.Invoke();

        if(more)
        {
            Payment += DataTableManager.crewTable.Get(rank).crewPaycheck;
        }else
        {
            Payment -= DataTableManager.crewTable.Get(rank).crewPaycheck;
        }
    }

    public void ClearDragCrew()
    {
        if (DragCrew != null)
        {
            placePosition[DragCrew.Rank].Add(DragCrew.UnderTile.transform.position);
            DragCrew.SetUnderTile(null);
            DragCrew.UnShowAttackRadius();
            placedCrews.Add(DragCrew);
            SetPlaceCount(DragCrew.Rank, GetPlaceCount(DragCrew.Rank) + 1);
            DragCrew = null;
        }
    }

    private void CrewDrag()
    {
        if (!Status.CrewDrag && !Status.CrewTab) return;


        // 이미 필드에 소환되어있는 대원 선택시
        if (TouchManager.touchType == TouchType.Tab && !TouchManager.TouchStartInUI())
        {
            // 기존에 드래그하던 대원이 있다면 대원 취소
            ClearDragCrew();

            Ray ray = Camera.main.ScreenPointToRay(TouchManager.GetDragPos());
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity , ~0 , QueryTriggerInteraction.Ignore))
            {
                // 대원 찾아서 드래그 가능한 상태로 변경
                var find = hit.collider.GetComponent<Crew>();
                if (find != null)
                {
                    OnClickCrew?.Invoke();
                    find.ShowAttackRaius();
                    find.ResetUnderTile();
                    DragCrew = find;
                    SetPlaceCount(DragCrew.Rank, GetPlaceCount(DragCrew.Rank) - 1);
                    placedCrews.Remove(DragCrew);
                }
            }
        }
    }

    public void ClearAllCrews()
    {
        foreach(var placedCrew in placedCrews)
        {
            Destroy(placedCrew.gameObject);
            placedCrew.ResetUnderTile();
            SetPlaceCount(placedCrew.Rank, GetPlaceCount(placedCrew.Rank) - 1);
        }

        placedCrews.Clear();
    }

    public void ClearCrew(Crew crew)
    {
        SetPlaceCount(crew.Rank, GetPlaceCount(crew.Rank) - 1);
    }

    private void DragDrop()
    {
        if (!Status.CrewDrag) return;

        if (DragCrew != null)
        {
            Status.CameraDrag = false;
            Vector3 touchPosition = Vector3.zero;

            if (TouchManager.touchType == TouchType.Drag)
            {
                touchPosition = TouchManager.GetDragWorldPosition();
                DragCrew.transform.position = new Vector3(touchPosition.x, 1, touchPosition.z);
                isSpawn = true;
                DragCrew.UnShowAttackRadius();
            }
            else if (TouchManager.touchType == TouchType.None && isSpawn)
            {
                Status.CameraDrag = true;

                // Check if it is sellable
                if (crewSellingEvent.SellAble)
                {
                    SetHireCount(DragCrew.Rank, GetHireCount(DragCrew.Rank) - 1);
                    gamemanager.Gold += DragCrew.GetCost();
                    Destroy(DragCrew.gameObject);
                    SellingEventInTutorial?.Invoke();
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
                            CrewPlaceInTutorial?.Invoke();
                            placePosition[DragCrew.Rank].Add(underTile.transform.position);
                            SetPlaceCount(DragCrew.Rank, GetPlaceCount(DragCrew.Rank) + 1);
                            SoundManager.Instance.PlayOneShot(SFX.PlaceCrewSound);
                            placedCrews.Add(DragCrew);
                        }
                        else
                        {
                            Destroy(DragCrew.gameObject);
                            ReturnCrewInTutorial?.Invoke();
                        }
                    }
                }
                else
                {
                    Destroy(DragCrew.gameObject);
                    ReturnCrewInTutorial?.Invoke();
                }
                DragCrew.UnShowAttackRadius();
                DragCrew = null;
                isSpawn = false;
            }
        }
    }
}
