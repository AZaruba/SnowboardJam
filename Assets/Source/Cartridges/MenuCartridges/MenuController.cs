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

    private iMessageClient c_menuClient;
    private iMenuItemController c_activeMenuItem;
    private ActiveMenuData c_activeMenuData;
    private LastFrameActiveMenuData c_lastFrameData;
    private IncrementCartridge cart_incr;
    private LerpCartridge cart_lerp;

    private int i_superMenuIndex;
    private int i_activeMenuItemIndex;

    void Start()
    {
        SetMenuDefaults();
        InitializeData();
        InitializeStateMachine();
        InitializeMessageClient();

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
        rectTransform.anchoredPosition = Utils.InterpolateFixedVector(c_lastFrameData.v_lastFramePosition, c_activeMenuData.v_currentPosition);
    }

    void FixedUpdate()
    {
        c_lastFrameData.v_lastFramePosition = c_activeMenuData.v_currentPosition;
        sm_menuInput.Act();
        sm_showHide.Act();
    }

    public void UpdateMenu()
    {
        if (c_activeMenuData.b_editorActive)
        {
            c_activeMenuItem.UpdateEditor();
            return;
        }

        if (c_activeMenuData.b_menuActive == false)
        {
            return;
        }

        c_activeMenuData.i_menuMousePositionItemIndex = -1;
        CheckMouseInput();

        float inputAxisValue = GlobalInputController.GetAnalogInputAction(ControlAction.FLIP_AXIS);
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

        if (!CheckForConfirmation())
        {
            return;
        }

        if (GlobalInputController.GetInputAction(ControlAction.BACK, KeyValue.PRESSED))
        {
            // return to previous menu
            c_activeMenuItem.MenuCommandBack();
        }

        if (GlobalInputController.GetInputAction(ControlAction.CONFIRM, KeyValue.PRESSED) ||
            c_activeMenuData.b_menuItemClicked)
        {
            c_activeMenuItem.ExecuteMenuCommand();
        }

        UpdateStateMachine();

        SetActiveMenuItemIndex();
    }

    public void CheckMouseInput()
    {
        c_activeMenuData.b_menuItemClicked = false;
        for (int i = 0; i < MenuItems.Count; i++)
        {
            iMenuItemController itemController = MenuItems[i];
            if (GlobalMouseInputController.MouseOverItem(itemController.ItemTransform))
            {
                c_activeMenuData.i_menuMousePositionItemIndex = i;
                c_activeMenuData.b_menuItemClicked = GlobalMouseInputController.GetMouseClick() == KeyValue.UP;
            }
        }
    }

    public virtual bool CheckForConfirmation()
    {
        if (!c_activeMenuData.b_menuConfirmActive)
        {
            if (GlobalInputController.GetInputAction(ControlAction.CONFIRM, KeyValue.IDLE) &&
                GlobalInputController.GetInputAction(ControlAction.BACK, KeyValue.IDLE))
            {
                c_activeMenuData.b_menuConfirmActive = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public void SetActiveMenuItemIndex()
    {
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
        if (c_activeMenuData.i_menuMousePositionItemIndex >= 0)
        {
            sm_menuInput.Execute(Command.MENU_MOUSE_INPUT);
        }
        else if (c_activeMenuData.i_menuDir != 0)
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

    public void UpdateHelpTextOnBack()
    {
        c_activeMenuItem.OnItemActive();
    }

    private void InitializeData()
    {
        c_activeMenuData = new ActiveMenuData();
        c_lastFrameData = new LastFrameActiveMenuData(ControllerData.DisabledPosition);

        c_activeMenuData.f_currentMenuTickCount = Constants.ZERO_F;
        c_activeMenuData.f_currentMenuWaitCount = ControllerData.ShortTickTime;
        c_activeMenuData.i_activeMenuItemIndex = i_activeMenuItemIndex;
        c_activeMenuData.i_menuItemCount = MenuItems.Count;
        c_activeMenuData.i_menuDir = Constants.ZERO;
        c_activeMenuData.b_showMenu = false;
        c_activeMenuData.b_menuActive = true;
        c_activeMenuData.v_currentPosition = ControllerData.DisabledPosition;
        c_activeMenuData.v_targetPosition = ControllerData.DisabledPosition;
        c_activeMenuData.i_menuMousePositionItemIndex = -1;
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
        MenuTickState s_tick = new MenuTickState(ref c_activeMenuData);
        MenuJumpState s_jump = new MenuJumpState(ref c_activeMenuData);

        MenuHideState s_hide = new MenuHideState(ref c_activeMenuData, ref ControllerData, ref cart_lerp);
        MenuShowState s_show = new MenuShowState(ref c_activeMenuData, ref ControllerData, ref cart_lerp);

        sm_menuInput = new StateMachine(s_ready, StateRef.MENU_READY);
        sm_menuInput.AddState(s_jump, StateRef.MENU_MOUSE);
        sm_menuInput.AddState(s_wait, StateRef.MENU_WAIT);
        sm_menuInput.AddState(s_tick, StateRef.MENU_TICK);
        sm_menuInput.AddState(s_disabled, StateRef.MENU_DISABLED);

        sm_showHide = new StateMachine(s_hide, StateRef.MENU_HIDDEN);
        sm_showHide.AddState(s_show, StateRef.MENU_SHOWN);
    }

    private void InitializeMessageClient()
    {
        c_menuClient = new MenuMessageClient(ref c_activeMenuData);
        MessageServer.Subscribe(ref c_menuClient, MessageID.EDIT_START);
        MessageServer.Subscribe(ref c_menuClient, MessageID.EDIT_END);
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
 