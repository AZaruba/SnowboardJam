﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageMenuItemController : iMenuItemController, iEntityController
{
    [SerializeField] private BasicMenuItemData ItemData;
    [SerializeField] private RectTransform ItemTransform;
    [SerializeField] private Image ItemImage;
    [SerializeField] private MenuCommand MenuAction;
    [SerializeField] private Scene NextSceneId;

    private StateMachine sm_menuItem;
    private MenuItemActiveData c_itemActiveData;
    private MenuItemLastFrameData c_lastFrameData;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeData();
        InitializeStateMachine();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        EnginePull();
        UpdateStateMachine();
        sm_menuItem.Act();
    }

    void Update()
    {
        EngineUpdate();
    }

    public void EngineUpdate()
    {
        ItemTransform.anchoredPosition = Utils.InterpolateFixedVector(c_lastFrameData.v_lastFramePosition, c_itemActiveData.v_itemPosition);
        ItemImage.color = Utils.InterpolateFixedColor(c_lastFrameData.c_lastFrameColor, c_itemActiveData.c_currentColor);
    }

    public void EnginePull()
    {
        c_lastFrameData.v_lastFramePosition = c_itemActiveData.v_itemPosition;
        c_lastFrameData.c_lastFrameColor = c_itemActiveData.c_currentColor;
    }

    public void UpdateStateMachine()
    {
        // state machine only updates on parent call
        if (Vector2.Distance(c_itemActiveData.v_targetItemPosition, c_itemActiveData.v_itemPosition) < Constants.VECTOR_2_TOLERANCE)
        {
            sm_menuItem.Execute(Command.END_TRANSITION);
        }
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
                if (c_itemActiveData.i_nextScene > -1 && c_itemActiveData.i_nextScene < SceneManager.sceneCountInBuildSettings)
                {
                    MessageServer.OnSceneChange();
                    SceneManager.LoadScene(c_itemActiveData.i_nextScene, LoadSceneMode.Single);
                }
                break;
            case MenuCommand.RESUME:
                MessageServer.SendMessage(MessageID.PAUSE, new Message(0));
                break;
            case MenuCommand.CONFIRM:
                break;
        }
    }

    public override void ExecuteStateMachineCommand(Command cmd)
    {
        sm_menuItem.Execute(cmd);
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

        c_lastFrameData = new MenuItemLastFrameData(c_itemActiveData.v_itemPosition, c_itemActiveData.c_currentColor);

        c_itemActiveData.i_nextScene = (int)NextSceneId;
    }
}
