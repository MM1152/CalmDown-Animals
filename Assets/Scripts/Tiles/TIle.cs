using UnityEngine;

public enum TileType
{
    None,
    Blocked,
    Path,
}

public class Tile : MonoBehaviour
{
    private TileType type;
    public TileType Type
    {
        get => type;
        set
        {
            type = value;
            switch (type)
            {
                case TileType.None:
                    GetComponent<Renderer>().material.color = Color.white;
                    break;
                case TileType.Blocked:
                    GetComponent<Renderer>().material.color = Color.black;
                    break;
                case TileType.Path:
                    GetComponent<Renderer>().material.color = Color.green;
                    break;
            }
        }
    }
}