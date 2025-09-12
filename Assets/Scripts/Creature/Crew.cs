using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(SphereCollider))]
public class Crew : MonoBehaviour
{
    public LayerMask mask;
    
    public static int hireCount = 0;
    public static int placeCount = 0;
   
    public float attackRadius;
    public float attackInterval;
    public float lastAttackTime;
    public CrewSpawner spawner;

    public int damage;

    private int killCount;
    private bool isSpawn;
    private bool dragAble;

    public bool DragAble => dragAble;

    private PathTile underTile;
    private SphereCollider collider;
    public EnemyHealth target;
    public List<EnemyHealth> targets;

    public static Action<int> changePlaceCount;
    public static Action<int> changeHireCount;

    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
    }

    public static void Init(Action<int> _changePlaceCount, Action<int> _changeHireCount)
    {
        changePlaceCount = _changePlaceCount;
        changeHireCount = _changeHireCount;
    }

    public void Spawn(CrewSpawner spawner)
    {
        this.spawner = spawner;
        SetDrag();
        collider.radius = attackRadius;
    }

    public void SetDrag()
    {
        dragAble = true;
        isSpawn = false;
    }

    public static void Hire()
    {
        hireCount++;
        changeHireCount?.Invoke(hireCount);
    }
    public static void Sell()
    {
        hireCount--;
        changeHireCount?.Invoke(hireCount);
    }


    private void Update()
    {
        DragDrop();   
        if(!dragAble && target != null)
        {
            if(target.IsDie)
            {
                targets.Remove(target);
                target = GetTarget();
            }

            if (Time.time > lastAttackTime + attackInterval)
            {
                lastAttackTime = Time.time;
                if (target.Hit(damage))
                {
                    killCount++;
                }
            }
        }
        if(target == null)
        {
            target = GetTarget();
        }
    }

    private EnemyHealth GetTarget()
    {
        if(targets.Count == 0)
        {
            return null;
        }
        return targets[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            var find = other.GetComponent<EnemyHealth>();

            if(find != null)
            {
                targets.Add(find);
            }
        }    
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var find = other.GetComponent<EnemyHealth>();
            if(targets.Contains(find))
            {
                targets.Remove(find);
            }
        }
    }

    private void DragDrop()
    {
        if (dragAble)
        {
            Vector3 touchPosition = Vector3.zero;

            if (TouchManager.TouchType == TouchType.Drag)
            {
                if(underTile != null)
                {
                    underTile.Type = TileType.None;
                    placeCount--;
                    changePlaceCount?.Invoke(placeCount);
                    underTile = null;
                }
                touchPosition = TouchManager.GetDragWorldPosition();
                transform.position = new Vector3(touchPosition.x, 1, touchPosition.z);
                isSpawn = true;
                spawner.DragCrew = this;
            }
            else if (TouchManager.TouchType == TouchType.None && isSpawn)
            {
                dragAble = false;

                Ray ray = Camera.main.ScreenPointToRay(TouchManager.GetDragPos());
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask))
                {
                    underTile = hit.collider.GetComponent<PathTile>();
                    if (underTile != null)
                    {
                        if (underTile.Type == TileType.None)
                        {
                            underTile.Type = TileType.Crew;
                            transform.position = underTile.transform.position + Vector3.up * 0.5f;
                            placeCount++;
                            changePlaceCount?.Invoke(placeCount);
                        }
                        else
                        {
                            Destroy(gameObject);
                        }
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
                spawner.DragCrew = null;
            }
        }
    }
}
