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
        editTileBNT.onClick.AddListener(() => {
            manager.Open(Window.TileEditorWindow);
            SoundManager.Instance.PlayOneShot(SFX.OnClickButtonSound);
        });
        employUnitBNT.onClick.AddListener(() => {
            manager.Open(Window.EmployUnitWindow);
            SoundManager.Instance.PlayOneShot(SFX.OnClickButtonSound);
        });
        readyUnitBNT.onClick.AddListener(() => {
            manager.Open(Window.CrewReadyWindow);       
            SoundManager.Instance.PlayOneShot(SFX.OnClickButtonSound);
        });
    }

    public override void Open()
    {
        base.Open();
        Status.ShowAnimalInfo = true;
    }

    public override void Close()
    {
        base.Close();
        Status.ShowAnimalInfo = true;
    }
}