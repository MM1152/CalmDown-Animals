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
        editButton.onClick.AddListener(() =>
        {
            tileManager.drawMode = !tileManager.drawMode;
            DragAble.CameraDrag = !tileManager.drawMode;
        });
    }

    private void CheckPath()
    {
        bool susecss = tileManager.FindPath();

        if(susecss)
        {
            manager.Open(Window.EditorWindow);
        }else
        {
            var popup = (StringPopUp)popupManager.Open(Popup.TextPopUp);
            popup.Id = 0;
        }
    }

    public override void Close()
    {
        tileManager.drawMode = false;
        DragAble.CameraDrag = true;
        base.Close();
    }
}