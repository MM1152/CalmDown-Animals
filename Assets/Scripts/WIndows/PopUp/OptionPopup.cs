using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionPopup : GenericPopup
{
    public Toggle FPS30Button;
    public Toggle FPS60Button;

    public Slider soundSlider;

    public Button offClouds;
    public Button offTrees;
    public Button offFPX;

    public Sprite initSprite;
    public Sprite pressedSprite;

    public GameObject layout;

    public override void Init(PopupManager manager)
    {
        base.Init(manager);
        var toggleBackGround1 = FPS30Button.gameObject.GetComponentInChildren<Image>();
        var toggleBackGround2 = FPS60Button.gameObject.GetComponentInChildren<Image>();

        FPS30Button.onValueChanged.AddListener((value) =>
        {
            if(value)
            {
                toggleBackGround2.color = new Vector4(0.5f , 0.5f, 0.5f, 1f);
                SetFPS(30);
            }else
            {
                toggleBackGround2.color = Vector4.one;
            }
        });

        FPS60Button.onValueChanged.AddListener((value) =>
        {
            if (value)
            {
                toggleBackGround2.color = new Vector4(0.5f , 0.5f , 0.5f , 1f);
                SetFPS(60);
            }
            else
            {
                toggleBackGround2.color = Vector4.one;
            }
        });

        FPS60Button.isOn = true;
        EventSystem.current.firstSelectedGameObject = FPS60Button.gameObject;
    }

    public void SetFPS(int fps)
    {
        Application.targetFrameRate = fps;
    }

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
