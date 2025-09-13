using UnityEngine;
using UnityEngine.UI;
public class TileEditorWindow : GenericWindow
{
    public Button editButton;
    public Button deleteButton;
    public Button backButton;

    public PopupManager popupManager;
    public TileManager tileManager;

    private void Awake()
    {
        backButton.onClick.AddListener(() => CheckPath());
        editButton.onClick.AddListener(() => tileManager.drawMode = !tileManager.drawMode); 
    }

    private void CheckPath()
    {
        bool susecss = tileManager.FindPath();

        if(susecss)
        {
            manager.Open(Window.EditorWindow);
        }else
        {
            popupManager.Open(Popup.TextPopUp);
        }
    }

    public override void Close()
    {
        tileManager.drawMode = false;
        base.Close();
    }
}