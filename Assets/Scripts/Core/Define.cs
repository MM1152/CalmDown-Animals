using UnityEngine;

public class NeighborPosition
{
    public Vector2 gridSize;
    public readonly Vector3[] nextNeighborPos = new Vector3[6];

    public NeighborPosition(Renderer render)
    {
        this.gridSize = new Vector2(Mathf.Ceil(render.bounds.size.z * 10f) / 10f, Mathf.Ceil(render.bounds.size.x * 10f) / 10f);
        Debug.Log(render.bounds.size);
        Debug.Log(gridSize);
        nextNeighborPos[0] = GetFloor(new Vector3((gridSize.x - gridSize.x * 0.5f) * 0.9f , 0, gridSize.y * 0.9f ));
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

public static class HexaUtility
{
    public static int FindHexaSector(Vector3 center, Vector3 point)
    {
        float dx = point.x - center.x;
        float dz = point.z - center.z;

        float angle = Mathf.Atan2(dz, dx);
        if (angle < 0) angle += Mathf.PI * 2f;
        float sectorSize = Mathf.PI * 2f / 6f;

        return Mathf.FloorToInt(angle / sectorSize % 6 + 6) % 6;
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

public static class FloatUtility
{
    public static float Normalization(this float x , float max , float min)
    {   
        return (x - min) / (max - min);
    }
    public static float ReverseNormalization(this float x, float max, float min)
    {
        return x * (max - min) + min;
    }
}

public static class DragAble
{
    public static bool CameraDrag = true;
    public static bool CrewDrag;
}

public static class DataTableIds
{
    public static readonly string StringTableIds = "StringTable";
    public static readonly string RoundTableIds = "RoundTable";
    public static readonly string AnimalSizeTable = "AnimalSizeTable";
    public static readonly string AnimalSpeedTable = "AnimalSpeedTable";
    public static readonly string AnimalInfoTable = "AnimalInfoTable";
    public static readonly string AnimalCRRankTable = "AnimalCRRank";
    public static readonly string CrewTable = "CrewTable";
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
    DuringGameWindow,
}

public enum Popup
{
    TextPopUp,
}

public enum CrewRank
{
    Intern = 30110,
    Newbie,
    Senior,
    Ace,
}