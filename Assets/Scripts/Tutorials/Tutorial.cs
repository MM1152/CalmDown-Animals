using UnityEngine;


public enum Side
{
    Left,
    Top,
    Right,
    Bottom
}

public abstract class Tutorial
{
    public TutorialManager manager;
    private Vector3 upPos = Vector3.up * 200f;

    public Tutorial(TutorialManager manager)
    {
        this.manager = manager;
    }

    protected void ChangeFollowFingerPosition(GameObject target , Side side)
    {
        //LEFT 0 , 0.5 : 0 , 0.5
        //TOP 0.5 , 1 : 0.5 , 1
        //BOTTOM 0.5 , 0 : 0.5 , 0
        //RIGHT 1 , 0.5 : 1 , 0.5
        Vector2 anchorPosMin = Vector3.zero;
        Vector2 anchorPosMax = Vector2.zero;

        switch (side)
        {
            case Side.Left:
                anchorPosMin = new Vector2(0, 0.5f);
                anchorPosMax = new Vector2(0, 0.5f);
                break;
            case Side.Top:
                anchorPosMin = new Vector2(0.5f, 1f);
                anchorPosMax = new Vector2(0.5f, 1f);
                break;
            case Side.Right:
                anchorPosMin = new Vector2(1f, 0.5f);
                anchorPosMax = new Vector2(1f, 0.5f);
                break;
            case Side.Bottom:
                anchorPosMin = new Vector2(0.5f, 0f);
                anchorPosMax = new Vector2(0.5f, 0f);
                break;
        }
        manager.followFingerImage.transform.SetParent(target.transform);
        manager.followFingerImage.SetActive(true);

        manager.followFingerImage.GetComponent<RectTransform>().anchorMax = anchorPosMax;
        manager.followFingerImage.GetComponent<RectTransform>().anchorMin = anchorPosMin;
        manager.followFingerImage.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
    protected void ChangeArrowPosition(GameObject target, Side side)
    {
        //LEFT 0 , 0.5 : 0 , 0.5
        //TOP 0.5 , 1 : 0.5 , 1
        //BOTTOM 0.5 , 0 : 0.5 , 0
        //RIGHT 1 , 0.5 : 1 , 0.5
        Vector2 anchorPosMin = Vector3.zero;
        Vector2 anchorPosMax = Vector2.zero;

        switch (side)
        {
            case Side.Left:
                anchorPosMin = new Vector2(0, 0.5f);
                anchorPosMax = new Vector2(0, 0.5f);
                break;
            case Side.Top:
                anchorPosMin = new Vector2(0.5f, 1f);
                anchorPosMax = new Vector2(0.5f, 1f);
                break;
            case Side.Right:
                anchorPosMin = new Vector2(1f, 0.5f);
                anchorPosMax = new Vector2(1f, 0.5f);
                break;
            case Side.Bottom:
                anchorPosMin = new Vector2(0.5f, 0f);
                anchorPosMax = new Vector2(0.5f, 0f);
                break;
        }
        manager.arrowImage.transform.SetParent(target.transform);
        manager.arrowImage.SetActive(true);

        manager.arrowImage.GetComponent<RectTransform>().anchorMax = anchorPosMax;
        manager.arrowImage.GetComponent<RectTransform>().anchorMin = anchorPosMin;
        manager.arrowImage.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
    public abstract void Play();
    public abstract void Update();
    public abstract void Clear();
}
