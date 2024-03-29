﻿using System.Collections;
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
    private bool MenuActive;
    private bool EditorActive;
    public bool b_menuConfirmActive;

    private Vector2 CurrentPosition;
    private Vector2 LastFramePosition;
    private Vector2 TargetPosition;
    private float CurrentOpacity;

    public int i_menuMousePositionItemIndex;
    public bool b_menuItemClicked;

    public bool b_showMenu
    {
        get { return ShowMenu; }
        set { ShowMenu = value; }
    }

    public bool b_menuActive
    {
        get { return MenuActive; }
        set { MenuActive = value; }
    }

    public bool b_editorActive
    {
        get { return EditorActive; }
        set { EditorActive = value; }
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

    public Vector2 v_lastFramePosition
    {
        get { return LastFramePosition; }
        set { LastFramePosition = value; }
    }

    public Vector2 v_targetPosition
    {
        get { return TargetPosition; }
        set { TargetPosition = value; }
    }

    public float f_currentOpacity
    {
        get { return CurrentOpacity; }
        set { CurrentOpacity = value; }
    }

    public ActiveMenuData()
    {
        b_menuConfirmActive = false;
    }
}

public class LastFrameActiveMenuData
{
    public Vector3 v_lastFramePosition;

    public LastFrameActiveMenuData(Vector3 posIn)
    {
        v_lastFramePosition = posIn;
    }
}
