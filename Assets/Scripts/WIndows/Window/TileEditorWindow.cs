using UnityEngine;
using UnityEngine.UI;
public class TileEditorWindow : GenericWindow
{
    [Header("Buttons")]
    public Button inEditModeButton;
    public Button backButtonInEditMode;

    public ButtonUI editButton;
    public ButtonUI destroyButton;

    public Button deleteButton;
    public Button backButton;

    [Header("Reference")]
    public PopupManager popupManager;
    public TileManager tileManager;

    [Header("UI Objects")]
    public GameObject selectModeGo;
    public GameObject editModeGo;

    private bool editMode;
    private bool destroyMode;

    private void Awake()
    {
        backButton.onClick.AddListener(() => CheckPath());
        inEditModeButton.onClick.AddListener(() => {
            editModeGo.SetActive(true);
            selectModeGo.SetActive(false);
        });
        backButtonInEditMode.onClick.AddListener(() => {
            editModeGo.SetActive(false);
            selectModeGo.SetActive(true);
        });
        editButton.GetComponent<Button>().onClick.AddListener(() =>
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
            editButton.IsOn = editMode;
            destroyButton.IsOn = destroyMode;
            tileManager.tileType = TileType.Path;
            tileManager.drawMode = editMode;
            DragAble.CameraDrag = !editMode;
        });
        destroyButton.GetComponent<Button>().onClick.AddListener(() =>
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
            editButton.IsOn = editMode;
            destroyButton.IsOn = destroyMode;
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
        editButton.IsOn = false;
        destroyButton.IsOn = false;
        editModeGo.gameObject.SetActive(false);
        tileManager.ClearRoad();
    }

    public override void Close()
    {
        tileManager.drawMode = false;
        DragAble.CameraDrag = true;
        base.Close();
    }
}