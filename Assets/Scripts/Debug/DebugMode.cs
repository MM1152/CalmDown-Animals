using UnityEngine;
using UnityEngine.UI;
public class DebugMode : MonoBehaviour
{
#if DEBUG
    public Button setPrevWaveButton;
    public Button setNextWaveButton;

    public Button restartRound;
    public Button clearRound;
    public Button speedupButton;

    private bool speedup;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindWithTag(TagIds.GameManagerTag).GetComponent<GameManager>();

        setPrevWaveButton.onClick.AddListener(() =>
        {
            if (DataTableManager.roundTable.Get(gameManager.Wave).Map_Size != DataTableManager.roundTable.Get(gameManager.Wave - 1).Map_Size) return;
            gameManager.Wave--;
        });

        restartRound.onClick.AddListener(() =>
        {
            gameManager.EndWave(true);
        });

        clearRound.onClick.AddListener(() =>
        {
            gameManager.EndWave();
        });

        setNextWaveButton.onClick.AddListener(() =>
        {
            gameManager.EndWave();
        });

        speedupButton.onClick.AddListener(() =>
        {
            speedup = !speedup;
            if(speedup)
            {
                Time.timeScale = 5f;
            }else
            {
                Time.timeScale = 1f;
            }
        });
    }
#endif
}
