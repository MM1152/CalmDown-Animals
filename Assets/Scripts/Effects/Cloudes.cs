using UnityEngine;

public class Cloudes : MonoBehaviour
{
    public GameObject[] clouds;
    public TileManager tilemanager;
    public float cloudSpeed;
    private void Start()
    {
        foreach (var cloud in clouds) 
        {
            SetStartPosition(cloud);
        }
    }

    private void Update()
    {
        foreach(var cloud in clouds)
        {
            cloud.transform.position += Vector3.right * cloudSpeed * Time.deltaTime;

            if(cloud.transform.position.x >= 30f)
            {
                SetStartPosition(cloud);
            }
        }
    }

    public void OffAllClouds()
    {
        foreach(var cloud in clouds)
        {
            cloud.SetActive(false);
        }
    }

    public void SetStartPosition(GameObject cloud)
    {
        Vector3 position = new Vector3(Random.Range(-40f , -20f), 5f, Random.Range(tilemanager.DragAbleRect.y, tilemanager.DragAbleRect.w));
        cloud.transform.position = position;
    }
}
