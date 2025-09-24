using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Crew : MonoBehaviour
{
    private readonly int Ani_AttackId = Animator.StringToHash("Attack");
    private readonly int Ani_PlantTrap = Animator.StringToHash("PlantTrap");
    private readonly int Ani_ShootGun = Animator.StringToHash("ShootGun");

    public CrewRank Rank => (CrewRank)data.Crew_ID;
    public CrewManager spawner;
    public EnemyHealth target;
    public List<GameObject> weapons;
    private PathTile underTile;
    public PathTile UnderTile => underTile;
    public List<InTileAnimal> aroundTiles = new List<InTileAnimal>();

    private Animator animator;

    public Weapon weapon;
    //0.037 , 0.131 , 0  , 90
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
        weapon.Equip(data.equType_ID);
    }

    public void ShowAttackRaius()
    {
        foreach(var aroundtile in aroundTiles)
        {
            if(aroundtile.GetComponent<PathTile>().Type == TileType.Path)
            {
                aroundtile.ChangeColor(Color.red);
            }
        }
    }

    public void UnShowAttackRadius()
    {
        foreach (var aroundtile in aroundTiles)
        {
            if (aroundtile.GetComponent<PathTile>().Type == TileType.Path)
            {
                aroundtile.ResetColor();
            }
        }
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
    }
    public void FindAroundTiles()
    {
        if (underTile == null) return;

        UnShowAttackRadius();
        ResetUnderTile();

        List<PathTile> pathTiles = new List<PathTile>();
        for (int i = 0; i < underTile.Neighbor.Count; i++)
        {
            pathTiles.Add(underTile.Neighbor[i]);
        }

        if (attackRadius > 1)
        {
            List<PathTile> copyTiles = new List<PathTile>(pathTiles);
            for (int i = 1; i < attackRadius; i++)
            {
                List<PathTile> saveAroundTile = new List<PathTile>();
                foreach(var tile in copyTiles)
                {
                    for (int k = 0; k < tile.Neighbor.Count; k++)
                    {
                        if (!pathTiles.Contains(tile.Neighbor[k]))
                        {
                            pathTiles.Add(tile.Neighbor[k]);
                            saveAroundTile.Add(tile.Neighbor[k]);
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

        ShowAttackRaius();
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
                else if (weapon.GetWeaponId() == 2)
                {
                    animator.SetTrigger(Ani_ShootGun);
                }
                transform.position = underTile.transform.position;
                if (target.Hit(weapon.GetCaptureDmg()))
                {
                    underTile.CrewKillCount++;
                    spawner.gamemanager.captureAnimalCount++;
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
            var animal = inTileAnimal.Get(weapon.GetCaptureSize());

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
