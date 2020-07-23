using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private List<iMenuItemController> MenuItems;
    [SerializeField] private BasicMenuControllerData ControllerData;
    private StateMachine sm_menuInput;
    private iMenuItemController c_activeMenuItem;
    private ActiveMenuData c_activeMenuData;
    private IncrementCartridge cart_incr;
    

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
    }

    void Update()
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


        if (GlobalInputController.GetInputValue(GlobalInputController.ControllerData.DTrickButton) == KeyValue.PRESSED)
        {
            c_activeMenuItem.ExecuteMenuCommand();
        }

        UpdateStateMachine();

        sm_menuInput.Act();

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

    private void InitializeData()
    {
        c_activeMenuData = new ActiveMenuData();

        c_activeMenuData.f_currentMenuTickCount = 0.0f;
        c_activeMenuData.f_currentMenuWaitCount = ControllerData.ShortTickTime;
        c_activeMenuData.i_activeMenuItemIndex = i_activeMenuItemIndex;
        c_activeMenuData.i_menuItemCount = MenuItems.Count;
        c_activeMenuData.i_menuDir = 0;
        c_activeMenuData.b_showMenu = true;
    }

    /// <summary>
    /// Initialize the menu's state machine and cartridges, by creating new cartridges, 
    /// providing each state with access to the necessary cartridges and organizing the states into the machine
    /// </summary>
    private void InitializeStateMachine()
    {
        cart_incr = new IncrementCartridge();

        MenuReadyState s_ready = new MenuReadyState(ref c_activeMenuData);
        MenuWaitState s_wait = new MenuWaitState(ref ControllerData, ref c_activeMenuData, ref cart_incr);
        MenuTickState s_tick = new MenuTickState(ref c_activeMenuData, ref cart_incr);

        sm_menuInput = new StateMachine(s_ready, StateRef.MENU_READY);
        sm_menuInput.AddState(s_wait, StateRef.MENU_WAIT);
        sm_menuInput.AddState(s_tick, StateRef.MENU_TICK);
    }

    private void SetMenuDefaults()
    {
        MenuSelectionData.SetNextScene(1);
    }
}
 