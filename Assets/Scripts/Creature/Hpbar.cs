using UnityEngine;
using UnityEngine.UI;
public class Hpbar : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void SetValue(int hp , int maxHp)
    {
        if(slider != null)
        {
            slider.value = hp / (float)maxHp;
        }
    }
}
