using System.Drawing;
using UnityEngine;

public class ZoomCamera : MonoBehaviour
{
    public float minSize;
    public float maxSize;

    public float zoomSpeed;
    public float zoomLimitTime;

    public float speedToZoomZone;
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

        if (size != 0 && Camera.main.orthographicSize > minSize && Camera.main.orthographicSize < maxSize)
        {
            Debug.Log(TouchManager.GetStartPositionInWorld());
            Vector3 cameraNewPos = Vector3.Scale(new Vector3(1f, 1f, 0f), TouchManager.GetStartPositionInWorld()) * speedToZoomZone * Time.deltaTime;
            Camera.main.transform.position += cameraNewPos;
        }
    }
}
