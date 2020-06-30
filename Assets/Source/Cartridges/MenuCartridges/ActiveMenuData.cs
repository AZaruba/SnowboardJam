using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMenuData
{
    private int ActiveMenuItemIndex;
    private int MenuItemCount;
    private float CurrentMenuTickTime;
    private float CurrentMenuWaitTime;
    private int MenuDirection;

    private bool ShowMenu;

    private Vector2 CurrentPosition;
    private Vector2 TargetPosition;

    public bool b_showMenu
    {
        get { return ShowMenu; }
        set { ShowMenu = value; }
    }

    public int i_activeMenuItemIndex
    {
        get { return ActiveMenuItemIndex; }
        set { ActiveMenuItemIndex = value; }
    }

    public int i_menuItemCount
    {
        get { return MenuItemCount; }
        set { MenuItemCount = value; }
    }

    public int i_menuDir
    {
        get { return MenuDirection; }
        set { MenuDirection = value; }
    }
    public float f_currentMenuTickCount
    {
        get { return CurrentMenuTickTime; }
        set { CurrentMenuTickTime = value; }
    }

    public float f_currentMenuWaitCount
    {
        get { return CurrentMenuWaitTime; }
        set { CurrentMenuWaitTime = value; }
    }

    public Vector2 v_currentPosition
    {
        get { return CurrentPosition; }
        set { CurrentPosition = value; }
    }

    public Vector2 v_targetPosition
    {
        get { return TargetPosition; }
        set { TargetPosition = value; }
    }
}
