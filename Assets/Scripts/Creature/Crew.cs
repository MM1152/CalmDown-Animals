using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Crew : MonoBehaviour
{
    private readonly int Ani_AttackId = Animator.StringToHash("Attack");

    public CrewRank Rank => (CrewRank)data.Crew_ID;
    public CrewManager spawner;

    public EnemyHealth target;

    private PathTile underTile;
    private List<InTileAnimal> aroundTiles = new List<InTileAnimal>();

    private Animator animator;

    private CrewTable.Data data;
    private float lastAttackTime;
    private float attackRadius;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Spawn(CrewManager spawner , CrewTable.Data data)
    {
        this.spawner = spawner;
        this.data = data;
        
        attackRadius = 1;
    }

    public void SetUnderTile(PathTile tile)
    {
        if(tile != null)
        {
            underTile = tile;
        }

        transform.position = underTile.transform.position + Vector3.up * 0.5f;
        FindAroundTiles();
        underTile.Type = TileType.Crew;
    }

    public void ResetUnderTile()
    {
        underTile.Type = TileType.None;
        foreach(var aroundTile in aroundTiles)
        {
            aroundTile.CheckOutAnimal -= CheckTargetInTile;
        }
        aroundTiles.Clear();
    }

    private void FindAroundTiles()
    {
        List<PathTile> pathties = new List<PathTile>();

        for(int i = 0; i < underTile.Neighbor.Count; i++)
        {
            pathties.Add(underTile.Neighbor[i]);
        }
        
        if (attackRadius > 1)
        {
            for(int i = 0; i < pathties.Count; i++)
            {
                for(int j = 0; j < pathties[i].Neighbor.Count; j++)
                {
                    if(!pathties.Contains(pathties[i].Neighbor[j]))
                    {
                        pathties.Add(pathties[i].Neighbor[j]);
                    }
                }
            }       
        }

        aroundTiles = pathties.Select(x => x.GetComponent<InTileAnimal>()).ToList();

        for(int i = 0; i < aroundTiles.Count; i++)
        {
            aroundTiles[i].CheckOutAnimal += CheckTargetInTile;
        }
    }

    private void CheckTargetInTile(EnemyHealth animal)
    {
        if (animal == target) target = null;
    }

    private void Update()
    {
        if(target != null)
        {
            if(target.IsDie)
            {
                target = null;
            }
            

            if(target != null)
            {
                transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
            }

            if (Time.time > lastAttackTime + data.Crew_atkspd)
            {
                lastAttackTime = Time.time;
                animator.SetTrigger(Ani_AttackId);
                transform.position = underTile.transform.position;
                if (target.Hit(10))
                {
                    Debug.Log("Kill Unit", gameObject);
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
        
        foreach(var tile in aroundTiles)
        {
            var inTileAnimal = tile.GetComponent<InTileAnimal>();
            var animal = inTileAnimal.Get();

            if(animal != null)
            {
                return animal.GetComponent<EnemyHealth>();
            }
        }
        return null;
    }
    
    public int GetCost()
    {
        return data.crewCost;
    }

}
