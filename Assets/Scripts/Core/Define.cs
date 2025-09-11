using UnityEngine;

public class NeighborPosition
{
    public Vector2 gridSize;
    public readonly Vector3[] nextNeighborPos = new Vector3[6];

    public NeighborPosition(Renderer render)
    {
        this.gridSize = new Vector2(Mathf.Ceil(render.bounds.size.x * 10f) / 10f, Mathf.Ceil(render.bounds.size.z * 10f) / 10f);

        nextNeighborPos[0] = GetFloor(new Vector3(gridSize.y, 0, -(gridSize.x - gridSize.x * 0.5f)));
        nextNeighborPos[1] = GetFloor(new Vector3(gridSize.y, 0, (gridSize.x - gridSize.x * 0.5f)));
        nextNeighborPos[2] = GetFloor(new Vector3(-gridSize.y, 0, (gridSize.x - gridSize.x * 0.5f)));
        nextNeighborPos[3] = GetFloor(new Vector3(-gridSize.y, 0, -(gridSize.x - gridSize.x * 0.5f)));
        nextNeighborPos[4] = GetFloor(new Vector3(0, 0, gridSize.x));
        nextNeighborPos[5] = GetFloor(new Vector3(0, 0, -gridSize.x));
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