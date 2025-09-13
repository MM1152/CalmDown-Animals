using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DragCamera : MonoBehaviour
{
    public float dragSpeed = 2f;

    public Camera cam;
    private Vector3 dragOrigin;


    [Header("Game View Used")]
    public float distance;
    public float checkTime;
    private float timer;
    private float amount;
    private Vector3 dir;
    private bool isDrag;
    private void Update()
    {
        Vector3 linear = Vector3.zero;

#if UNITY_EDITOR
        //if (Input.GetMouseButton(0))
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        dragOrigin = Input.mousePosition;
        //        timer = 0;
        //        amount = 0;
        //        dir = Vector3.zero;
        //        isDrag = false;
        //    }

        //    timer += Time.deltaTime;

        //    if((amount > distance && timer < checkTime) || isDrag)
        //    {
        //        dir = new Vector3(dragOrigin.x - Input.mousePosition.x, 0f, dragOrigin.y - Input.mousePosition.y).normalized;
        //        linear = -dir;
        //        isDrag = true;
        //    }
        //    else
        //    {
        //        amount += (dragOrigin - Input.mousePosition).magnitude;
        //    }
        //}
#elif UNITY_ANDROID || UNITY_IOS
        if (TouchManager.TouchType == TouchType.Swipe)
        {
            linear = TouchManager.GetSwipeDir();
        }
#endif
        if (TouchManager.TouchType == TouchType.Swipe)
        {
            linear = TouchManager.GetSwipeDir();
        }
        cam.transform.position += -(linear * dragSpeed * Time.deltaTime);
    }
}
