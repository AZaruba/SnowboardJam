using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputEditController : iEditController
{
    public ControlAction InputAction;
    public Image SpriteDisplay;

    private Sprite spriteOut;

    public override void CancelDataEdit()
    {
        MessageServer.SendMessage(MessageID.EDIT_END, new Message());
        c_controllerData.k = GlobalInputController.GetInputKey(InputAction);
        Deactivate();
    }

    public override void ConfirmDataEdit(DataTarget targetIn)
    {
        MessageServer.SendMessage(MessageID.EDIT_END, new Message());
        EnginePush();
        GlobalInputController.UpdateAction(InputAction, c_controllerData.k);
        GlobalGameData.SetActionSetting(InputAction, c_controllerData.k);
        Deactivate();
    }

    public virtual void EnginePush()
    {
        if (InputSpriteController.getInputSprite(out spriteOut, c_controllerData.k, InputType.KEYBOARD_WIN))
        {
            SpriteDisplay.sprite = spriteOut;
        }
    }

    public override void InitializeData()
    {
        c_controllerData = new EditControllerData();
        c_controllerData.b_editorActive = false;
        c_controllerData.k = GlobalGameData.GetActionSetting(InputAction);
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeData();
        InitializeStateMachine();
        if (InputSpriteController.getInputSprite(out spriteOut, c_controllerData.k, InputType.KEYBOARD_WIN))
        {
            SpriteDisplay.sprite = spriteOut;

        }
    }

    // Update is called once per frame
    public override void UpdateEditor()
    {
        if (c_controllerData.b_editorActive == false)
        {
            return;
        }

        if (GlobalInputController.GetInputAction(ControlAction.CONFIRM) == KeyValue.IDLE)
        {
            GlobalInputController.StartWatchForAnyInput();
        }

        KeyCode keyIn = GlobalInputController.GetAnyKey();
        if (keyIn == KeyCode.Escape)
        {
            GlobalInputController.StopWatchForAnyInput();
            CancelDataEdit();
        }

        else if (keyIn != KeyCode.None)
        {
            GlobalInputController.StopWatchForAnyInput();
            c_controllerData.k = keyIn;
            ConfirmDataEdit(CurrentTarget);
        }

        // no state machine needed, handled by isActive
        EnginePush();
    }
}
