using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("REFERENCE")]
    public Enemy prefabs;
    public SpawnEnemyInfo infoTile;
    public TileManager tileManager;
    public GameManager gameManager;

    public TextMeshProUGUI[] sizeText;

    [Header("SPAWN INFOS")]
    public int spawnCount;
    public int copySpawnCount;
    public float spawnInterval;
    private float lastSpawnTime;

    AnimalInfoTable animalInfoTable;

    private List<SpawnEnemyInfo> infoTiles = new List<SpawnEnemyInfo>();

    private void Awake()
    {
        copySpawnCount = spawnCount;
        gameManager.endWave += EndWaveToSetInfoTiles;

        animalInfoTable = DataTableManager.animalInfoTable;
    }

    public SpawnEnemyInfo GetInfoTileInTutorial()
    {
        return infoTiles[0];
    }

    public void SetSizeColor(int sizeId)
    {
        sizeText[sizeId].color = Color.white;
    }

    public void ClearAllText()
    {
        foreach(var text in sizeText) { 
            text.color = Color.black;
        }
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

    public void ClearAllAnimals()
    {
        foreach(var info in infoTiles)
        {
            info.ClearAllAnimals();
        }
    }

    private void EndWaveToSetInfoTiles()
    {
        copySpawnCount = 0;
        ClearAllText();
        // 맵 넓어지면 여기 수정해야됌
        //var spawnInfo = animalInfoTable.RandomGet(DataTableManager.roundTable.Get(gameManager.Wave).CR_ID1);
        for (int i = 0; i < infoTiles.Count; i++)   
        {
            var spawnInfo = animalInfoTable.RandomGet(DataTableManager.roundTable.Get(gameManager.Wave).CR_IDS[i]);
            SetSizeColor(spawnInfo.Size_ID);
            //TEST 
            //var spawnInfo = animalInfoTable.Get(207);
            //Debug.Log(spawnInfo.Skin.name);
            spawnCount = Random.Range(spawnInfo.Range_min, spawnInfo.Range_max);
            copySpawnCount += spawnCount;
            gameManager.AllAnimalSpawnCount += spawnCount;
            infoTiles[i].SpawnEnemyCount(spawnCount);
            infoTiles[i].SetSpawnEnemy(spawnInfo);
        }
    }
    public void RemoveInfoTile(SpawnEnemyInfo infotile)
    {
        infoTiles.Remove(infotile);
        Destroy(infotile.gameObject);
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
