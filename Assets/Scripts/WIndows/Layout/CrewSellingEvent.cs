using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CrewSellingEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CrewManager spanwer;
    public TextMeshProUGUI goldText;
    public bool SellAble { get; private set; }
    public void OnEnable()
    {
        SellAble = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
#if DEV_MODE
        Debug.Log("sell able true");
#endif
        Debug.Log("Sell Able");
        SellAble = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (TouchManager.Phase == Phase.Up) return;
        Debug.Log("Sell Able False");
        SellAble = false;
        
    }
}
