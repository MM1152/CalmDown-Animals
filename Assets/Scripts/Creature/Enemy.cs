using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private PathTile spawnTile;
    private PathTile nTile;
    private EnemyHealth health;
    private bool spawn;

    public float speed;

    public void Awake()
    {
        health = GetComponent<EnemyHealth>();
    }

    public void Spawn(PathTile spawnTile)
    {
        this.spawnTile = spawnTile;
        transform.position = this.spawnTile.transform.position;
        nTile = spawnTile.ParentTile;
        health.Init();
        spawn = true;
    }

    private void Update()
    {
        if(spawn && nTile != null)
        {
            Vector3 dir = (nTile.transform.position - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;
            
            if(Vector3.Distance(transform.position , nTile.transform.position) < 0.1f)
            {
                nTile = nTile.ParentTile;
            }

            if(nTile == null)
            {
                Destroy(gameObject);
            }
        }
    }
}
