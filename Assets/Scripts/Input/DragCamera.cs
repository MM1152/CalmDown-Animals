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
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = Input.mousePosition;
            }

            linear = new Vector3(dragOrigin.x - Input.mousePosition.x , 0f, dragOrigin.y - Input.mousePosition.y);
        }
#elif UNITY_ANDROID || UNITY_IOS
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                dragOrigin = touch.position;
            }
            else if(touch.phase == TouchPhase.Moved) 
            {
                linear = new Vector3(dragOrigin.x  - touch.position.x , 0f , dragOrigin.y - touch.position.y );
            }
        }
#endif
        cam.transform.position += linear.normalized * dragSpeed * Time.deltaTime;
    }
}
