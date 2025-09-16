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

    AnimalInfoTable animalInfoTable;

    private List<SpawnEnemyInfo> infoTiles = new List<SpawnEnemyInfo>();

    private void Start()
    {
        copySpawnCount = spawnCount;
        gameManager.endWave += EndWaveToSetInfoTiles;
        animalInfoTable = DataTableManager.animalInfoTable;
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

    private void EndWaveToSetInfoTiles()
    {
        var spawnInfo = animalInfoTable.RandomGet(DataTableManager.roundTable.Get(gameManager.wave).CR_ID1);
        spawnCount = Random.Range(spawnInfo.Range_min , spawnInfo.Range_max);
        copySpawnCount = spawnCount;

        foreach (var info in infoTiles)
        {
            info.SpawnEnemyCount(copySpawnCount);
            info.SetSpawnEnemy(spawnInfo);
        }
    }
    //Test ¿ë
    public void SettingSpawnInfoTile(PathTile spawnTile, Vector3 drawPosition , Vector3 enemySpawnPosition)
    {
        var spawnInfoTile = Instantiate(infoTile, transform);
        spawnInfoTile.Init(this, spawnTile , drawPosition , enemySpawnPosition);
        infoTiles.Add(spawnInfoTile);

        EndWaveToSetInfoTiles();
    }
}
