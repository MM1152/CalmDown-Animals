using UnityEngine;

public class ETATile : DrawTile
{
    public override bool IsDraw { 
        get => base.isDraw;
        set
        {
            isDraw = value;
            
        }
    }
    protected override void Awake()
    {
        base.Awake();
        foreach (var mater in material)
        {
            mater.color = new Color(1f, 0f, 0f, 1f);
        }
    }
    public override void Undo()
    {
        IsDraw = false;
    }
}