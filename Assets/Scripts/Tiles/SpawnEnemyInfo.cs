using TMPro;
using UnityEngine;

public class SpawnEnemyInfo : MonoBehaviour
{
    private int spawnCount;
    private TextMeshProUGUI spawnCountText;
    private PathTile spawnTile;
    private EnemySpawner spawner;

    private void Awake()
    {
        spawnCountText = transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Init(EnemySpawner spawner)
    {
        this.spawner = spawner;
    }

    public void SpawnEnemyCount(int count)
    {
        spawnCount = count;
        spawnCountText.text = spawnCount.ToString();
    }

    public void SetSpawnEnemy(Vector3 position, Sprite image, int count , PathTile spawnTile)
    {
        transform.position = position;
        spawnCount = count;
        this.spawnTile = spawnTile;

        spawnCountText.text = spawnCount.ToString();
    }


    //Test 용 코드임
    //Enemy 테이블 연결시 변경되어야 할듯
    public void Spawn(Enemy spawnEnemy)
    {
        if (spawnCount <= 0) return;

        spawnCount--;
        spawnCountText.text = spawnCount.ToString();

        var enemy = Instantiate(spawnEnemy);
        var health = enemy.GetComponent<EnemyHealth>();
        if(health != null)
        {
            health.onDie += spawner.CheckDieEnemy;
        }
        enemy.Spawn(spawnTile);
    }
}
