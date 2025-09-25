using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimalImage : MonoBehaviour , IPointerClickHandler
{
    public Image image;
    private AnimalInfoTable.Data data;
    private AnimalDes des;
    public void OnPointerClick(PointerEventData eventData)
    {
        des.UpdateDescription(data);
    }

    public void UpdateSlot(AnimalInfoTable.Data data , AnimalDes des)
    {
        this.des = des;
        this.data = data;
        this.image.sprite = data.Icon;
    }
}
