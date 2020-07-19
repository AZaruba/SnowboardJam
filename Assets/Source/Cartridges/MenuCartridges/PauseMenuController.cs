using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private List<TextMenuItemController> MenuItems;
    [SerializeField] private BasicMenuControllerData ControllerData;
    [SerializeField] private RectTransform rectTransform;
    private StateMachine sm_menuInput;
    private StateMachine sm_pauseMenu;

    private iMenuItemController c_activeMenuItem;
    private ActiveMenuData c_activeMenuData;
    private IncrementCartridge cart_incr;
    private LerpCartridge cart_lerp;

    private iMessageClient c_messageClient;

    private int i_activeMenuItemIndex;

    void Start()
    {
        SetMenuDefaults();
        InitializeData();
        InitializeStateMachine();

        i_activeMenuItemIndex = 0;
        c_activeMenuItem = MenuItems[i_activeMenuItemIndex];
        c_activeMenuItem.ExecuteStateMachineCommand(Command.SELECT);
    }

    void Update()
    {
        rectTransform.anchoredPosition = c_activeMenuData.v_currentPosition;
        if (c_activeMenuData.b_showMenu == true)
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
        }

        UpdateStateMachine();

        sm_menuInput.Act();
        sm_pauseMenu.Act();

        if (i_activeMenuItemIndex != c_activeMenuData.i_activeMenuItemIndex)
        {
            c_activeMenuItem.ExecuteStateMachineCommand(Command.UNSELECT);
            i_activeMenuItemIndex = c_activeMenuData.i_activeMenuItemIndex;
            c_activeMenuItem = MenuItems[i_activeMenuItemIndex];
            c_activeMenuItem.ExecuteStateMachineCommand(Command.SELECT);
        }
    }

    void UpdateStateMachine()
    {
        if (!c_activeMenuData.b_showMenu)
        {
            sm_menuInput.Execute(Command.MENU_HIDE);
            sm_pauseMenu.Execute(Command.MENU_HIDE);
        }
        else
        {
            sm_menuInput.Execute(Command.MENU_SHOW);
            sm_pauseMenu.Execute(Command.MENU_SHOW);
        }
        if (float.Equals(c_activeMenuData.f_currentMenuTickCount, c_activeMenuData.f_currentMenuWaitCount))
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
        c_activeMenuData.b_showMenu = false;
        c_activeMenuData.v_currentPosition = rectTransform.anchoredPosition;
        c_activeMenuData.v_targetPosition = rectTransform.anchoredPosition;

        c_messageClient = new PauseMenuMessageClient(ref c_activeMenuData);
        MessageServer.Subscribe(ref c_messageClient, MessageID.PAUSE);
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
        MenuHideState s_hidden = new MenuHideState(ref c_activeMenuData, ref cart_lerp);
        MenuShowState s_shown = new MenuShowState(ref c_activeMenuData, ref cart_lerp);

        MenuReadyState s_ready = new MenuReadyState(ref c_activeMenuData);
        MenuWaitState s_wait = new MenuWaitState(ref ControllerData, ref c_activeMenuData, ref cart_incr);
        MenuTickState s_tick = new MenuTickState(ref c_activeMenuData, ref cart_incr);

        sm_menuInput = new StateMachine(s_disabled, StateRef.MENU_DISABLED);
        sm_menuInput.AddState(s_ready, StateRef.MENU_READY);
        sm_menuInput.AddState(s_wait, StateRef.MENU_WAIT);
        sm_menuInput.AddState(s_tick, StateRef.MENU_TICK);

        sm_pauseMenu = new StateMachine(s_hidden, StateRef.MENU_HIDDEN);
        sm_pauseMenu.AddState(s_shown, StateRef.MENU_SHOWN);
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
