using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    public Sprite pressedSprite;
    public Sprite normalSprite;

    private Button button;

    private bool isOn;
    public bool IsOn
    {
        get => isOn;
        set
        {
            isOn = value;
            if(isOn)
            {
                button.image.sprite = pressedSprite;
            }
            else
            {
                button.image.sprite = normalSprite;
            }
        }
    }

    private void Awake()
    {
        button = GetComponent<Button>();
    }
}
