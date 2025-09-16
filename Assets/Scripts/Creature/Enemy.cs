using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    private PathTile nTile;
    private PathTile prevTile;

    private EnemyHealth health;
    private GameManager gameManager;
    private AnimalInfoTable.Data data;
    
    private float speed;
    private bool spawn;
    private Vector3 endPoint;
    private Vector3 nPos;
    private Vector3 dir;

    private Animator animator;
    public void Awake()
    {
        health = GetComponent<EnemyHealth>();
        var gmObj = GameObject.FindWithTag(TagIds.GameManagerTag);
        if(gmObj != null)
        {
            gameManager = gmObj.GetComponent<GameManager>();
        }

    }

    public void Spawn(PathTile spawnTile , AnimalInfoTable.Data data , Vector3 spawnPoint)
    {
        this.data = data;
        Vector3 gridSize = spawnTile.GetComponent<Renderer>().bounds.size;

        speed = gridSize.x / this.data.Time;
        nTile = spawnTile;
        health.Init(this.data.MaxHp);

        spawn = true;
        transform.position = spawnPoint;

        nPos = nTile.transform.position;
        dir = (nPos - transform.position).normalized;
        transform.LookAt(nPos);

        GameObject skin = Instantiate(data.Skin, transform);
        animator = skin.AddComponent<Animator>();
        animator.runtimeAnimatorController = data.Animator;
        animator.avatar = data.Avatar;
    }

    private void Update()
    {
        if(spawn && nTile != null)
        {
            transform.position += dir * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position , nPos) < 0.1f)
            {
                if (nTile.ParentTile == null)
                {
                    nPos = nTile.ArriveDrawTile.transform.position;
                    dir = (nPos - transform.position).normalized;
                    nTile = nTile.ParentTile;
                    transform.LookAt(nPos);
                }
                else
                {
                    nTile = nTile.ParentTile;
                    nPos = nTile.transform.position;
                    dir = (nPos - transform.position).normalized;
                    transform.LookAt(nPos);
                }
            }
        }

        else if(spawn && nTile == null)
        {
            transform.position += dir * speed * Time.deltaTime;

            if (Vector3.Distance(nPos , transform.position) < 0.05f)
            {
                health.Die();
                gameManager.WaveFail();
            }
        }
    }
}
