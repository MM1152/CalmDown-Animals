using UnityEngine;
using UnityEngine.UI;

public class TileUndoButton : MonoBehaviour
{
    [Header("Colors")]
    public Sprite disAbleButton;
    public Sprite onEnableButton;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void UpdateButton(bool isActive)
    {
        if(isActive)
        {
            image.sprite = onEnableButton;
        }else
        {
            image.sprite = disAbleButton;
        }
    }
}
