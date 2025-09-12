using UnityEngine;

public class NeighborPosition
{
    public Vector2 gridSize;
    public readonly Vector3[] nextNeighborPos = new Vector3[6];

    public NeighborPosition(Renderer render)
    {
        this.gridSize = new Vector2( Mathf.Ceil(render.bounds.size.z * 10f) / 10f , Mathf.Ceil(render.bounds.size.x * 10f) / 10f);
        Debug.Log(gridSize);
        nextNeighborPos[0] = GetFloor(new Vector3(-(gridSize.x - gridSize.x * 0.5f), 0, gridSize.y));
        nextNeighborPos[1] = GetFloor(new Vector3((gridSize.x - gridSize.x * 0.5f) , 0, gridSize.y ));
        nextNeighborPos[2] = GetFloor(new Vector3((gridSize.x - gridSize.x * 0.5f), 0, -gridSize.y));
        nextNeighborPos[3] = GetFloor(new Vector3(-(gridSize.x - gridSize.x * 0.5f), 0, -gridSize.y));
        nextNeighborPos[4] = GetFloor(new Vector3(gridSize.x, 0, 0));
        nextNeighborPos[5] = GetFloor(new Vector3(-gridSize.x, 0 ,0));
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

public static class DataTableIds
{
    public static readonly string StringTableIds = "StringTable";

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