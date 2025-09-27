using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System;
public class TutorialManager : MonoBehaviour
{
    [Header("Reference")]
    public WindowManager windowManager;
    public TileManager tileManager;
    public CrewManager crewManager;
    public PopupManager popupManager;

    [Header("Tutorial Objects")]
    public GameObject tutorial;
    public TextMeshProUGUI tutorialText;
    public GameObject arrowImage;

    [Header("FingerImage")]
    public GameObject followFingerImage;

    [Header("FollowTargets")]
    public GameObject goldObject;

    [Header("Editor Window")]
    public Button inEditWindowButton;
    public Button inEmployCrewButton;
    public Button inReadyCrewButton;
    public Button inStartButton;

    [Header("TileEditor Window")]
    public Button inTileDrawModeButton;
    public Button disAbleTileDeleteButton;
    public Button backButtonInTileEditor;
    [Space(10)]
    public Button editTileButton;
    public Button deleteTileButton;
    public Button deleteAllTileButton;
    public Button backButtonInEditTile;
    
    private List<Tutorial> tutorials = new List<Tutorial>();
    private int curIdx = 0;
    private Tutorial curTutorial;
    
    private Coroutine co;

    //Tutorial Strings Start 7
    public void Start()
    {
        tutorials.Add(new DrawTileTutorial(this));
        tutorials.Add(new DeleteTileTutorial(this));
        tutorials.Add(new CreateRoadTutorial(this));
        
        curTutorial = tutorials[curIdx];
        curTutorial.Play();
    }

    private void LateUpdate()
    {
        if((TouchManager.touchType == TouchType.Tab || TouchManager.touchType == TouchType.Drag) && TouchManager.Phase == Phase.Up)
        {
            curTutorial.Update();
        }
    }

    public void SetText(int id)
    {
        tutorialText.text = DataTableManager.stringTable.Get(id);
    }

    public void Close()
    {
        tutorial.SetActive(false);
    }

    public void Open()
    {
        tutorial.SetActive(true);
    }

    public void SetNexttutorial()
    {
        curTutorial.Clear();
        curIdx++;
        if (curIdx < tutorials.Count)
        {
            curTutorial = tutorials[curIdx];
        }
        curTutorial.Play();
    }

    public void DisAbleAllButton()
    {
        inEditWindowButton.interactable = false;
        inEmployCrewButton.interactable = false;
        inReadyCrewButton.interactable = false;
        inStartButton.interactable = false;

        inTileDrawModeButton.interactable = false;
        disAbleTileDeleteButton.interactable = false;
        backButtonInTileEditor.interactable = false;

        editTileButton.interactable = false;
        deleteTileButton.interactable = false; 
        deleteAllTileButton.interactable = false;
        backButtonInEditTile.interactable = false;
    }

    public void SetStartCoroutine(IEnumerator ienum)
    {
        if(co != null)
        {
            StopCoroutine(co);
            co = null;
        }
        co = StartCoroutine(ienum);
    }

    public void SetStopCoroutine()
    {
        if(co != null)
        {
            StopCoroutine(co);
            co = null;
        }
    }
}
