using UnityEngine;
using System.Collections.Generic;

public class DrawTile : Tile
{
    public List<DrawTile> AroundTile { get; set; } = new List<DrawTile>(); 

    protected bool isDraw = false;
    public virtual bool IsDraw 
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

    public virtual void Undo()
    {
        IsDraw = false;
        AroundTile.Clear();
    }
}
