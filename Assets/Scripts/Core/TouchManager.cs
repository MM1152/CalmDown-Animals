using Unity.VisualScripting;
using UnityEngine;

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
    public static TouchType TouchType {get; private set;}
    public TouchType touchType;
    private static Vector2 fingerTouchStartPosition;
    private float fingerTouchStartTime;

    public float checkTime = 0.5f;
    public float amount;

    private static Vector2 dir;
    private static Vector3 pos;

    private bool touchFinish;

    private float zoomInDistance;
    
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
            TouchType = TouchType.None;
            touchFinish = true;
            fingerTouchStartPosition = Vector3.zero;
            zoomInDistance = 0;
        }
        else if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if(Phase == Phase.None)
            {
                Phase = Phase.Begin;
            }

            if (touch.phase == TouchPhase.Began)
            {
                amount = 0;
                dir = Vector2.zero;
                pos = Vector2.zero;
                fingerTouchStartTime = Time.time;
                fingerTouchStartPosition = touch.position;
                TouchType = TouchType.None;
            }

            if(Time.time > (fingerTouchStartTime + checkTime) && amount > 5)
            {
                TouchType = TouchType.Drag;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                TouchType = TouchType.Tab;
            }
            else
            {
                amount += touch.deltaPosition.magnitude;
            }

            if(touch.phase == TouchPhase.Ended)
            {
                Phase = Phase.Up;
            }
            prevPhase = Phase;
            dir = (touch.position - fingerTouchStartPosition).normalized;
            pos = new Vector3(touch.position.x, touch.position.y , 10);
        }
        else if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began && touch2.phase == TouchPhase.Began)
            {
                amount = 0;
                dir = Vector2.zero;
                pos = Vector2.zero;
                fingerTouchStartTime = Time.time;
                fingerTouchStartPosition = Vector3.Lerp(touch1.position , touch2.position , 0.5f);
                TouchType = TouchType.None;
            }

            if (zoomInDistance == 0)
            {
                zoomInDistance = Vector3.Distance(touch1.position - touch1.deltaPosition, touch2.position - touch2.deltaPosition);
            }else
            {
                float distance = Vector3.Distance(touch1.position - touch1.deltaPosition, touch2.position - touch2.deltaPosition); 
                if(zoomInDistance > distance)
                {
                    TouchType = TouchType.ZoomOut;
                }
                if (zoomInDistance < distance)
                {
                    TouchType = TouchType.ZoomIn;
                }
            }

        }
        touchType = TouchType;
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
}
