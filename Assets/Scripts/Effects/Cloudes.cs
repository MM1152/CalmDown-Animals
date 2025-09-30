using UnityEngine;

public class Cloudes : MonoBehaviour
{
    public GameObject[] clouds;
    public TileManager tilemanager;
    private void Update()
    {

    }

    public void OffAllClouds()
    {
        foreach(var cloud in clouds)
        {
            cloud.SetActive(false);
        }
    }

    public void SetPosition()
    {
        Vector3 position = new Vector3(-100f, 0f, Random.Range(tilemanager.DragAbleRect.y, tilemanager.DragAbleRect.w));

    }
}
