using UnityEngine;

public class GenericWindow : MonoBehaviour
{
    protected WindowManager manager;
    
    public void Init(WindowManager manager)
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