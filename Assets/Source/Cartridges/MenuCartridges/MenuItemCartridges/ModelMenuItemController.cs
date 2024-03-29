﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelMenuItemController : iMenuItemController, iEntityController
{
    [SerializeField] private MenuCommand MenuAction;
    [SerializeField] CharacterSelection currentCharacter;
    [SerializeField] CharacterAttributeData attributes;

    private StateMachine sm_menuItem;

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
        // sm_menuItem.Act();
        EngineUpdate();
    }

    public void EngineUpdate()
    {
        //
    }

    public void EnginePull()
    {
        //
    }

    public void UpdateStateMachine()
    {
        //
    }

    public override void ExecuteStateMachineCommand(Command cmd)
    {
        // sm_menuItem.Execute(cmd);
    }

    public override void ExecuteMenuCommand()
    {
        GlobalInputController.LockInput();
        switch (MenuAction)
        {
            case MenuCommand.EXIT_GAME:
                Application.Quit();
                break;
            case MenuCommand.RESUME:
                MessageServer.SendMessage(MessageID.PAUSE, new Message(0));
                break;
            case MenuCommand.CONFIRM:
                break;
            case MenuCommand.UPDATE_GAME_DATA:
                GlobalGameData.playerOneCharacter = currentCharacter;
                MessageServer.SendMessage(MessageID.CHARACTER_SELECTED, new Message((uint)PlayerID.PLAYER1));
                break;
            case MenuCommand.MENU_BACK:
                MessageServer.SendMessage(MessageID.MENU_BACK, new Message());
                break;
        }
    }

    public override void InitializeStateMachine()
    {

    }

    public override void InitializeData()
    {

    }
}
