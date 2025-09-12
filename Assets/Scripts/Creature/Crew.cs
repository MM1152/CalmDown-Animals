using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
[RequireComponent(typeof(SphereCollider))]
public class Crew : MonoBehaviour
{
    public LayerMask mask;
    
    public static int hireCount = 0;
    public static int placeCount = 0;

    public float attackRadius;
    public float attackInterval;
    public float lastAttackTime;

    public int damage;

    private int killCount;
    private bool isSpawn;
    private bool dragAble;

    public bool DragAble => dragAble;

    private SphereCollider collider;
    public EnemyHealth target;
    public List<EnemyHealth> targets;

    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
    }

    public void Spawn()
    {
        dragAble = true;
        isSpawn = false;

        collider.radius = attackRadius;
    }
    public static void Hire()
    {
        hireCount++;
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
                touchPosition = TouchManager.GetDragWorldPosition();
                transform.position = new Vector3(touchPosition.x, 1, touchPosition.z);
                Debug.Log(touchPosition);
                isSpawn = true;
            }
            else if (TouchManager.TouchType == TouchType.None && isSpawn)
            {
                dragAble = false;

                Ray ray = Camera.main.ScreenPointToRay(TouchManager.GetDragPos());
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask))
                {
                    var find = hit.collider.GetComponent<PathTile>();
                    if (find != null)
                    {
                        if (find.Type == TileType.None)
                        {
                            find.Type = TileType.Crew;
                            transform.position = find.transform.position + Vector3.up * 0.5f;
                            placeCount++;
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
            }
        }
    }
}
