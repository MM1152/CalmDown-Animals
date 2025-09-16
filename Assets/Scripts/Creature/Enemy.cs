using UnityEngine;

public class Enemy : MonoBehaviour
{

    private PathTile spawnTile;
    private PathTile nTile;
    private EnemyHealth health;
    private bool spawn;
    private float speed;
    private GameManager gameManager;
    private AnimalInfoTable.Data data;

    public void Awake()
    {
        health = GetComponent<EnemyHealth>();
        var gmObj = GameObject.FindWithTag(TagIds.GameManagerTag);
        if(gmObj != null)
        {
            gameManager = gmObj.GetComponent<GameManager>();
        }
    }

    public void Spawn(PathTile spawnTile , AnimalInfoTable.Data data)
    {
        this.data = data;
        this.spawnTile = spawnTile;

        Vector3 gridSize = spawnTile.GetComponent<Renderer>().bounds.size;

        speed = gridSize.x / this.data.Time;

        nTile = spawnTile.ParentTile;
        health.Init(this.data.MaxHp);

        spawn = true;
        transform.position = this.spawnTile.transform.position;
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
                health.Die();
                gameManager.WaveFail();
            }
        }
    }
}
