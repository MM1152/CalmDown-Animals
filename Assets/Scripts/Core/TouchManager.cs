using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngineInternal;
public enum TouchType
{
    None,
    Tab,
    ZoomIn,
    ZoomOut,
    Drag,
}

public enum Phase
{
    None,
    Begin,
    Up,
}

public class TouchManager : MonoBehaviour
{
    public static Phase Phase { get; private set; }
    private Phase prevPhase;
    private static TouchType TouchType {get; set;}
    public static TouchType touchType;
    private static Vector2 fingerTouchStartPosition;
    private float fingerTouchStartTime;

    public float checkTime = 0.5f;
    public float ckeckDragDistance;

    private float amount;
    private static Vector2 dir;
    private static Vector3 pos;

    private float zoomInDistance;

    private int touchId1;
    private int touchId2;

    private static bool touchInUi;

    private Queue<TouchType> touchQueue = new Queue<TouchType>();

    private static bool TouchPositionInUi(Touch touch)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = touch.position;
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);

        return result.Count > 0;
    }

    public static List<RaycastResult> GetTouchPositionUI(Vector3 pos)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = pos;
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);
  
        return result;
    }

    public void Update()
    {
        if (Input.touchCount == 0)
        {
            if(prevPhase != Phase.None)
            {
                Phase = Phase.Up;
                prevPhase = Phase.None;
            }
            else
            {
                Phase = Phase.None;
            }
            touchType = TouchType.None;
            fingerTouchStartPosition = Vector3.zero;
            zoomInDistance = 0;

            touchId1 = -1;
            touchId2 = -1;
            touchInUi = false;
        }
        else if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if(Phase == Phase.None)
            {
                touchInUi = TouchPositionInUi(touch);
                Phase = Phase.Begin;
            }

            if (touch.phase == TouchPhase.Began)
            {
                amount = 0;
                dir = Vector2.zero;
                pos = Vector2.zero;
                fingerTouchStartTime = Time.time;
                fingerTouchStartPosition = touch.position;
                touchType = TouchType.None;
            }

            if(Time.time > (fingerTouchStartTime + checkTime) && amount > ckeckDragDistance)
            {
                touchType = TouchType.Drag;
            }
            else if (touchType != TouchType.Drag && touch.phase == TouchPhase.Ended)
            {
                touchType = TouchType.Tab;
            }
            else
            {
                amount += touch.deltaPosition.magnitude;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                Phase = Phase.Up;
            }
            prevPhase = Phase;
            dir = (touch.position - fingerTouchStartPosition).normalized;
            pos = new Vector3(touch.position.x, touch.position.y , 10);
        }
        else if (Input.touchCount == 2 && touchType == TouchType.None)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if(touchId1 == -1)
                touchId1 = touch1.fingerId;
            if (touchId2 == -1)
                touchId2 = touch2.fingerId;

            if(touchId1 != -1 && touchId2 != -1)
            {
                touchInUi = TouchPositionInUi(touch1);
                if (!touchInUi) touchInUi = TouchPositionInUi(touch2);
                if (touch1.phase == TouchPhase.Began && touch2.phase == TouchPhase.Began)
                {
                    amount = 0;
                    dir = Vector2.zero;
                    pos = Vector2.zero;
                    fingerTouchStartTime = Time.time;
                    fingerTouchStartPosition = Vector3.Lerp(touch1.position, touch2.position, 0.5f);
                    touchType = TouchType.None;
                }

                if (zoomInDistance == 0)
                {
                    zoomInDistance = Vector3.Distance(touch1.position - touch1.deltaPosition, touch2.position - touch2.deltaPosition);
                }
                else
                {
                    float distance = Vector3.Distance(touch1.position - touch1.deltaPosition, touch2.position - touch2.deltaPosition);
                    if (zoomInDistance > distance)
                    {
                        touchType = TouchType.ZoomOut;
                    }
                    if (zoomInDistance < distance)
                    {
                        touchType = TouchType.ZoomIn;
                    }
                }
            }  
        }
        touchQueue.Enqueue(touchType);
    }

    private void LateUpdate()
    {
        if(touchQueue.Count > 0)
        {
            touchType = touchQueue.Dequeue();
        }
    }

    public static Vector3 GetSwipeDir()
    {
        return new Vector3(dir.x , 0 , dir.y);
    }
    public static Vector3 GetDragPos()
    {
        return pos;
    }

    //Y 축 카메라 포지션 Y 축임
    public static Vector3 GetDragWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(pos);
    }

    public static Vector3 GetStartPosition()
    {
        return fingerTouchStartPosition;
    }

    public static Vector3 GetStartPositionInWorld()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(fingerTouchStartPosition.x , fingerTouchStartPosition.y , Camera.main.transform.position.y));
    }

    public static bool TouchStartInUI()
    {
        return touchInUi;
    }
}
