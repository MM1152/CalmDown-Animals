
using UnityEngine;
using System.Collections;
using System;
public class CrewSellingTutorial : Tutorial
{
    public CrewSellingTutorial(TutorialManager manager) : base(manager) { }

    private readonly int[] stringTableIds1 = { 36 };
    private int[] curStringTable;
    private int curIdx;
    private Action action;
    private Vector3 startPos;
    public override void Clear()
    {

    }

    public override void Play()
    {
        manager.Close();

        manager.followFingerImage.SetActive(true);
        manager.followFingerImage.transform.position = 
            Camera.main.WorldToScreenPoint(manager.crewManager.GetCrewInTutorial().transform.position) + Vector3.up * 100f;

        manager.crewManager.OnClickCrew += OnClickCrew;
    }

    public override void Update()
    {
        if(curStringTable != null && curStringTable.Length > curIdx)
        {
            curIdx++;
            if(curIdx >= curStringTable.Length)
            {
                action?.Invoke();
            }
        }
    }

    private void CrewSellingEvent()
    {
        manager.SetStopCoroutine();
        manager.followFingerImage.SetActive(false);

        manager.crewManager.SellingEventInTutorial -= CrewSellingEvent;
        manager.Open();
        curStringTable = stringTableIds1;
        manager.SetText(curStringTable[curIdx]);
        action = manager.SetNexttutorial;
    }

    private void OnClickCrew()
    {
        startPos = Camera.main.WorldToScreenPoint(manager.crewManager.GetCrewInTutorial().transform.position);
        manager.SetStartCoroutine(FingerMoveCo());

        manager.crewManager.SellingEventInTutorial += CrewSellingEvent; 
        manager.crewManager.OnClickCrew -= OnClickCrew;
    }

    private IEnumerator FingerMoveCo()
    {
        Vector3 endPos = manager.sellingLayout.transform.position;

        for (float i = 0; i <= 1f; i += Time.deltaTime)
        {
            Vector3 newPos = Vector3.Lerp(startPos, endPos, i);
            manager.followFingerImage.transform.position = newPos + Vector3.up * 100f;
            yield return null;
        }

        manager.SetStartCoroutine(FingerMoveCo());
    }
}