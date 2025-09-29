using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Reference")]
    public TileManager tileManager;
    public EnemySpawner enemySpawner;
    public WindowManager windowManager;
    public PopupManager popupManager;
    public CrewManager crewManager;

    [Header("Texts")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI paymentText;
    public TextMeshProUGUI roundClearGoldText;

    [Header("OptionWindows")]
    public Button optionButton;
    public GameObject optionTab;

    [Header("Debug")]
    public int timerToInt = 0;
    public int maxWave;
    public int captureAnimalCount;

    private int wave = 1;
    public int Wave
    {
        get => wave;
        set
        {
            if (value <= 0 || value > maxWave) return;
            wave = value;
            waveText.text = wave + "웨이브";
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
        }
    }

    private int roundClearGold = 0;
    public int RoundClearGold
    {
        get => roundClearGold;
        set
        {
            roundClearGold = value;
            roundClearGoldText.text = "+" + roundClearGold;
        }
    }
    private float timer = 0;

    private bool GameFin { get; set; }

    public event Action startWave;
    public event Action endWave;

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
    private int currentSpawnCount;
    private int escapeCount;
    public bool WaveClear { get; set; }
    private void Awake()
    {
        AllAnimalSpawnCount = 0;
        waveText.text = wave + "웨이브";
        goldText.text = gold.ToString();
        captureAnimalCount = 0;

    }

    private void Start()
    {
        SoundManager.Instance.PlayBackGround(BGM.InGameSoundOneTime);
        if (SaveLoadManager.Data.canLoadSaveData)
        {
            Gold = SaveLoadManager.Data.gold;
            Wave = SaveLoadManager.Data.wave;
            timer = SaveLoadManager.Data.time;
            timerToInt = SaveLoadManager.Data.time;
            tileManager.DrawTiles(SaveLoadManager.Data.mapSize);
        }
        else
        {
            tileManager.DataLoadFail();
        }
        optionButton.onClick.AddListener(() =>
        {
            popupManager.Open(Popup.OptionTabPopup);
        });
        RoundClearGold = DataTableManager.roundTable.Get(Wave).RewardGold;
    }


    public void StartWave()
    {
        if(!tileManager.FindPathAndDrawRoads())
        {
            var popup = popupManager.Open(Popup.TextPopUp) as StringPopUp;
            popup.Id = 0;
            return;
        }
        if(crewManager.GetPlaceCrewCount() == 0)
        {
            var popup = popupManager.Open(Popup.TextPopUp) as StringPopUp;
            popup.Id = 6;
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
        WaveClear = !waveFail;
        // 웨이브 증가 이후 텍스트 찍기. 고려 해야됌
        if (wave == maxWave && !waveFail)
        {
            popupManager.Open(Popup.ScorePopUp);
            enemySpawner.ClearAllAnimals();
            SaveLoadManager.Data.canLoadSaveData = false;
            SaveLoadManager.Save();
            return;
        }
        else if (waveFail)
        {
            enemySpawner.ClearAllAnimals();
            popupManager.Open(Popup.ScorePopUp);
            SaveLoadManager.Data.canLoadSaveData = false;
            SaveLoadManager.Save();
            return;
        }
        else
        {
            wave++;
        }   
        endWave?.Invoke();

        crewManager.UpdateCrewStatus();

        SaveLoadManager.Data.gold = gold;
        SaveLoadManager.Data.wave = wave;
        SaveLoadManager.Data.mapid = tileManager.mapIdx;
        SaveLoadManager.Data.mapSize = tileManager.mapSize;
        SaveLoadManager.Data.time = timerToInt;
        SaveLoadManager.Data.canLoadSaveData = true;
        SaveLoadManager.Save();

        RoundClearGold = DataTableManager.roundTable.Get(Wave).RewardGold;
        escapeCount = 0;

        waveText.text = wave + " 웨이브";
    }

    private void Update()
    {
#if UNITY_EDITOR


        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.N))
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
