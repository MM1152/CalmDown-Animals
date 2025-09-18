using UnityEngine.UI;
public class TileEditorWindow : GenericWindow
{
    public Button editButton;

    public Button destroyButton;

    public Button deleteButton;
    public Button backButton;

    public PopupManager popupManager;
    public TileManager tileManager;


    private bool editMode;
    private bool destroyMode;

    private void Awake()
    {
        backButton.onClick.AddListener(() => CheckPath());
        editButton.onClick.AddListener(() =>
        {
            if (destroyMode)
            {
                destroyMode = false;
                editMode = true;
            }
            else
            {
                editMode = !editMode;
            }

            tileManager.tileType = TileType.Path;
            tileManager.drawMode = editMode;
            DragAble.CameraDrag = !editMode;
        });
        destroyButton.onClick.AddListener(() =>
        {
            if(editMode)
            {
                editMode = false;
                destroyMode = true;
            }
            else
            {
                destroyMode = !destroyMode;
            }
            tileManager.tileType = TileType.None;
            tileManager.drawMode = destroyMode;
            DragAble.CameraDrag = !destroyMode;
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

    public override void Open()
    {
        base.Open();
        tileManager.ClearRoad();
    }

    public override void Close()
    {
        tileManager.drawMode = false;
        DragAble.CameraDrag = true;
        base.Close();
    }
}