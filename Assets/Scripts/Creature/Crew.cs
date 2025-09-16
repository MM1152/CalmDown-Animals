using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class Crew : MonoBehaviour
{
    private readonly int Ani_AttackId = Animator.StringToHash("Attack");

    public CrewRank rank;
    public float attackRadius;
    public float attackInterval;
    public float lastAttackTime;
    public CrewManager spawner;

    public int damage;

    private int killCount;

    private PathTile underTile;
    private new SphereCollider collider;
    private Animator animator;
    private GameManager gameManager;

    public EnemyHealth target;
    public List<EnemyHealth> targets;
    
    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
        animator = GetComponent<Animator>();
        
    }

    private void Start()
    {
        var find = GameObject.FindWithTag(TagIds.GameManagerTag);
        if (find != null)
        {
            gameManager = find.GetComponent<GameManager>();
            gameManager.endWave += () => targets.Clear();
        }
    }

    public void Spawn(CrewManager spawner)
    {
        this.spawner = spawner;
        collider.radius = attackRadius;
    }

    public void SetUnderTile(PathTile tile)
    {
        if(tile != null)
        {
            underTile = tile;
        }

        underTile.Type = TileType.Crew;
    }
    public void ResetUnderTile()
    {
        underTile.Type = TileType.None;
    }

    private void Update()
    {
        if(target != null)
        {
            if(target.IsDie)
            {
                targets.Remove(target);
                target = GetTarget();
            }
            
            if(target != null)
            {
                transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
            }

            if (Time.time > lastAttackTime + attackInterval)
            {
                lastAttackTime = Time.time;
                animator.SetTrigger(Ani_AttackId);
                transform.position = underTile.transform.position;
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
        foreach(var target in targets)
        {
            if(target != null)
            {
                return target;
            }
        }
        return null;
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
                target = null;
                targets.Remove(find);
            }
        }
    }


}
