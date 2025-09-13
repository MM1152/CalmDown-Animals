using UnityEngine;
using UnityEngine.UI;

public class EditorWindow : GenericWindow
{
    public Button editTileBNT;
    public Button employUnitBNT;
    public Button readyUnitBNT;
    public Button startBNT;

    public void Start()
    {
        editTileBNT.onClick.AddListener(() => manager.Open(Window.TileEditorWindow));
        employUnitBNT.onClick.AddListener(() => manager.Open(Window.EmployUnitWindow));
    }
}