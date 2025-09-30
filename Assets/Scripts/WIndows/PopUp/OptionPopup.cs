using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionPopup : GenericPopup
{
    public Slider SFXsoundSlider;
    public Slider BGMsoundSlider;

    public Button offClouds;
    public Button offTrees;
    public Button offFPX;

    public Sprite initSprite;
    public Sprite pressedSprite;

    public GameObject layout;
    public TileManager tileManager;
    public Cloudes cloud;
    public override void Init(PopupManager manager)
    {
        SFXsoundSlider.onValueChanged.AddListener(SoundManager.Instance.ChangeSFXSound);
        BGMsoundSlider.onValueChanged.AddListener(SoundManager.Instance.ChangeBGMSound);
        offTrees.GetComponent<Image>().color = Variable.onTree ? new Color(1f, 1f, 1f) : new Color(0.3207547f, 0.3207547f, 0.3207547f);
        offFPX.GetComponent<Image>().color = Variable.onFPX ? new Color(1f, 1f, 1f) : new Color(0.3207547f, 0.3207547f, 0.3207547f);
        offClouds.GetComponent<Image>().color = Variable.onFPX ? new Color(1f, 1f, 1f) : new Color(0.3207547f, 0.3207547f, 0.3207547f);
        SFXsoundSlider.value = SoundManager.Instance.GetSFXValue();
        BGMsoundSlider.value = SoundManager.Instance.GetBGMValue();

        offTrees.onClick.AddListener(() =>
        {
            Variable.onTree = !Variable.onTree;
            if(!Variable.onTree)
            {
                offTrees.GetComponent<Image>().color = new Color(0.3207547f, 0.3207547f, 0.3207547f);
            }
            else
            {
                offTrees.GetComponent<Image>().color = new Color(1f,1f,1f);
            }
            tileManager?.ChangeVariableOnTree();
        });

        offFPX.onClick.AddListener(() =>
        {
            Variable.onFPX = !Variable.onFPX;
            if (!Variable.onFPX)
            {
                offFPX.GetComponent<Image>().color = new Color(0.3207547f, 0.3207547f, 0.3207547f);
            }
            else
            {
                offFPX.GetComponent<Image>().color = new Color(1f, 1f, 1f);
            }
        });

        offClouds.onClick.AddListener(() =>
        {
            Variable.onCloud = !Variable.onCloud;
            if (!Variable.onCloud)
            {
                offClouds.GetComponent<Image>().color = new Color(0.3207547f, 0.3207547f, 0.3207547f);
                cloud?.gameObject.SetActive(false);
            }
            else
            {
                offClouds.GetComponent<Image>().color = new Color(1f, 1f, 1f);
            }
        });
        base.Init(manager);
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
