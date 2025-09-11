using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;
using static UnityEngine.InputManagerEntry;
public class DrawManager : MonoBehaviour
{
    public GameObject prefabs;
    public LayerMask mask;
    private Vector2 gridSize;

    private List<DrawTile> tiles = new List<DrawTile>();
    private Dictionary<Vector3, DrawTile> tileTable = new Dictionary<Vector3, DrawTile>();

    private NeighborPosition neighborPosition;

    private void Awake()
    {
        Renderer sp = prefabs.GetComponent<Renderer>();
        neighborPosition = new NeighborPosition(sp);

        var tile = Instantiate(prefabs, transform);
        tile.transform.position = Vector3.zero;

        var find = tile.GetComponent<DrawTile>();
        if(find != null)
        {
            find.Draw();
            tiles.Add(find);
            tileTable.Add(find.transform.position , find);
            SetAroundTile(find);
        }

    }

    private void Update()
    {
        if(TouchManager.TouchType == TouchType.Tab)
        {
            Ray ray = Camera.main.ScreenPointToRay(TouchManager.GetDragPos());
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red , 100);
            if(Physics.Raycast(ray , out RaycastHit hit , Mathf.Infinity , mask))
            {
                var find = hit.collider.GetComponent<DrawTile>();

                if(find != null && !find.IsDraw)
                {
                    find.Draw();
                    SetAroundTile(find);
                }
            }
        }
    }

    private void SetAroundTile(DrawTile tile)
    {
        for(int i = 0; i < neighborPosition.nextNeighborPos.Length; i++)
        {
            Vector3 newPosition = NeighborPosition.GetFloor(neighborPosition.nextNeighborPos[i] + tile.transform.position);
            if(!tileTable.ContainsKey(newPosition))
            {
                var newTile = Instantiate(prefabs, transform).GetComponent<DrawTile>();
                newTile.transform.position = newPosition;

                if (newTile != null)
                {
                    tileTable.Add(newTile.transform.position, newTile);
                }
            }
        }
    }
}
