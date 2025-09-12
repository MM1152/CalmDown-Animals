using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public GenericWindow[] windows;
    public Window CurrentWindow { get; private set; }
    public Window startWindow;

    public void Start()
    {
        foreach(var window in windows)
        {
            window.Init(this);
            window.Close();
        }

        CurrentWindow = startWindow;
        windows[(int)CurrentWindow].Open();
    }

    public void Open(Window id)
    {
        windows[(int)CurrentWindow].Close();
        CurrentWindow = id;
        windows[(int)CurrentWindow].Open();
    }
}