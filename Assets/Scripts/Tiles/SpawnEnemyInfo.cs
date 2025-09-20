using TMPro;
using UnityEngine;
using System.Collections.Generic;
public class SpawnEnemyInfo : MonoBehaviour
{
    private int spawnCount;
    private float spawnTime;
    private float timer;

    private TextMeshProUGUI spawnCountText;
    private PathTile spawnTile;
    private EnemySpawner spawner;
    private Enemy prefabs;
    
    private AnimalInfoTable.Data spawnAnimalInfo;
    private GameManager gameManager;
    private List<Enemy> spawnList = new List<Enemy>();
    private Vector3 spawnPoint;
    private void Awake()
    {
        spawnCountText = transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        Debug.Log("Create");
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

    private void Update()
    {
        if (gameManager.WaveStart)
        {
            if(Time.time > spawnTime + timer)
            {
                timer = Time.time;
                Spawn();
            }   
        }
    }

    public void Init(EnemySpawner spawner, PathTile spawnTile , Enemy prefabs , Vector3 drawPosition , Vector3 spawnPosition)
    {
        this.spawner = spawner;
        this.spawnTile = spawnTile;
        this.prefabs = prefabs;
        transform.position = drawPosition;
        spawnPoint = spawnPosition;
    }

    public void SpawnEnemyCount(int count)
    {
        spawnCount = count;
        spawnCountText.text = spawnCount.ToString();
    }

    public void SetSpawnEnemy(AnimalInfoTable.Data spawnAnimalInfo)
    {
        this.spawnAnimalInfo = spawnAnimalInfo;
        spawnTime = this.spawnAnimalInfo.Spawn;
        //Image 스프라이트 설정 필요

        spawnCountText.text = spawnCount.ToString();
    }

    //Test 용 코드임
    //Enemy 테이블 연결시 변경되어야 할듯
    public void Spawn()
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

        enemy.Spawn(spawnTile , spawnAnimalInfo , spawnPoint);
    }
}
