using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EmployUnitWindow : GenericWindow
{
    public LayerMask mask;

    [Header("Reference")]
    public Button backBNT;
    public GameObject sellLayout;
    public CrewManager spawner;
    public Button clearCrewsBNT;

    private void Start()
    {
        backBNT.onClick.AddListener(() => manager.Open(Window.EditorWindow));
        clearCrewsBNT.onClick.AddListener(() => spawner.ClearAllCrews());
    }

    public override void Open()
    {
        base.Open();
        sellLayout.SetActive(false);
        DragAble.CrewDrag = true;
    }

    public override void Close()
    {
        base.Close();
        DragAble.CrewDrag = false;
    }

    public void Update()
    {
        if(spawner.IsDrag)
        {
            sellLayout.SetActive(true);
        }else
        {
            sellLayout.SetActive(false);
        }
    }
}
