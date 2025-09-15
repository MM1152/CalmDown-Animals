using System;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour{
    public int wave = 1;
    private float timer = 0;
    private int timerToInt = 0;
    
    public WindowManager windowManager;

    public event Action startWave;
    public event Action endWave;

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI timerText;

    public bool WaveStart { get; private set; } = false;

    private void Awake()
    {
        waveText.text = wave + " 웨이브";
    }

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
        wave++;
        waveText.text = wave + " 웨이브";
    }

    private void Update()
    {
        if(WaveStart)
        {
            timer += Time.deltaTime;
            if(timerToInt != (int)timer)
            {
                timerToInt = (int)timer;
                timerText.text = timerToInt + " 초";
            }
        }
    }
}
