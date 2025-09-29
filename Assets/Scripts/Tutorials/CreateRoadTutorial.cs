using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CreateRoadTutorial : Tutorial
{
    public CreateRoadTutorial(TutorialManager manager) : base(manager) { }
    private readonly int[] stringIdx = new int[] { 14, 15, 16 };
    private readonly int[] stringIdx2 = new int[] { 17, 18 };
    private readonly int[] stringIdx3 = new int[] { 19, 20};
    private int[] currentStringTable;
    private int curIdx = 0;

    private UnityAction action1;

    private Action currentAction;

    private bool showCreateRoad;
    private bool showDisAblePopup;
    //14
    public override void Clear()
    {
        manager.backButtonInEditTile.onClick.RemoveListener(action1);
    }

    public override void Play()
    {
        manager.DisAbleAllButton();
        currentAction = FirstAction;
        currentStringTable = stringIdx;

        manager.backButtonInEditTile.interactable = true;

        manager.followFingerImage.SetActive(false);
        manager.arrowImage.SetActive(true);

        manager.Open();
        manager.SetText(currentStringTable[curIdx]);

        manager.arrowImage.transform.position = manager.goldObject.transform.position + Vector3.down * 100f;
        
        action1 = () =>
        {
            manager.Open();
            currentStringTable = stringIdx2;
            curIdx = -1;
            currentAction = SecondAction;
            manager.followFingerImage.SetActive(false);
        };

        manager.backButtonInEditTile.onClick.AddListener(action1);
    }

    public override void Update()
    { 
        if(currentStringTable == null)
        {
            manager.SetNexttutorial();
            return;
        }
        if(curIdx < currentStringTable.Length)
        {
            curIdx++;
            if(curIdx >= currentStringTable.Length)
            {
                currentAction?.Invoke();
                return;
            }
            manager.Open();
            manager.arrowImage.SetActive(false);
            manager.SetText(currentStringTable[curIdx]);
        }

        if(showDisAblePopup)
        {
            var popup = manager.popupManager.Open(Popup.TextPopUp) as StringPopUp;
            popup.Id = 0;
            showDisAblePopup = false;
            currentStringTable = stringIdx3;
            curIdx = -1;
            currentAction = ThirdAction;
        }

        if(showCreateRoad)
        {
            manager.tileManager.FindPathAndDrawRoads();
            manager.windowManager.Open(Window.EditorWindow);

            showCreateRoad = false;
            currentStringTable = null;
        }
    }

    private void FirstAction()
    {
        manager.Close();
        ChangeFollowFingerPosition(manager.backButtonInEditTile.gameObject , Side.Top);
    }

    private void SecondAction()
    {
        manager.Close();
        
        manager.tileManager.SetDragFailTileInTutorial();
        manager.followFingerImage.SetActive(false);
        showDisAblePopup = true;
    }

    private void ThirdAction()
    {
        manager.Close();
        manager.tileManager.DrawTwoRoadTilesInTutorial();
        showCreateRoad = true;
    }
    
}
