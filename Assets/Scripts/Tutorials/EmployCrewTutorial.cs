using System;
using UnityEngine.Events;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using UnityEditor;
public class EmployCrewTutorial : Tutorial
{
    public EmployCrewTutorial(TutorialManager manager) : base(manager) { }
    private readonly int[] stringTableId1 = { 21 };
    private readonly int[] stringTableId2 = { 22 };
    private readonly int[] stringTableId3 = { 23,24,25 };
    private readonly int[] stringTableId4 = { 26, 27, 28, 29 };
    private readonly int[] stringTableId5 = { 30, 31 };

    private int curIdx = 0; 
    private int[] curStringTable;

    private UnityAction action1;

    private Action tutorialAction1;
    private Action tutorialAction2;

    private Action curAction;
    private Action duringAction;

    public override void Clear()
    {

    }

    public override void Play()
    {
        manager.DisAbleAllButton();

        manager.arrowImage.SetActive(false);
        manager.inEmployCrewButton.interactable = true;

        curStringTable = stringTableId1;
        manager.followFingerImage.SetActive(false);

        curIdx = 0;
        manager.Open();
        manager.SetText(curStringTable[curIdx]);

        curAction = FirstAction;
        tutorialAction1 = () =>
        {
            manager.SetStopCoroutine();
            manager.followFingerImage.SetActive(false);

            curStringTable = stringTableId4;
            curIdx = 0;
            manager.Open();
            manager.SetText(curStringTable[curIdx]);
            FirstPositionArrow();
            manager.crewManager.CrewPlaceInTutorial -= tutorialAction1;
            manager.crewManager.OnClickCrew += tutorialAction2;

        };

        tutorialAction2 = () =>
        {
            manager.arrowImage.SetActive(false);

            curStringTable = stringTableId5;

            manager.Open();
            curIdx = -1;
            manager.crewManager.OnClickCrew -= tutorialAction2;
        };

        manager.crewManager.CrewPlaceInTutorial += tutorialAction1;

        action1 = () =>
        {
            curIdx = -1;
            curStringTable = stringTableId2;
            manager.Open();

            manager.followFingerImage.SetActive(false);
            curAction = SecondAction;
        };

        manager.inEmployCrewButton.onClick.AddListener(action1);
    }

    public override void Update()
    {
         if(curIdx < curStringTable.Length)
         {
            curIdx++;
            if(curStringTable.Length <= curIdx)
            {
                curAction?.Invoke();
                return;
            }
            manager.Open();
            manager.arrowImage.SetActive(false);
            duringAction?.Invoke();
            manager.SetText(curStringTable[curIdx]);
         }
    }

    private void FirstPositionArrow()
    {
        manager.arrowImage.SetActive(true);
        manager.arrowImage.transform.eulerAngles = Vector3.zero;
        ChangeArrowPosition(manager.crewHireCount.gameObject, Side.Right);
        duringAction = SecondPositionArrow;
    }

    private void SecondPositionArrow()
    {
        manager.arrowImage.SetActive(true);
        manager.arrowImage.transform.eulerAngles = Vector3.zero;
        ChangeArrowPosition(manager.crewPlaceCount.gameObject, Side.Right);
        duringAction = null;
        curAction = ShowAttackRadiusCrew;
    }

    private void ShowAttackRadiusCrew()
    {
        manager.Close();
        var crew = manager.crewManager.GetCrewInTutorial();
        var arrowPos = Camera.main.WorldToScreenPoint(crew.transform.position) +new Vector3(-50f , 70f , 0f);

        manager.arrowImage.SetActive(true);
        manager.arrowImage.transform.eulerAngles = Vector3.forward * 120f;
        manager.arrowImage.transform.position = arrowPos;
        curAction = () => manager.SetNexttutorial();
    }

    private void FirstAction()
    {
        ChangeFollowFingerPosition(manager.inEmployCrewButton.gameObject, Side.Top);
        manager.Close();    
        action1 = null;
    }

    private void SecondAction()
    {
        manager.Close();
        manager.arrowImage.transform.eulerAngles += new Vector3(0f, 0f, 90f);
        manager.arrowImage.SetActive(true);

        ChangeArrowPosition(manager.employPanel.gameObject , Side.Right);

        curStringTable = stringTableId3;
        curIdx = -1;
        curAction = ThirdAction;
    }

    private void ThirdAction()
    {
        var uiEvent = manager.employInternPanel.AddComponent<UIEvent>();
        uiEvent.PointerClick += (eventData) =>
        {
            manager.SetStartCoroutine(FingerMoveCo());
            uiEvent.enabled = false;
        };
        ChangeFollowFingerPosition(manager.employInternPanel.gameObject , Side.Top);
        manager.Close();
        duringAction = FirstPositionArrow;
        curAction = null;
    }

    private IEnumerator FingerMoveCo()
    {
        Vector3 startPos = manager.employInternPanel.transform.position + new Vector3(250f, -150f, 0f);
        Vector3 endPos = Camera.main.WorldToScreenPoint(manager.tileManager.tileList[5].transform.position);

        for (float i = 0; i <= 1f; i += Time.deltaTime)
        {
            Vector3 newPos = Vector3.Lerp(startPos, endPos, i);
            manager.followFingerImage.transform.position = newPos + Vector3.up * 100f;
            yield return null;
        }

        manager.SetStartCoroutine(FingerMoveCo());
    }

}