using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

[RequireComponent(typeof(SphereCollider))]
public class Crew : MonoBehaviour
{
    public float attackRadius;
    public float attackInterval;
    public float lastAttackTime;
    public CrewManager spawner;

    public int damage;

    private int killCount;

    private PathTile underTile;
    private new SphereCollider collider;
    public EnemyHealth target;
    public List<EnemyHealth> targets;

    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
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


}
