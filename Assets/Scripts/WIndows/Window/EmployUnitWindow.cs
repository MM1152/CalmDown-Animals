using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EmployUnitWindow : GenericWindow
{
    [Header("Reference")]
    public Button backBNT;
    public GameObject internHire;
    public LayerMask mask;
    public GameObject sellLayout;
    public CrewSpawner spawner;

    private void Start()
    {
        backBNT.onClick.AddListener(() => manager.Open(Window.EditorWindow));
    }

    private void OnEnable()
    {
        sellLayout.SetActive(false);
    }

    public void Update()
    {
        if(TouchManager.TouchType == TouchType.Tab)
        {
            Ray ray = Camera.main.ScreenPointToRay(TouchManager.GetDragPos());
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                var find = hit.collider.GetComponent<Crew>();
                if(find != null)
                {
                    find.SetDrag();
                }
            }
        }
        if(spawner.IsSpawn)
        {
            sellLayout.SetActive(true);
        }else
        {
            sellLayout.SetActive(false);
        }
    }
}
