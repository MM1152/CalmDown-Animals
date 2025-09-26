using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionPopup : GenericPopup
{
    public Slider soundSlider;

    public Button offClouds;
    public Button offTrees;
    public Button offFPX;

    public Sprite initSprite;
    public Sprite pressedSprite;

    public GameObject layout;


    public override bool Close()
    {
        if (TouchManager.TouchStartInUI())
        {
            var hit = TouchManager.GetTouchPositionUI(TouchManager.GetStartPosition());
            if (hit.Count > 0)
            {
                foreach (var obj in hit)
                {
                    if (obj.gameObject == layout)
                    {
                        return false;
                    }
                }
            }
        }

        return base.Close();
    }
}
