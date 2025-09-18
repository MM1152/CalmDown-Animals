using UnityEngine;
using UnityEngine.UI;
public class DebugMode : MonoBehaviour
{
#if DEBUG
    public Button setPrevWaveButton;
    public Button setFirstWaveButton;
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
            gameManager.Wave--;
        });
        setFirstWaveButton.onClick.AddListener(() =>
        {
            gameManager.Wave = 1;
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
                Time.timeScale = 2f;
            }else
            {
                Time.timeScale = 1f;
            }
        });
    }
#endif
}
