using System;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TileManager tileManager;
    public EnemySpawner enemySpawner;
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

    private int payment = 0;
    public int Payment
    {
        get => payment;
        set
        {
            payment = value;
            paymentText.text = $"-{payment}";

            ChangeRoundClearGold(roundClearGold - payment);
        }
    }

    private int roundClearGold = 0;
    public int RoundClearGold
    {
        get => roundClearGold;
        set
        {
            roundClearGold = value;
            ChangeRoundClearGold(roundClearGold - payment);
        }
    }

    private float timer = 0;
    public int timerToInt = 0;
    private bool GameFin { get; set; }

    public WindowManager windowManager;
    public PopupManager popupManager;

    public event Action startWave;
    public event Action endWave;

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI paymentText;
    public TextMeshProUGUI roundClearGoldText;
    public int maxWave;
    public bool WaveStart { get; private set; } = false;

    public int AllAnimalSpawnCount
    {
        get => allAnimalSpawnCount;
        set 
        {
            currentSpawnCount = value - allAnimalSpawnCount;
            allAnimalSpawnCount = value;
        }
    }
    private int allAnimalSpawnCount;
    public int captureAnimalCount ;
    private int currentSpawnCount;
    private int escapeCount;
    public bool WaveClear { get; set; }
    private void Awake()
    {
        AllAnimalSpawnCount = 0;
        waveText.text = wave + " Round";
        goldText.text = gold.ToString();
        captureAnimalCount = 0;

    }

    private void Start()
    {
        if (SaveLoadManager.Load())
        {
            Gold = SaveLoadManager.Data.gold;
            Wave = SaveLoadManager.Data.wave;
            tileManager.DrawTiles(SaveLoadManager.Data.mapSize);
        }
        else
        {
            tileManager.DataLoadFail();
        }

        RoundClearGold = DataTableManager.roundTable.Get(Wave).RewardGold;
    }

    private void ChangeRoundClearGold(int gold)
    {
        if(gold < 0)
        {
            roundClearGoldText.text = gold.ToString();
            roundClearGoldText.color = Color.red;
        }
        else
        {
            roundClearGoldText.text = "+" + gold.ToString();
            roundClearGoldText.color = Color.green;
        }
    }

    public void StartWave()
    {
        if(!tileManager.FindPath())
        {
            var popup = (StringPopUp)popupManager.Open(Popup.TextPopUp);
            popup.Id = 4;
            return;
        }

        WaveStart = true;
        windowManager.Open(Window.DuringGameWindow);
        startWave?.Invoke();
    }

    public void EndWave(bool waveFail = false)
    {
        WaveStart = false;
        windowManager.Open(Window.EditorWindow);
        Gold += RoundClearGold - Payment;
        if(Gold < 0)
        {
            waveFail = true;
        }
        // 웨이브 증가 이후 텍스트 찍기. 고려 해야됌
        endWave?.Invoke();
        WaveClear = !waveFail;
        if (wave == maxWave && !waveFail)
        {
            popupManager.Open(Popup.ScorePopUp);
            return;
        }
        else if(waveFail)
        {
            popupManager.Open(Popup.ScorePopUp);
            return;
        }
        else
        {
            wave++;
        }


        SaveLoadManager.Data.gold = gold;
        SaveLoadManager.Data.wave = wave;
        SaveLoadManager.Data.mapid = tileManager.mapIdx;
        SaveLoadManager.Data.mapSize = tileManager.mapSize;

        RoundClearGold = DataTableManager.roundTable.Get(Wave).RewardGold;
        escapeCount = 0;

        waveText.text = wave + " 웨이브";
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
            EscapeAnimals();
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            SaveLoadManager.Save();
        }
#endif
        if (WaveStart)
        {
            timer += Time.deltaTime;
            if(timerToInt != (int)timer)
            {
                timerToInt = (int)timer;
                if(timerToInt % 60 < 10)
                {
                    timerText.text = $"{timerToInt / 60}:0{timerToInt % 60}";
                }else
                {
                    timerText.text = $"{timerToInt / 60}:{timerToInt % 60}";
                }
            }
        }
    }

    public void EscapeAnimals()
    {
        escapeCount++;
        if(currentSpawnCount * 0.1f < escapeCount)
        {
            EndWave(true);
        }
    }
}
