using UnityEngine;

public enum TouchType
{
    None,
    Tab,
    Swipe,
    Zoom,
    Draw,
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
    private bool touchFinish;

    public void LateUpdate()
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
                fingerTouchStartTime = Time.time;
                fingerTouchStartPosition = touch.position;
                TouchType = TouchType.None;
            }

            if(Time.time > fingerTouchStartTime + checkTime)
            {
                if (amount > swipeAmount)
                {
                    TouchType = TouchType.Swipe;
                }
                else if (amount < swipeAmount)
                {
                    TouchType = TouchType.Draw;
                }
                
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                TouchType = TouchType.Tab;
            }
            else
            {
                amount += touch.deltaPosition.magnitude;
                dir = (touch.position - fingerTouchStartPosition).normalized;
            }
        }
        touchType = TouchType;
    }

    public static Vector3 GetSwipeDir()
    {
        return new Vector3(dir.x , 0 , dir.y);
    }
}
