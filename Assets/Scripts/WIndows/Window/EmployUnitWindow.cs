using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EmployUnitWindow : GenericWindow
{
    [Header("Reference")]
    public TextMeshProUGUI hireText;
    public TextMeshProUGUI placedText;

    public Button backBNT;

    public GameObject internHire;
    public LayerMask mask;
    private void Start()
    {
        backBNT.onClick.AddListener(() => manager.Open(Window.EditorWindow));
    }

    public void Update()
    {
        if(TouchManager.TouchType == TouchType.Tab)
        {
            Ray ray = new Ray(TouchManager.GetDragWorldPosition());
        }
    }
}
