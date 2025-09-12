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
    public CrewSpawner spawner;

    public GameObject internHire;
    public LayerMask mask;
    private void Start()
    {
        backBNT.onClick.AddListener(() => manager.Open(Window.EditorWindow));
    }
}
