using UnityEngine;

public class GenericPopup : MonoBehaviour
{
    protected PopupManager manager;

    public void Init(PopupManager manager)
    {
        this.manager = manager; 
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}