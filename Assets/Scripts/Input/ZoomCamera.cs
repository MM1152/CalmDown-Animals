using System.Drawing;
using UnityEngine;

public class ZoomCamera : MonoBehaviour
{
    public float minSize;
    public float maxSize;

    public float zoomSpeed;
    public float zoomLimitTime;

    public float speedToZoomZone;

    private Vector3 zoomInArea;
    private void Update()
    {
        float size = 0;
        if (TouchManager.TouchType == TouchType.ZoomOut)
        {
            size = Camera.main.orthographicSize * zoomSpeed * Time.deltaTime;
        }
        else if(TouchManager.TouchType == TouchType.ZoomIn)
        {
            size = Camera.main.orthographicSize * -zoomSpeed * Time.deltaTime;
        }

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + size, minSize, maxSize);

        if (TouchManager.TouchType == TouchType.ZoomIn 
            && size != 0 && Camera.main.orthographicSize > minSize 
            && Camera.main.orthographicSize < maxSize)
        {
            if(zoomInArea == Vector3.zero)
            {
                var viewPort = Camera.main.ScreenToViewportPoint(TouchManager.GetStartPosition());
                Debug.Log(viewPort);
                zoomInArea = new Vector3(viewPort.x - 0.5f, 0f , viewPort.y - 0.5f);
            }
            Vector3 cameraNewPos = zoomInArea * speedToZoomZone * Time.deltaTime;
            Camera.main.transform.position += cameraNewPos;
        }else
        {   
            zoomInArea = Vector3.zero;
        }
    }
}
