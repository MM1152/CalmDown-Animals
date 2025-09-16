using UnityEngine;

public class NeighborPosition
{
    public Vector2 gridSize;
    public readonly Vector3[] nextNeighborPos = new Vector3[6];

    public NeighborPosition(Renderer render)
    {
        this.gridSize = new Vector2( Mathf.Ceil(render.bounds.size.z * 10f) / 10f , Mathf.Ceil(render.bounds.size.x * 10f) / 10f);
        nextNeighborPos[0] = GetFloor(new Vector3((gridSize.x - gridSize.x * 0.5f) , 0, gridSize.y ));
        nextNeighborPos[1] = GetFloor(new Vector3(-(gridSize.x - gridSize.x * 0.5f), 0, gridSize.y));
        nextNeighborPos[2] = GetFloor(new Vector3(-gridSize.x, 0 ,0));
        nextNeighborPos[3] = GetFloor(new Vector3(-(gridSize.x - gridSize.x * 0.5f), 0, -gridSize.y));
        nextNeighborPos[4] = GetFloor(new Vector3((gridSize.x - gridSize.x * 0.5f), 0, -gridSize.y));
        nextNeighborPos[5] = GetFloor(new Vector3(gridSize.x, 0, 0));

        //nextNeighborPos[0] = GetFloor(new Vector3(-(gridSize.x - gridSize.x * 0.5f), 0, gridSize.y));
        //nextNeighborPos[1] = GetFloor(new Vector3((gridSize.x - gridSize.x * 0.5f) , 0, gridSize.y ));
        //nextNeighborPos[2] = GetFloor(new Vector3((gridSize.x - gridSize.x * 0.5f), 0, -gridSize.y));
        //nextNeighborPos[3] = GetFloor(new Vector3(-(gridSize.x - gridSize.x * 0.5f), 0, -gridSize.y));
        //nextNeighborPos[4] = GetFloor(new Vector3(gridSize.x, 0, 0));
        //nextNeighborPos[5] = GetFloor(new Vector3(-gridSize.x, 0 ,0));
    }
    
    public static float GetFloor(float value)
    {
        return Mathf.Round(value * 10f) / 10f;
    }

    public static Vector3 GetFloor(Vector3 value)
    {
        return new Vector3(
            Mathf.Round(value.x * 10f) / 10f,
            Mathf.Round(value.y * 10f) / 10f,
            Mathf.Round(value.z * 10f) / 10f
        );
    }
}

public enum AnimalTypes
{
    Hyena = 212356,
    Rabbit = 211445,
    Red_Fox = 212434,
    Warthog = 213335,
    Nanger_Granti = 213423,
}

public static class TagIds
{
    public static readonly string GameManagerTag = "GameController";

}

public static class DataTableIds
{
    public static readonly string StringTableIds = "StringTable";
    public static readonly string RoundTableIds = "RoundTable";
    public static readonly string AnimalSizeTable = "AnimalSizeTable";
    public static readonly string AnimalSpeedTable = "AnimalSpeedTable";
    public static readonly string AnimalInfoTable = "AnimalInfoTable";
    public static readonly string AnimalCRRankTable = "AnimalCRRank";

    public static readonly string[] MapDataIds =
    {
        "SabanaMap"
    };
}

public enum Window
{
    EditorWindow,
    TileEditorWindow,
    EmployUnitWindow,
}

public enum Popup
{
    TextPopUp,
}

public enum CrewRank
{
    Intern,
}