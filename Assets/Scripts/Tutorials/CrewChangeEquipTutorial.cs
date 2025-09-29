using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CrewChangeEquipTutorial : Tutorial
{
    public CrewChangeEquipTutorial(TutorialManager manager) : base(manager) { }

    private readonly int[] stringTableIds1 = { 37,38 };
    private readonly int[] stringTableIds2 = { 39 , 40 ,41 };
    private readonly int[] stringTableIds3 = { 42, 43, 44, 45, 46 };
    private readonly int[] stringTableIds4 = { 47,48 };
    private readonly int[] stringTableIds5 = { 49,50,51,52,53,54 };
    private int[] curStirngTable;

    private int curIdx;

    private Action action;
    private UnityAction onClickReadyCrew;

    public override void Clear()
    {
        manager.inReadyCrewButton.onClick.RemoveListener(onClickReadyCrew);
    }

    public override void Play()
    {
        manager.weaponListDisAbleTabs[0].GetComponent<UIEvent>().enabled = false;
        manager.weaponListDisAbleTabs[1].GetComponent<UIEvent>().enabled = false;
        
        manager.DisAbleAllButton();
        manager.arrowImage.SetActive(false);
        manager.Close();
        manager.inReadyCrewButton.interactable = true;
        onClickReadyCrew = () =>
        {
            curStirngTable = stringTableIds1;
            curIdx = -1;
            action = ClickCrew;
        };
        manager.inReadyCrewButton.onClick.AddListener(onClickReadyCrew);

        ChangeFollowFingerPosition(manager.inReadyCrewButton.gameObject, Side.Top);

        manager.windowManager.Open(Window.EditorWindow);
        if(manager.crewManager.GetPlaceCrewCount() == 0)
        {
            manager.crewManager.CrewForcingSpawn(CrewRank.Intern, manager.tileManager.GetTileInTutorial());
        }

    }

    public override void Update()
    {
        if(curStirngTable != null && curStirngTable.Length > curIdx)
        {
            curIdx++;
            if(curIdx >= curStirngTable.Length)
            {
                action?.Invoke();
                return;
            }
            if(curIdx < 0)
            {
                return;
            }

            manager.Open();
            manager.SetText(curStirngTable[curIdx]);
        }
    }

    private void ClickCrew()
    {
        manager.Close();
        manager.followFingerImage.transform.SetParent(manager.readyCrewWindow.transform);
        manager.followFingerImage.SetActive(true);
        manager.followFingerImage.transform.position =
            Camera.main.WorldToScreenPoint(manager.crewManager.GetCrewInTutorial().transform.position) + Vector3.up * 100f;

        manager.crewManager.OnClickCrew += OnClickCrew;
        action = null;
    }
    private void OnClickCrew()
    {
        curStirngTable = stringTableIds2;
        curIdx = -1;

        manager.crewManager.OnClickCrew -= OnClickCrew;

        // 다음 Action 지정
        action = ClickToChangeWeapon;
    }
    private void ClickToChangeWeapon()
    {
        ChangeFollowFingerPosition(manager.changeWeaponTab, Side.Top);
        manager.Close();
        manager.changeWeaponTab.GetComponent<UIEvent>().PointerClick += OnClickToChangeWeapon;
        action = null;
    }

    private void OnClickToChangeWeapon(PointerEventData data)
    {
        curStirngTable = stringTableIds3;
        curIdx = -1;

        manager.changeWeaponTab.GetComponent<UIEvent>().PointerClick -= OnClickToChangeWeapon;
        action = ChangeWeapon;
    }

    private void ChangeWeapon()
    {
        ChangeFollowFingerPosition(manager.weaponListTab, Side.Top);
        manager.Close();
        manager.weaponListTab.GetComponent<UIEvent>().PointerClick += ChangeToWeapon;
        action = null;
    }

    private void ChangeToWeapon(PointerEventData data)
    {
        manager.arrowImage.transform.eulerAngles = Vector3.zero;
        ChangeArrowPosition(manager.captureAbleSizeTab , Side.Right);
        curStirngTable = stringTableIds4;
        curIdx = -2;

        //다음 액션
        action = ShowAnimalInfo;
    }

    private void ShowAnimalInfo()
    {
        manager.Close();
        manager.followFingerImage.transform.SetParent(manager.readyCrewWindow.transform);
        manager.followFingerImage.transform.position =
            Camera.main.WorldToScreenPoint(manager.enemyInfo.GetInfoTileInTutorial().transform.position) + Vector3.up * 100f;
        manager.tileManager.OnClickInfoTileInTutorial += OnClickInfoTile;
        manager.arrowImage.SetActive(false);
    }

    private void OnClickInfoTile()
    {
        curStirngTable = stringTableIds5;
        curIdx = -1;
        manager.followFingerImage.SetActive(false);
        manager.tileManager.OnClickInfoTileInTutorial -= OnClickInfoTile;
        action = () =>
        {
            SaveLoadManager.Data.isClearTutorial = true;
            SaveLoadManager.Save();
            SceneManager.LoadScene(1);
        };
    }
}