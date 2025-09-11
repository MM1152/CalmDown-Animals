using JetBrains.Annotations;
using System;
using UnityEngine;

public class Crew : MonoBehaviour
{
    public LayerMask mask;
    
    public float attackRadius;
    public float attackInterval;

    private bool isSpawn;
    private bool dragAble;

    public bool DragAble => dragAble;

    public void Spawn()
    {
        dragAble = true;
        isSpawn = false;
    }

    private void Update()
    {
        DragDrop();
    }

    private void DragDrop()
    {
        if (dragAble)
        {
            Vector3 touchPosition = Vector3.zero;

            if (TouchManager.TouchType == TouchType.Drag)
            {
                touchPosition = TouchManager.GetDragWorldPosition();
                transform.position = new Vector3(touchPosition.x, 1, touchPosition.z);
                Debug.Log(touchPosition);
                isSpawn = true;
            }

            else if (TouchManager.TouchType == TouchType.None && isSpawn)
            {
                dragAble = false;

                Ray ray = Camera.main.ScreenPointToRay(TouchManager.GetDragPos());
                Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red , 100f);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask))
                {
                    var find = hit.collider.GetComponent<PathTile>();
                    if (find != null)
                    {
                        if (find.Type == TileType.None)
                        {
                            find.Type = TileType.Crew;
                            transform.position = find.transform.position + Vector3.up * 0.5f;
                        }
                        else
                        {
                            Destroy(gameObject);
                        }
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
