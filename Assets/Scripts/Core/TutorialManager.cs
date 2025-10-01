using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System;
using CsvHelper.Configuration.Attributes;

public enum TutorialType
{
    Tile = 0,
    Crew = 3,
    Equipment = 6
}

public class TutorialManager : MonoBehaviour
{
    [Header("Select Start Tutorial")]
    public TutorialType type;

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
    public GameObject employPanel;
    public GameObject employInternPanel;
    public GameObject crewHireCount;
    public GameObject crewPlaceCount;
    public GameObject crewHireCost;
    public GameObject payment;
    public GameObject sellingLayout;

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

    [Header("EmployCrew Window")]
    public Button clearAllCrew;
    public Button inEmployCrewBackButton;

    [Header("ReadyCrew Window")]
    public GameObject readyCrewWindow;
    public GameObject changeWeaponTab;
    public GameObject[] weaponListDisAbleTabs;
    public GameObject weaponListTab;
    public Button backButtonInReadyCrew;
    public GameObject captureAbleSizeTab;

    [Header("AnimalInfoTab")]
    public EnemySpawner enemyInfo;
    public GameObject animalInfoTab;
    
    private List<Tutorial> tutorials = new List<Tutorial>();
    private int curIdx = 0;
    private Tutorial curTutorial;
    
    private Coroutine co;

    //Tutorial Strings Start 7
    public IEnumerator Start()
    {
        yield return null;
        tutorials.Add(new DrawTileTutorial(this));
        tutorials.Add(new DeleteTileTutorial(this));
        tutorials.Add(new CreateRoadTutorial(this));
        tutorials.Add(new EmployCrewTutorial(this));
        tutorials.Add(new EmployCrewTutorial2(this));
        tutorials.Add(new CrewSellingTutorial(this));
        tutorials.Add(new CrewChangeEquipTutorial(this));

#if UNITY_EDITOR 
        curTutorial = tutorials[(int)type];
        curIdx = (int)type; 
#elif UNITY_ANDROID || UNITY_IOS
        curTutorial = tutorials[0];
        curIdx = 0;
#endif
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

        clearAllCrew.interactable = false;
        inEmployCrewBackButton.interactable = false;

        backButtonInReadyCrew.interactable = false;
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
