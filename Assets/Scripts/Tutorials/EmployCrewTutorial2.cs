//PPT 45 Page

using UnityEngine;
using System.Collections;
using System;

public class EmployCrewTutorial2 : Tutorial
{
    private readonly int[] stringTableIds1 = { 32, 33 , 34 , 35 };

    private int[] curStringTable;
    private int curIdx = 0;
    public EmployCrewTutorial2(TutorialManager manager) : base(manager) { }
    private Vector3 startPos;

    private Action duringAction;
    private Action action;

    public override void Clear()
    {

    }

    public override void Play()
    {
        manager.Close();
        manager.crewManager.ClearDragCrew();

        manager.followFingerImage.SetActive(true);
        manager.followFingerImage.transform.position = 
            Camera.main.WorldToScreenPoint(manager.crewManager.GetCrewInTutorial().transform.position) + Vector3.up * 100f;
        manager.crewManager.OnClickCrew += OnClickCrewAction;
    }

    public override void Update()
    {
        if(curStringTable != null && curStringTable.Length > curIdx)
        {
            curIdx++;
            if(curIdx >= curStringTable.Length)
            {
                action?.Invoke();
                return;
            }
            manager.arrowImage.SetActive(false);
            duringAction?.Invoke();
            manager.Open();
            manager.SetText(curStringTable[curIdx]);
        }
    }

    private void OnClickCrewAction()
    {
        manager.SetStartCoroutine(FingerMoveCo());
        startPos = Camera.main.WorldToScreenPoint(manager.crewManager.GetCrewInTutorial().transform.position);
        manager.crewManager.ReturnCrew += ReturnCrewAction;
        manager.crewManager.OnClickCrew -= OnClickCrewAction;
    }

    private void ReturnCrewAction()
    {
        curStringTable = stringTableIds1;
        manager.Open();
        curIdx = 0;
        manager.SetText(curStringTable[curIdx]);    

        manager.SetStopCoroutine();
        manager.crewManager.ReturnCrew -= ReturnCrewAction;
        manager.followFingerImage.SetActive(false);

        ChangeArrowPosition(manager.crewHireCost , Side.Top);
        manager.arrowImage.transform.eulerAngles = Vector3.forward * 90f;
        duringAction = () =>
        {
            ChangeArrowPosition(manager.payment, Side.Bottom);
            manager.arrowImage.transform.eulerAngles = Vector3.back * 90f;

            duringAction = () =>
            {
                ChangeArrowPosition(manager.goldObject, Side.Bottom);
                manager.arrowImage.transform.eulerAngles = Vector3.back * 90f;
                duringAction = null;
                action = manager.SetNexttutorial;
            };
        };
    } 

    private IEnumerator FingerMoveCo()
    {
        Vector3 endPos = manager.employInternPanel.transform.position + new Vector3(250f, -150f, 0f);

        for (float i = 0; i <= 1f; i += Time.deltaTime)
        {
            Vector3 newPos = Vector3.Lerp(startPos, endPos, i);
            manager.followFingerImage.transform.position = newPos + Vector3.up * 100f;
            yield return null;
        }

        manager.SetStartCoroutine(FingerMoveCo());
    }
}