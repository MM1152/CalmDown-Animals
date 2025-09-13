using UnityEngine;

public enum TouchType
{
    None,
    Tab,
    Swipe,
    Zoom,
    Drag,
}

public class TouchManager : MonoBehaviour
{
    public TouchType touchType;
    public static TouchType TouchType {get; private set;}

    private Vector2 fingerTouchStartPosition;
    private float fingerTouchStartTime;

    public float checkTime = 0.5f;
    public float swipeAmount = 5f;
    public float amount;

    private static Vector2 dir;
    private static Vector3 pos;

    private bool touchFinish;

    public void Update()
    {
        if (Input.touchCount == 0)
        {
            TouchType = TouchType.None;
            touchFinish = true;
        }
        else if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
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
                if (amount > swipeAmount)
                {
                    TouchType = TouchType.Swipe;
                }
                else if (amount < swipeAmount)
                {
                    TouchType = TouchType.Drag;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                TouchType = TouchType.Tab;
            }
            else
            {
                amount += touch.deltaPosition.magnitude;
            }
            dir = (touch.position - fingerTouchStartPosition).normalized;
            pos = new Vector3(touch.position.x, touch.position.y , 10);
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
}
