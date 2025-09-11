using UnityEngine;

public enum TileType
{
    None,
    Blocked,
    Path,
    Crew,
}

public class Tile : MonoBehaviour
{
    private new Renderer renderer;
    protected Material[] material;
    private TileType type;
    public TileType Type
    {
        get => type;
        set
        {
            type = value;
            if (material.Length == 0) return;
            switch (type)
            {
                case TileType.None:
                    material[0].color = Color.white;
                    break;
                case TileType.Blocked:
                    material[0].color = Color.black;
                    break;
                case TileType.Path:
                    material[0].color = Color.green;
                    break;
                case TileType.Crew:
                    material[0].color = Color.blue;
                    break;
            }
        }
    }

    protected virtual void Awake()
    {
        renderer = GetComponent<Renderer>();
        material = renderer.materials;
    }
}