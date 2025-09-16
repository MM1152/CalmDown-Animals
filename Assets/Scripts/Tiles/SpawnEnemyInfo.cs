using TMPro;
using UnityEngine;
using System.Collections.Generic;
public class SpawnEnemyInfo : MonoBehaviour
{
    private int spawnCount;
    private TextMeshProUGUI spawnCountText;
    private PathTile spawnTile;
    private EnemySpawner spawner;

    private AnimalInfoTable.Data spawnAnimalInfo;
    private GameManager gameManager;
    private List<Enemy> spawnList = new List<Enemy>();

    private void Awake()
    {
        spawnCountText = transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        var find = GameObject.FindWithTag(TagIds.GameManagerTag);
        if(find != null)
        {
            gameManager = find.GetComponent<GameManager>();
            gameManager.endWave += () =>
            {
                foreach(var enemy in spawnList)
                {
                    if(enemy != null)
                    {
                        Destroy(enemy.gameObject);
                    }
                }
                spawnList.Clear();
            };
        }
    }

    public void Init(EnemySpawner spawner, PathTile spawnTile)
    {
        this.spawner = spawner;
        this.spawnTile = spawnTile;
        transform.position = spawnTile.transform.position;
    }

    public void SpawnEnemyCount(int count)
    {
        spawnCount = count;
        spawnCountText.text = spawnCount.ToString();
    }

    public void SetSpawnEnemy(AnimalInfoTable.Data spawnAnimalInfo)
    {
        this.spawnAnimalInfo = spawnAnimalInfo;
        spawnCountText.text = spawnCount.ToString();
    }


    //Test 용 코드임
    //Enemy 테이블 연결시 변경되어야 할듯
    public void Spawn(Enemy prefabs)
    {
        if (spawnCount <= 0) return;

        spawnCount--;
        spawnCountText.text = spawnCount.ToString();

        var enemy = Instantiate(prefabs);
        var health = enemy.GetComponent<EnemyHealth>();
        spawnList.Add(enemy);

        if (health != null)
        {
            health.onDie += spawner.CheckDieEnemy;
        }
        enemy.Spawn(spawnTile , spawnAnimalInfo);
    }
}
