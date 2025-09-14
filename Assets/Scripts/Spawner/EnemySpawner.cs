using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("REFERENCE")]
    public Enemy prefabs;
    public SpawnEnemyInfo infoTile;
    public TileManager tileManager;
    public GameManager gameManager;

    [Header("SPAWN INFOS")]
    public int spawnCount;
    public int copySpawnCount;
    public float spawnInterval;
    private float lastSpawnTime;

    private List<SpawnEnemyInfo> infoTiles = new List<SpawnEnemyInfo>();

    private void Start()
    {
        copySpawnCount = spawnCount;
        gameManager.endWave += () => copySpawnCount = spawnCount;
        gameManager.endWave += SetInfoTiles;
    }

    private void Update()
    {
        if(gameManager.WaveStart)
        {
            if(lastSpawnTime + spawnInterval < Time.time)
            {
                lastSpawnTime = Time.time;

                for(int i = 0; i < infoTiles.Count; i++)
                {
                    infoTiles[i].Spawn(prefabs);
                }
            }
        }
    }

    public void CheckDieEnemy()
    {
        copySpawnCount--;
        if(copySpawnCount == 0)
        {
            gameManager.EndWave();
        }
    }

    private void SetInfoTiles()
    {
        foreach(var info in infoTiles)
        {
            info.SpawnEnemyCount(copySpawnCount);
        }
    }
    //Test ¿ë
    public void SettingSpawnInfoTile(PathTile spawnTile , Vector2 gridSize)
    {
        Vector3 newPosition = (Vector3)(Vector3.right  * gridSize) + spawnTile.transform.position;
        var spawnInfoTile = Instantiate(infoTile, transform);
        spawnInfoTile.SetSpawnEnemy(newPosition, null, copySpawnCount, spawnTile);
        spawnInfoTile.Init(this);
        infoTiles.Add(spawnInfoTile);
    }
}
