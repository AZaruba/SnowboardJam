using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private List<MenuItemController> l_menuItems;
    [SerializeField] private ControllerInputData keyList;
    [SerializeField] private BasicMenuControllerData ControllerData;
    private StateMachine sm_menuInput;
    private MenuItemController c_activeMenuItem;
    private ActiveMenuData c_activeMenuData;
    private IncrementCartridge cart_incr;

    private int i_activeMenuItemIndex;

    void Start()
    {
        SetMenuDefaults();
        InitializeData();
        InitializeStateMachine();

        i_activeMenuItemIndex = 0;
        c_activeMenuItem = l_menuItems[i_activeMenuItemIndex];
        c_activeMenuItem.ExecuteStateMachineCommand(Command.SELECT);
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.W))
        {
            c_activeMenuItem.ExecuteStateMachineCommand(Command.UNSELECT);

            // bug fix: modulo negative numbers produces an incorrect result
            if (i_activeMenuItemIndex == 0)
            {
                i_activeMenuItemIndex = l_menuItems.Count - 1;
            }
            else
            {
                i_activeMenuItemIndex = Mathf.Abs((i_activeMenuItemIndex - 1) % l_menuItems.Count);
            }
            c_activeMenuItem = l_menuItems[i_activeMenuItemIndex];
            c_activeMenuItem.ExecuteStateMachineCommand(Command.SELECT);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            c_activeMenuItem.ExecuteStateMachineCommand(Command.UNSELECT);
            i_activeMenuItemIndex = Mathf.Abs((i_activeMenuItemIndex + 1) % l_menuItems.Count);
            c_activeMenuItem = l_menuItems[i_activeMenuItemIndex];
            c_activeMenuItem.ExecuteStateMachineCommand(Command.SELECT);
        }
        */
        float inputAxisValue = GlobalInputController.GetInputValue(keyList.LeftVerticalAxis);
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


        if (GlobalInputController.GetInputValue(keyList.DTrickButton) == KeyValue.PRESSED)
        {
            c_activeMenuItem.ExecuteMenuCommand();
        }

        UpdateStateMachine();

        sm_menuInput.Act();

        if (i_activeMenuItemIndex != c_activeMenuData.i_activeMenuItemIndex)
        {
            c_activeMenuItem.ExecuteStateMachineCommand(Command.UNSELECT);
            i_activeMenuItemIndex = c_activeMenuData.i_activeMenuItemIndex;
            c_activeMenuItem = l_menuItems[i_activeMenuItemIndex];
            c_activeMenuItem.ExecuteStateMachineCommand(Command.SELECT);
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
        c_activeMenuData.i_menuItemCount = l_menuItems.Count;
        c_activeMenuData.i_menuDir = 0;
    }

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

/* TODO / Improving the menu controller
 * 1. Get rid of hardcoded axes and input
 * 3. Get rid of constants
 * 5. grid based menus
 */
 