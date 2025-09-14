using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int wave = 1;
    private float timer = 0;
        
    public WindowManager windowManager;

    public event Action startWave;
    public event Action endWave;

    public bool WaveStart { get; private set; } = false;
           
    public void StartWave()
    {
        WaveStart = true;
        windowManager.CloseAll();
        startWave?.Invoke();
    }

    public void EndWave()
    {
        WaveStart = false;
        windowManager.Open(Window.EditorWindow);
        endWave?.Invoke();
    }

    private void Update()
    {
        if(WaveStart)
        {
            timer += Time.deltaTime;
        }
    }
}
