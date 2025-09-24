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
    public Button deleteAllTilesBNT;
    [Header("Reference")]
    public PopupManager popupManager;
    public TileManager tileManager;

    [Header("UI Objects")]
    public GameObject selectModeGo;
    public GameObject editModeGo;
    public GameObject editModeOutline;

    private bool editMode;
    private bool destroyMode;

    private void Awake()
    {
        backButton.onClick.AddListener(() => CheckPath());
        inEditModeButton.onClick.AddListener(() => {
            editModeGo.SetActive(true);
            selectModeGo.SetActive(false);
            tileManager.drawMode = false;
            Status.CameraDrag = true;
        });
        backButtonInEditMode.onClick.AddListener(() => {
            editModeGo.SetActive(false);
            selectModeGo.SetActive(true);
            editButton.IsOn = false;
            destroyButton.IsOn = false;
            tileManager.drawMode = false;
            Status.CameraDrag = true;
            editModeOutline.SetActive(false);
        });
        deleteAllTilesBNT.onClick.AddListener(() => tileManager.ClearAllTiles());
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
            UpdateSetting(editMode, destroyMode, TileType.Path);
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
            UpdateSetting(editMode, destroyMode, TileType.None);
            
        });
    }

    private void UpdateSetting(bool editMode, bool destroyMode, TileType tileType)
    {
        editButton.IsOn = editMode;
        destroyButton.IsOn = destroyMode;
        tileManager.tileType = tileType;
        tileManager.drawMode = editMode || destroyMode ? true : false;
        Status.CameraDrag = !tileManager.drawMode;
        editModeOutline.SetActive(tileManager.drawMode);
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
        tileManager.InEditorWindow = true;
        editButton.IsOn = false;
        destroyButton.IsOn = false;
        editModeGo.gameObject.SetActive(false);
        tileManager.ChangeToColorPathTiles();
        tileManager.ClearRoad();
    }

    public override void Close()
    {
        tileManager.InEditorWindow = false;
        tileManager.drawMode = false;
        tileManager.ResetToColorPathTiles();
        Status.CameraDrag = true;
        base.Close();
    }
}