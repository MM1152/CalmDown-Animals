using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class DragCamera : MonoBehaviour
{
    public float dragSpeed = 2f;

    public Camera cam;
    private Vector3 dragOrigin;
    public bool isPlayEditor;

    [Header("Game View Used")]
    public float distance;
    public float checkTime;
    public float dragTime;

    private float dragTimer;
    private float timer;
    private float amount;
    private Vector3 dir;
    private bool isDrag;

    private Vector3 prevPos;

    private void LateUpdate()
    {
        Vector3 linear = Vector3.zero;
        if (!DragAble.CameraDrag) return;

        dragSpeed = Camera.main.orthographicSize.Normalization(2, 10).ReverseNormalization(10, 30);
        
#if UNITY_EDITOR
        if (isPlayEditor)
        {
            if (Input.GetMouseButton(0))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    dragOrigin = Input.mousePosition;
                    timer = 0;
                    amount = 0;
                    dir = Vector3.zero;
                    isDrag = false;
                }

                timer += Time.deltaTime;

                if ((amount > distance && timer < checkTime) || isDrag)
                {
                    dir = new Vector3(dragOrigin.x - Input.mousePosition.x, 0f, dragOrigin.y - Input.mousePosition.y).normalized;
                    linear = -dir;
                    isDrag = true;
                }
                else
                {
                    amount += (dragOrigin - Input.mousePosition).magnitude;
                }
            }
        }
        else
        {
            //Debug.Log(Vector3.Distance(TouchManager.GetStartPositionInWorld(), Camera.main.transform.position));
            if (TouchManager.TouchType == TouchType.Drag)
            {
                if (prevPos == Vector3.zero)
                {
                    prevPos = TouchManager.GetDragWorldPosition();
                }
                else
                {
                    Vector3 nPos = prevPos - TouchManager.GetDragWorldPosition();

                    Debug.Log(nPos);
                    Camera.main.transform.position += nPos * dragSpeed * Time.deltaTime;
                }
            }else
            {
                prevPos = Vector3.zero;
            }
        }
#elif UNITY_ANDROID || UNITY_IOS
            if (TouchManager.TouchType == TouchType.Drag)
            {
                if (prevPos == Vector3.zero)
                {
                    prevPos = TouchManager.GetDragWorldPosition();
                }
                else
                {
                    Vector3 nPos = prevPos - TouchManager.GetDragWorldPosition();

                    Debug.Log(nPos);
                    Camera.main.transform.position += nPos * dragSpeed * Time.deltaTime;
                }
            }else
            {
                prevPos = Vector3.zero;
            }
#endif

        cam.transform.position += -(linear * dragSpeed * Time.deltaTime);
    }
}
