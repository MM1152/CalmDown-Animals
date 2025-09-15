using TMPro;
using UnityEngine;

public class SpawnEnemyInfo : MonoBehaviour
{
    private int spawnCount;
    private TextMeshProUGUI spawnCountText;
    private PathTile spawnTile;
    private EnemySpawner spawner;

    AnimalInfoTable.Data spawnAnimalInfo;

    private void Awake()
    {
        spawnCountText = transform.GetComponentInChildren<TextMeshProUGUI>();
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
        if(health != null)
        {
            health.onDie += spawner.CheckDieEnemy;
        }
        enemy.Spawn(spawnTile , spawnAnimalInfo);
    }
}
