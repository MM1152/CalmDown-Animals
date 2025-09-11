using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy prefabs;
    public TileManager tileManager;

    public void Spawn()
    {
        if (tileManager.GetEndTiles().Length == 0) return;

        int spawnIdx = Random.Range(0, tileManager.GetEndTiles().Length);

        var enemy = Instantiate(prefabs, transform);
        enemy.Spawn(tileManager.GetEndTiles()[spawnIdx]);
    }
}
