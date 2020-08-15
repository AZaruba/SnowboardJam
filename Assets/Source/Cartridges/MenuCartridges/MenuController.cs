using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private List<iMenuItemController> MenuItems;
    [SerializeField] private BasicMenuControllerData ControllerData;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private bool IsDefault;

    private StateMachine sm_menuInput;
    private StateMachine sm_showHide;

    private iMenuItemController c_activeMenuItem;
    private ActiveMenuData c_activeMenuData;
    private IncrementCartridge cart_incr;
    private LerpCartridge cart_lerp;

    private int i_superMenuIndex;
    private int i_activeMenuItemIndex;

    void Start()
    {
        SetMenuDefaults();
        InitializeData();
        InitializeStateMachine();

        i_activeMenuItemIndex = 0;
        c_activeMenuItem = MenuItems[i_activeMenuItemIndex];
        c_activeMenuItem.ExecuteStateMachineCommand(Command.SELECT);
        c_activeMenuItem.OnItemActive(i_activeMenuItemIndex);

        if (IsDefault)
        {
            ShowMenu();
        }
    }

    void Update()
    {
        rectTransform.anchoredPosition = c_activeMenuData.v_currentPosition;
        sm_menuInput.Act();
        sm_showHide.Act();
    }

    public void UpdateMenu()
    {
        float inputAxisValue = GlobalInputController.GetInputValue(GlobalInputController.ControllerData.LeftVerticalAxis);
        if (inputAxisValue > 0.5f)
        {
            c_activeMenuData.i_menuDir = -1; // menus are often organized top to bottom
        }
        else if (inputAxisValue < -0.5f)
        {
            c_activeMenuData.i_menuDir = 1;
        }
        else
        {
            c_activeMenuData.i_menuDir = 0;
        }

        if (GlobalInputController.GetInputValue(GlobalInputController.ControllerData.RTrickButton) == KeyValue.PRESSED)
        {
            // return to previous menu
            c_activeMenuItem.MenuCommandBack();
        }

        if (GlobalInputController.GetInputValue(GlobalInputController.ControllerData.DTrickButton) == KeyValue.PRESSED)
        {
            c_activeMenuItem.ExecuteMenuCommand();
        }

        UpdateStateMachine();

        if (i_activeMenuItemIndex != c_activeMenuData.i_activeMenuItemIndex)
        {
            c_activeMenuItem.ExecuteStateMachineCommand(Command.UNSELECT);
            i_activeMenuItemIndex = c_activeMenuData.i_activeMenuItemIndex;
            c_activeMenuItem = MenuItems[i_activeMenuItemIndex];
            c_activeMenuItem.ExecuteStateMachineCommand(Command.SELECT);
            c_activeMenuItem.OnItemActive(i_activeMenuItemIndex);
        }
    }

    void UpdateStateMachine()
    {
        if (!c_activeMenuData.b_showMenu)
        {
            sm_menuInput.Execute(Command.MENU_HIDE);
            sm_showHide.Execute(Command.MENU_HIDE);
        }
        else
        {
            sm_menuInput.Execute(Command.MENU_SHOW);
            sm_showHide.Execute(Command.MENU_SHOW);
        }

        if (float.Equals(c_activeMenuData.f_currentMenuTickCount,c_activeMenuData.f_currentMenuWaitCount))
        {
            sm_menuInput.Execute(Command.MENU_READY);
        }
        else
        {
            sm_menuInput.Execute(Command.MENU_IDLE);
        }
        if (c_activeMenuData.i_menuDir != 0)
        {
            sm_menuInput.Execute(Command.MENU_TICK_INPUT);
        }
        else
        {
            sm_menuInput.Execute(Command.MENU_READY);
        }
    }

    public void HideMenu()
    {
        sm_showHide.Execute(Command.MENU_HIDE);
        sm_menuInput.Execute(Command.MENU_HIDE);
        c_activeMenuData.b_showMenu = false;
    }

    public void ShowMenu()
    {
        sm_showHide.Execute(Command.MENU_SHOW);
        sm_menuInput.Execute(Command.MENU_SHOW);
        c_activeMenuData.b_showMenu = true;
    }

    private void InitializeData()
    {
        c_activeMenuData = new ActiveMenuData();

        c_activeMenuData.f_currentMenuTickCount = 0.0f;
        c_activeMenuData.f_currentMenuWaitCount = ControllerData.ShortTickTime;
        c_activeMenuData.i_activeMenuItemIndex = i_activeMenuItemIndex;
        c_activeMenuData.i_menuItemCount = MenuItems.Count;
        c_activeMenuData.i_menuDir = 0;
        c_activeMenuData.b_showMenu = false;
        c_activeMenuData.v_currentPosition = ControllerData.DisabledPosition;
        c_activeMenuData.v_targetPosition = ControllerData.DisabledPosition;
    }

    /// <summary>
    /// Initialize the menu's state machine and cartridges, by creating new cartridges, 
    /// providing each state with access to the necessary cartridges and organizing the states into the machine
    /// </summary>
    private void InitializeStateMachine()
    {
        cart_incr = new IncrementCartridge();
        cart_lerp = new LerpCartridge();

        MenuDisabledState s_disabled = new MenuDisabledState();
        MenuReadyState s_ready = new MenuReadyState(ref c_activeMenuData);
        MenuWaitState s_wait = new MenuWaitState(ref ControllerData, ref c_activeMenuData, ref cart_incr);
        MenuTickState s_tick = new MenuTickState(ref c_activeMenuData, ref cart_incr);

        MenuHideState s_hide = new MenuHideState(ref c_activeMenuData, ref ControllerData, ref cart_lerp);
        MenuShowState s_show = new MenuShowState(ref c_activeMenuData, ref ControllerData, ref cart_lerp);

        sm_menuInput = new StateMachine(s_ready, StateRef.MENU_READY);
        sm_menuInput.AddState(s_wait, StateRef.MENU_WAIT);
        sm_menuInput.AddState(s_tick, StateRef.MENU_TICK);
        sm_menuInput.AddState(s_disabled, StateRef.MENU_DISABLED);

        sm_showHide = new StateMachine(s_hide, StateRef.MENU_HIDDEN);
        sm_showHide.AddState(s_show, StateRef.MENU_SHOWN);
    }

    private void SetMenuDefaults()
    {
        MenuSelectionData.SetNextScene(1);
        for (int i = 0; i < MenuItems.Count; i++)
        {
            MenuItems[i].SetParent(this);
        }
    }

    public void SetSuperMenuIndex(int indexIn)
    {
        this.i_superMenuIndex = indexIn;
    }

    public int GetSuperMenuIndex()
    {
        return i_superMenuIndex;
    }
}
 