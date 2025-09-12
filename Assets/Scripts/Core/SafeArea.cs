//#define DEBUG

using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    public bool isSimulated;
    private RectTransform safeAreaRect;
    private Canvas canvas;
    private Rect lastSafeArea;

    void Start()
    {
        safeAreaRect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        OnRectTransformDimensionsChange();
    }

    private void OnRectTransformDimensionsChange()
    {

        if (GetSafeArea() != lastSafeArea && canvas != null)
        {
            lastSafeArea = GetSafeArea();
            UpdateSizeToSafeArea();
        }
    }

    private void UpdateSizeToSafeArea()
    {

        var safeArea = GetSafeArea();
        var inverseSize = new Vector2(1f, 1f) / canvas.pixelRect.size;
        var newAnchorMin = Vector2.Scale(safeArea.position, inverseSize);
        var newAnchorMax = Vector2.Scale(safeArea.position + safeArea.size, inverseSize);

        safeAreaRect.anchorMin = newAnchorMin;
        safeAreaRect.anchorMax = newAnchorMax;

        //safeAreaRect.offsetMin = Vector2.zero;
        //safeAreaRect.offsetMax = Vector2.zero;
    }

    private Rect GetSafeArea()
    {
        
        if (isSimulated)
        {
            return Screen.safeArea;
        }
        else
        {
            return new Rect(132, 102, 2172, 1023);
        }

#if UNITY_ANDROID || UNITY_IOS
            return Screen.safeArea;
#endif
    }
}