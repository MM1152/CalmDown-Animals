using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScorePopup : GenericPopup
{
    [Header("References")]
    public GameManager gameManager;

    [Header("Texts")]
    public TextMeshProUGUI titleText;

    public TextMeshProUGUI clearGameScoreText;

    public TextMeshProUGUI roundText;
    public TextMeshProUGUI roundScoreText;
    
    public TextMeshProUGUI clearTimeText;
    public TextMeshProUGUI clearTimeScoreText;
    
    public TextMeshProUGUI captureAnimalCountText;
    public TextMeshProUGUI captureAnimalCountScoreText;

    public TextMeshProUGUI holdGoldText;
    public TextMeshProUGUI holdGoldScoreText;

    public TextMeshProUGUI finalScoreText;

    public GameObject[] uguis;

    private int totalScore = 0;
    private Coroutine co;

    private int startIdx;

    public override void Open()
    {
        foreach(var gui in uguis)
        {
            gui.SetActive(false);
        }

        startIdx = 0;
        totalScore = 0;
        string clear;
        if(gameManager.WaveClear)
        {
            titleText.color = Color.red;
            clear = DataTableManager.stringTable.Get(4);
        }else
        {
            titleText.color = Color.black;
            clear = DataTableManager.stringTable.Get(5);
        }
        titleText.text = clear;

        clearGameScoreText.text = gameManager.WaveClear ? "+ 1000" : "0";
        totalScore += gameManager.WaveClear ? 1000 : 0;

        roundText.text = gameManager.Wave.ToString();
        roundScoreText.text = "+ " + (gameManager.Wave * 1000).ToString();
        totalScore += (gameManager.Wave * 1000);

        clearTimeText.text = gameManager.timerText.text;
        clearTimeScoreText.color = Color.red;
        clearTimeScoreText.text = "- " + (gameManager.timerToInt * 10).ToString();
        totalScore -= (gameManager.timerToInt * 10);

        captureAnimalCountText.text = gameManager.captureAnimalCount.ToString() + " ¸¶¸®";
        captureAnimalCountScoreText.text = "+ " + (gameManager.captureAnimalCount * 100);
        totalScore += (gameManager.captureAnimalCount * 100);

        holdGoldText.text = gameManager.Gold.ToString() + " °ñµå";
        holdGoldScoreText.text = "+ " + gameManager.Gold.ToString();
        totalScore += gameManager.Gold;

        finalScoreText.text = totalScore.ToString();
        base.Open();
        co = StartCoroutine(ShowClearInfomationCo());
    }

    public override bool Close()
    {
        if (co == null) { 
            base.Close();
            return true;
        }
        else
        {
            if(co != null)
            {
                if(startIdx < uguis.Length)
                {
                    uguis[startIdx].SetActive(true);
                    startIdx++;
                }
                return false;
            }

            co = StartCoroutine(ShowClearInfomationCo());

            return false;
        }
        
    }

    private IEnumerator ShowClearInfomationCo()
    {
        for(; startIdx < uguis.Length; startIdx++)
        {
            yield return new WaitForSeconds(1f);
            uguis[startIdx].SetActive(true);
        }

        yield return new WaitUntil(() => TouchManager.touchType == TouchType.Tab);

        co = null;
        SceneManager.LoadScene("TitleScene");
    }
}