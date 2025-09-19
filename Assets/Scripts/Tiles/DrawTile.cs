using UnityEngine;
using System.Collections.Generic;

public class DrawTile : Tile
{
    public int layer = -1;

    private DrawType drawType;
    public DrawType DrawType
    {
        get { return drawType; }
        set
        {
            drawType = value;

            switch (drawType)
            {
                case DrawType.Start:
                    material[0].color = new Color(1f, 0f, 0f);
                    break;
                case DrawType.Arrive:
                    material[0].color = new Color(0.647f, 0.165f, 0.165f);
                    break;
            }
        }
    }
    public List<DrawTile> AroundTile = new List<DrawTile>();
    public List<DrawTile> ConnectStartTiles = new List<DrawTile>();
    public DrawTile ConnectTile { get; set; }
    public DrawTile UnderTile { get; set; }
    public Vector3 ConnectPos { get; set; }
    public Vector3 InitPos { get; set; }

    public int connectCount = 0;
    public bool isDraw = false;
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
        drawType = DrawType.None;
        foreach (var mater in material)
        {
            mater.color = new Color(1f, 1f, 1f, 0.3f);
        }
    }

    public void Draw(int layer)
    {
        IsDraw = true;
        this.layer = layer;
    }

    public virtual void Undo()
    {
        IsDraw = false;
        AroundTile.Clear();
        layer = -1;
    }

    public void UpdateDrawTile(Map.DrawData data)
    {
        transform.position = data.Position;
        transform.eulerAngles = data.Rotation;
        DrawType = data.DrawType;
        ConnectPos = data.ConnectTile;
        InitPos = data.Position;

        Vector3 dir = ConnectPos - transform.position;
        if(DrawType != DrawType.None)
        {
            transform.position += dir * 0.28f;
        }
    }

    public void SetActive(bool isActive)
    {
        if (isActive)
        {
            if (IsDraw)
            {
                foreach (var mater in material)
                {
                    mater.color = new Color(1f, 1f, 1f, 1f);
                }
            }
        }
        else
        {
            if(IsDraw)
            {
                foreach (var mater in material)
                {
                    mater.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
                }
            }
        }
    }
}
