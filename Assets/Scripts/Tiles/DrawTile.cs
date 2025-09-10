using UnityEngine;

public class DrawTile : Tile
{
    private SpriteRenderer spriteRenderer;
    private bool isDraw = false;
    public bool IsDraw 
    { 
        get => isDraw; 
        set {
            isDraw = value;
            if(isDraw)
            {

            }else
            {

            }
        }
    }

    public void Draw()
    {
        IsDraw = true;
    }
}
