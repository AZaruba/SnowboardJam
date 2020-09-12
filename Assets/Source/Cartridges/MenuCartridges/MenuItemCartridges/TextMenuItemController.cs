using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextMenuItemController : iMenuItemController, iEntityController
{
    [SerializeField] private BasicMenuItemData ItemData;
    [SerializeField] private RectTransform ItemTransform;
    [SerializeField] private Text ItemText;
    [SerializeField] private MenuCommand MenuAction;
    [SerializeField] private Scene NextSceneId;
    [SerializeField] private DataTarget DataItem;
    [SerializeField] private MenuController ChildMenuController;
    [SerializeField] private iEditController ChildEditController;

    private MenuItemActiveData c_itemActiveData;

    private StateMachine sm_menuItem;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeData();
        InitializeStateMachine();
    }

    // Update is called once per frame
    void Update()
    {
        EnginePull();
        UpdateStateMachine();
        sm_menuItem.Act();
        EngineUpdate();
    }

    public void EngineUpdate()
    {
        ItemTransform.anchoredPosition = c_itemActiveData.v_itemPosition;
        ItemText.color = c_itemActiveData.c_currentColor;
    }

    public void EnginePull()
    {

    }

    public void UpdateStateMachine()
    {
        // state machine only updates on parent call
        if (Vector2.Distance(c_itemActiveData.v_targetItemPosition, c_itemActiveData.v_itemPosition) < Constants.VECTOR_2_TOLERANCE)
        {
            sm_menuItem.Execute(Command.END_TRANSITION);
        }
    }

    public override void ExecuteStateMachineCommand(Command cmd)
    {
        sm_menuItem.Execute(cmd);
    }

    public override void ExecuteMenuCommand()
    {
        GlobalInputController.LockInput();
        switch (MenuAction)
        {
            case MenuCommand.EXIT_GAME:
                Application.Quit();
                break;
            case MenuCommand.RESTART:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
                break;
            case MenuCommand.CHANGE_SCENE:
                if (c_itemActiveData.i_nextScene > -1)
                {
                    MessageServer.OnSceneChange();
                    SceneManager.LoadScene(c_itemActiveData.i_nextScene, LoadSceneMode.Single);
                }
                break;
            case MenuCommand.RESUME:
                MessageServer.SendMessage(MessageID.PAUSE, new Message(0));
                break;
            case MenuCommand.MENU_BACK:
                MessageServer.SendMessage(MessageID.MENU_BACK, new Message());
                break;
            case MenuCommand.MENU_FORWARD:
                if (ChildMenuController == null)
                {
                    return;
                }
                MessageServer.SendMessage(MessageID.MENU_FORWARD, new Message(ChildMenuController.GetSuperMenuIndex()));
                break;
            case MenuCommand.CONFIRM:
                break;
            case MenuCommand.EDIT_DATA:
                if (DataItem == DataTarget.ERROR_TARGET)
                {
                    return;
                }
                if (ChildEditController == null)
                {
                    return;
                }
                // open data editor, wait for return
                ChildEditController.Activate(DataItem);
                MessageServer.SendMessage(MessageID.EDIT_START, new Message());
                break;
        }
    }

    public override void InitializeStateMachine()
    {
        LerpCartridge cart_lerp = new LerpCartridge();

        SelectedState s_selected = new SelectedState(ref ItemData, ref c_itemActiveData);
        PostselectedState s_postSelected = new PostselectedState(ref ItemData, ref c_itemActiveData, ref cart_lerp);
        UnselectedState s_unselected = new UnselectedState(ref ItemData, ref c_itemActiveData);
        PreselectedState s_preselected = new PreselectedState(ref ItemData, ref c_itemActiveData, ref cart_lerp);

        sm_menuItem = new StateMachine(s_unselected, StateRef.ITEM_UNSELECTED);
        sm_menuItem.AddState(s_postSelected, StateRef.ITEM_POSTSELECTED);
        sm_menuItem.AddState(s_selected, StateRef.ITEM_SELECTED);
        sm_menuItem.AddState(s_preselected, StateRef.ITEM_PRESELECTED);
    }

    public override void InitializeData()
    {
        c_itemActiveData = new MenuItemActiveData();
        c_itemActiveData.v_itemPosition = ItemTransform.anchoredPosition;
        c_itemActiveData.v_targetItemPosition = ItemTransform.anchoredPosition;
        c_itemActiveData.v_origin = ItemTransform.anchoredPosition;
        c_itemActiveData.c_currentColor = ItemData.UnselectedColor;

        c_itemActiveData.i_nextScene = (int)NextSceneId;
    }
}
