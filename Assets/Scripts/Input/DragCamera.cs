using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DragCamera : MonoBehaviour
{
    public float dragSpeed = 2f;

    public Camera cam;
    private Vector2 dragOrigin;
    private void Update()
    {
        Vector3 linear = Vector3.zero;

        //if (Input.GetMouseButton(0))
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        dragOrigin = Input.mousePosition;
        //    }

        //    linear = new Vector3(dragOrigin.x - Input.mousePosition.x , 0f, dragOrigin.y - Input.mousePosition.y);
        //}

        if (TouchManager.TouchType == TouchType.Swipe)
        {
            linear = TouchManager.GetSwipeDir();
        }


        cam.transform.position += -(linear * dragSpeed * Time.deltaTime);
    }
}
