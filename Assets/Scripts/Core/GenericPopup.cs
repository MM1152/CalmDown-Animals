using UnityEngine;

public class GenericPopup : MonoBehaviour
{
    protected PopupManager manager;

    public virtual void Init(PopupManager manager)
    {
        this.manager = manager; 
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual bool Close()
    {
        gameObject.SetActive(false);
        return true;
    }
}