using UnityEngine;
using UnityEngine.UI;
public class TileEditorWindow : GenericWindow
{
    [Header("Buttons")]
    public Button inEditModeButton;
    public Button inDeleteButton;
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

    [Header("Sprite")]
    public Sprite initButtonSprite;
    public Sprite pressedButtonSprite;

    private bool editMode;
    private bool destroyMode;

    private void Awake()
    {
        backButton.onClick.AddListener(() => {
            CheckPath();
            SoundManager.Instance.PlayOneShot(SFX.BackSound);
        });
        inDeleteButton.onClick.AddListener(() =>
        {
            tileManager.deleteMode = !tileManager.deleteMode;
            if(tileManager.deleteMode)
            {
                inDeleteButton.GetComponent<Image>().sprite = pressedButtonSprite;
            }else
            {
                inDeleteButton.GetComponent<Image>().sprite = initButtonSprite;
            }
            Status.CameraDrag = false;
        });
        inEditModeButton.onClick.AddListener(() => {
            inDeleteButton.GetComponent<Image>().sprite = initButtonSprite;
            tileManager.deleteMode = false;
            editModeGo.SetActive(true);
            selectModeGo.SetActive(false);
            tileManager.drawMode = false;
            Status.CameraDrag = true;
            SoundManager.Instance.PlayOneShot(SFX.OnClickButtonSound);
        });
        backButtonInEditMode.onClick.AddListener(() => {
            editModeGo.SetActive(false);
            selectModeGo.SetActive(true);
            editButton.IsOn = false;
            destroyButton.IsOn = false;
            tileManager.drawMode = false;
            Status.CameraDrag = true;
            editModeOutline.SetActive(false);
            SoundManager.Instance.PlayOneShot(SFX.BackSound);
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
        manager.Open(Window.EditorWindow);
        inDeleteButton.GetComponent<Image>().sprite = initButtonSprite;
        tileManager.deleteMode = false;
        Status.CameraDrag = true;

        tileManager.FindPathAndDrawRoads();

        //if(susecss)
        //{
        //    manager.Open(Window.EditorWindow);
        //    inDeleteButton.GetComponent<Image>().sprite = initButtonSprite;
        //    tileManager.deleteMode = false;
        //    Status.CameraDrag = true;
        //}
        //else
        //{
        //    var popup = (StringPopUp)popupManager.Open(Popup.TextPopUp);
        //    popup.Id = 0;
        //}
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