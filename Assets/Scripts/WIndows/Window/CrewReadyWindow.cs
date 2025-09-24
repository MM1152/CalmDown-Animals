
using UnityEngine.UI;

public class CrewReadyWindow : GenericWindow
{
    public CrewManager crewManager;
    public CrewReadyTab readyTab;
    public Button backButton;

    private void Awake()
    {
        backButton.onClick.AddListener(() => manager.Open(Window.EditorWindow));
    }

    public override void Open()
    {
        base.Open();
        Status.CrewTab = true;
    }

    public override void Close()
    {
        base.Close();
        readyTab.Close();
        crewManager.ClearDragCrew();
        Status.CrewTab = false;
    }

    private void Update()
    {
        if(crewManager.DragCrew && !readyTab.gameObject.activeSelf)
        {
            readyTab.Open(crewManager.DragCrew);
        }
        else if(!crewManager.DragCrew)
        {
            readyTab.Close();
        }
    }
}