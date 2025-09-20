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

    public void CheckDieEnemy()
    {
        //FIX : 여기 수정 해야됨 모든 동물 수 카운트 이후 0 되면 종료되도록 구현 해ㅐ주어야함.
            
        copySpawnCount--;
        if(copySpawnCount == 0)
        {
            gameManager.EndWave();
        }
    }

    private void EndWaveToSetInfoTiles()
    {
        copySpawnCount = 0;
        // 맵 넓어지면 여기 수정해야됌
        //var spawnInfo = animalInfoTable.RandomGet(DataTableManager.roundTable.Get(gameManager.Wave).CR_ID1);
        for (int i = 0; i < infoTiles.Count; i++)
        {
            var spawnInfo = animalInfoTable.RandomGet(1);
            spawnCount = Random.Range(spawnInfo.Range_min, spawnInfo.Range_max);
            copySpawnCount += spawnCount;
            gameManager.allCountSpawnAnimals += spawnCount;
            infoTiles[i].SpawnEnemyCount(spawnCount);
            infoTiles[i].SetSpawnEnemy(spawnInfo);
        }
    }
    //Test 용
    public SpawnEnemyInfo SettingSpawnInfoTile(PathTile spawnTile, Vector3 drawPosition , Vector3 enemySpawnPosition)
    {
        var spawnInfoTile = Instantiate(infoTile, transform);
        spawnInfoTile.Init(this, spawnTile , prefabs , drawPosition , enemySpawnPosition);
        infoTiles.Add(spawnInfoTile);

        EndWaveToSetInfoTiles();
        return spawnInfoTile;
    }
}
