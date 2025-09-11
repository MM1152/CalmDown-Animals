using UnityEngine;

public class DrawTile : Tile
{
    private bool isDraw = false;
    public bool IsDraw 
    { 
        get => isDraw; 
        set {
            isDraw = value;
            if(isDraw)
            {
                foreach (var mater in material)
                {
                    mater.color = new Color(1f, 1f, 1f, 1f);
                }
            }
            else
            {
                foreach (var mater in material)
                {
                    mater.color = new Color(1f, 1f, 1f, 0.3f);
                }
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        foreach(var mater in material)
        {
            mater.color = new Color(1f, 1f, 1f, 0.3f);
        }
    }

    public void Draw()
    {
        IsDraw = true;
    }
}
