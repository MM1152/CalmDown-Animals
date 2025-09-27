using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DrawTileTutorial : Tutorial
{
    private readonly int[] stringIdxs = new int[] { 7, 8, 9, 10 };
    private int curIdx = 0;

    private UnityAction action1; // 기존에 람다로 넣으니까 RemoveListener 동작 X
    private UnityAction action2;
    private UnityAction action3;

    private bool checkFindPath;
    private Vector3 followPosition;

    public DrawTileTutorial(TutorialManager manager) : base(manager) { }

    public override void Play()
    {
        manager.Open();
        manager.SetText(stringIdxs[curIdx++]);
        manager.crewManager.ClearAllCrews();
        manager.DisAbleAllButton();
        manager.arrowImage.SetActive(false);
        manager.inEditWindowButton.interactable = true;
        manager.inTileDrawModeButton.interactable = true;
        manager.editTileButton.interactable = true;

        action1 = () => {
            manager.windowManager.Open(Window.TileEditorWindow);
            Canvas.ForceUpdateCanvases();
            ChangeFollowFingerPosition(manager.inTileDrawModeButton.transform.position);
        };
        action2 = () => {
            Canvas.ForceUpdateCanvases();
            ChangeFollowFingerPosition(manager.editTileButton.transform.position);
        };
        action3 = () =>
        {
            manager.tileManager.ResetInitPath();
            manager.SetStartCoroutine(FingerMoveCo());
            manager.editTileButton.interactable = false;
            checkFindPath = true;
        };

        manager.inEditWindowButton.onClick.AddListener(action1);
        manager.inTileDrawModeButton.onClick.AddListener(action2);
        manager.editTileButton.onClick.AddListener(action3);
        manager.followFingerImage.SetActive(false);

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


    public override void Update()
    {
        if(curIdx < stringIdxs.Length)
        {
            curIdx++;
            if (curIdx >= stringIdxs.Length)
            {
                manager.Close();
                ChangeFollowFingerPosition(manager.inEditWindowButton.transform.position);
                return;
            }

            manager.SetText(stringIdxs[curIdx]);
        }else if(checkFindPath)
        {
            if (manager.tileManager.FindPath())
            {
                manager.SetNexttutorial();
            }
        }
    }

    public override void Clear()
    {
        manager.inEditWindowButton.onClick.RemoveListener(action1);
        manager.inTileDrawModeButton.onClick.RemoveListener(action2);
        manager.editTileButton.onClick.RemoveListener(action3);
        checkFindPath = false;

        manager.SetStopCoroutine();
    }
}
