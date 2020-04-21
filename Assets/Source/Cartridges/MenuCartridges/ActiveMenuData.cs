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
}
