using System;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour{

    private int wave = 1;
    public int Wave
    {
        get => wave;
        set
        {
            if (value <= 0 || value > maxWave) return;
            wave = value;
            waveText.text = wave + "Round";
        }
    }
    private int gold = 200;
    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
            goldText.text = gold.ToString();
        }
    }

    private float timer = 0;
    private int timerToInt = 0;
    private bool GameFin { get; set; }

    public WindowManager windowManager;
    public PopupManager popupManager;

    public event Action startWave;
    public event Action endWave;

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI paymentText;

    public int maxWave;
    public bool WaveStart { get; private set; } = false;

    public int allCountSpawnAnimals;
    private void Awake()
    {
        allCountSpawnAnimals = 0;
        waveText.text = wave + " Round";
        goldText.text = gold.ToString();
    }

    public void StartWave()
    {
        WaveStart = true;
        windowManager.Open(Window.DuringGameWindow);
        startWave?.Invoke();
    }

    public void EndWave(bool waveFail = false)
    {
        WaveStart = false;
        windowManager.Open(Window.EditorWindow);
        Gold += DataTableManager.roundTable.Get(wave).RewardGold;
        // ���̺� ���� ���� �ؽ�Ʈ ���. ��� �ؾ߉�
        endWave?.Invoke();

        if (wave == maxWave && !waveFail)
        {
            GameFin = true;
            var popup = (StringPopUp)popupManager.Open(Popup.TextPopUp);
            popup.Id = 1;
            return;
        }
        else if(waveFail)
        {
            var popup = (StringPopUp)popupManager.Open(Popup.TextPopUp);
            popup.Id = 2;
        }
        else
        {
            wave++;
        }

        waveText.text = wave + " ���̺�";
    }

    private void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.N))
        {
            EndWave();
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
        {
            WaveFail();
        }
#endif
        if (WaveStart)
        {
            timer += Time.deltaTime;
            if(timerToInt != (int)timer)
            {
                timerToInt = (int)timer;
                timerText.text = timerToInt + " ��";
            }
        }
    }

    public void WaveFail()
    {
        EndWave(true);
    }
}
