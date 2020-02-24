using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItemActiveData
{
    private Vector2 ItemPosition;
    private Vector2 TargetItemPosition;
    private Color CurrentColor;

    private Vector2 OriginPosition;
    private MenuCommand Command;

    public Vector2 v_itemPosition
    {
        get { return ItemPosition; }
        set { ItemPosition = value; }
    }

    public Vector2 v_targetItemPosition
    {
        get { return TargetItemPosition; }
        set { TargetItemPosition = value; }
    }

    public Vector2 v_origin
    {
        get { return OriginPosition; }
        set { OriginPosition = value; }
    }

    public MenuCommand c_menuCommand
    {
        get { return Command; }
        set { Command = value; }
    }

    public Color c_currentColor
    {
        get { return CurrentColor; }
        set { CurrentColor = value; }
    }
}
