using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy prefabs;
    public TileManager tileManager;

    public void Spawn()
    {
        int spawnIdx = Random.Range(0, tileManager.GetEndTiles().Length);

        var enemy = Instantiate(prefabs, transform);
        enemy.Spawn(tileManager.GetEndTiles()[spawnIdx]);
    }
}
