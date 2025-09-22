using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Crew : MonoBehaviour
{
    private readonly int Ani_AttackId = Animator.StringToHash("Attack");
    private readonly int Ani_PlantTrap = Animator.StringToHash("PlantTrap");

    public CrewRank Rank => (CrewRank)data.Crew_ID;
    public CrewManager spawner;
    public EnemyHealth target;
    public List<GameObject> weapons;
    private PathTile underTile;
    private List<InTileAnimal> aroundTiles = new List<InTileAnimal>();

    private Animator animator;

    public Weapon weapon;

    private CrewTable.Data data;
    private float lastAttackTime;
    public int attackRadius;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Spawn(CrewManager spawner , CrewTable.Data data)
    {
        this.spawner = spawner;
        this.data = data;
        weapon = new Weapon(data.rank_ID , weapons , this);
        weapon.Equip(data.Equ_ID);
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
    public void FindAroundTiles()
    {
        if (underTile == null) return;
        List<PathTile> pathTiles = new List<PathTile>();
        for (int i = 0; i < underTile.Neighbor.Count; i++)
        {
            pathTiles.Add(underTile.Neighbor[i]);
        }

        if (attackRadius > 1)
        {
            List<PathTile> copyTiles = pathTiles;
            for (int i = 0; i < attackRadius; i++)
            {
                List<PathTile> saveAroundTile = new List<PathTile>();
                for(int j = 0; j < copyTiles.Count; j++)
                {
                    for(int k = 0; k < copyTiles[j].Neighbor.Count; k++)
                    {
                        if (!pathTiles.Contains(pathTiles[j].Neighbor[k]))
                        {
                            pathTiles.Add(pathTiles[j].Neighbor[k]);
                            saveAroundTile.Add(pathTiles[j].Neighbor[k]);
                        }
                    }
                }
                copyTiles = saveAroundTile;
            }       
        }

        aroundTiles = pathTiles.Select(x => x.GetComponent<InTileAnimal>()).ToList();

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

            if (Time.time > lastAttackTime + data.Crew_atkspd + weapon.GetCaptureSpeed())
            {
                lastAttackTime = Time.time;

                if (weapon.GetWeaponId() == 0)
                {
                    animator.SetTrigger(Ani_AttackId);
                }
                else if (weapon.GetWeaponId() == 1)
                {
                    animator.SetTrigger(Ani_PlantTrap);
                }    
                transform.position = underTile.transform.position;
                if (target.Hit(weapon.GetCaptureDmg()))
                {
                    underTile.CrewKillCount++;
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

    public int GetRank()
    {
        return data.Crew_ID;
    }

    public int GetPayCheck()
    {
        return data.crewPaycheck;
    }
}
