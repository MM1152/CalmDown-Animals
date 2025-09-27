using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class DeleteTileTutorial : Tutorial
{
    private readonly int[] stringIdxs = new int[] { 11,12,13 };
    private int curIdx = 0;
    private bool checkClearTile;

    private UnityAction action1;
    
    public DeleteTileTutorial(TutorialManager manager) : base(manager) { }

    public override void Clear()
    {
        manager.deleteTileButton.onClick.RemoveListener(action1);
    }
    public override void Play()
    {
        manager.DisAbleAllButton();
        manager.deleteTileButton.interactable = true;
        manager.arrowImage.SetActive(false);

        manager.Open();
        manager.SetText(stringIdxs[curIdx]);
        manager.followFingerImage.SetActive(false);

        action1 = () => {
            manager.SetStartCoroutine(FingerMoveCo());
            manager.deleteTileButton.interactable = false;
            checkClearTile = true;
        };

        manager.deleteTileButton.onClick.AddListener(action1);
    }

    public override void Update()
    {
        if(curIdx < stringIdxs.Length)
        {
            curIdx++;
            if(curIdx >= stringIdxs.Length)
            {
                manager.Close();
                ChangeFollowFingerPosition(manager.deleteTileButton.transform.position);
                return;
            }
            manager.SetText(stringIdxs[curIdx]);
        }else if(checkClearTile)
        {
            if(manager.tileManager.CheckClearAllTileInTutorial())
            {
                checkClearTile = false;
                manager.SetStopCoroutine();
                manager.SetNexttutorial();
            }
        }
    }

    private IEnumerator FingerMoveCo()
    {
        Vector3 startPos = Camera.main.WorldToScreenPoint(manager.tileManager.startTile[0].transform.position);
        Vector3 endPos = Camera.main.WorldToScreenPoint(manager.tileManager.arriveTile.transform.position);

        for (float i = 0; i <= 1f; i += Time.deltaTime)
        {
            Vector3 newPos = Vector3.Lerp(startPos, endPos, i);
            manager.followFingerImage.transform.position = newPos + Vector3.up * 100f;
            yield return null;
        }

        manager.SetStartCoroutine(FingerMoveCo());
    }
}
