using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputEditController : iEditController
{
    public ControlAction InputAction;
    public Image SpriteDisplay;
    public InputMutex InputMutex;
    public InputType InputType;

    private Sprite spriteOut;
    private iMessageClient c_messageClient;

    public override void CancelDataEdit()
    {
        MessageServer.SendMessage(MessageID.EDIT_END, new Message());
        c_controllerData.k = GlobalInputController.GetInputKey(InputAction);
        Deactivate();
    }

    public override void ConfirmDataEdit(DataTarget targetIn)
    {
        MessageServer.SendMessage(MessageID.EDIT_END, new Message());
        MessageServer.SendMessage(MessageID.EDIT_DISPLAY_UPDATE, new Message((int)c_controllerData.k, (uint)InputAction));
        GlobalInputController.UpdateAction(InputAction, c_controllerData.k);
        GlobalGameData.SetActionSetting(InputAction, c_controllerData.k, this.InputType);
        Deactivate();
    }

    public virtual void EnginePush()
    {

    }

    public override void InitializeData()
    {
        c_controllerData = new EditControllerData();
        c_controllerData.b_editorActive = false;
        c_controllerData.k = GlobalGameData.GetActionSetting(InputAction, this.InputType);
        c_messageClient = new InputEditMessageClient(this);
        MessageServer.Subscribe(ref c_messageClient, MessageID.EDIT_SWAP);
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeData();
        InitializeStateMachine();
        if (InputSpriteController.getInputSprite(out spriteOut, c_controllerData.k, this.InputType))
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

        if (GlobalInputController.GetInputAction(ControlAction.CONFIRM, KeyValue.IDLE))
        {
            GlobalInputController.StartWatchForAnyInput();
            SpriteDisplay.sprite = InputSpriteController.EmptySprite();
        }

        KeyCode keyIn = GlobalInputController.GetAnyKey();
        if (keyIn == KeyCode.Escape)
        {
            GlobalInputController.StopWatchForAnyInput();
            CancelDataEdit();
            if (InputSpriteController.getInputSprite(out spriteOut, c_controllerData.k))
            {
                SpriteDisplay.sprite = spriteOut;
            }
        }

        else if (keyIn != KeyCode.None)
        {
            GlobalInputController.StopWatchForAnyInput();
            VerifyAndUpdateMutex(keyIn);
            c_controllerData.k = keyIn;
            ConfirmDataEdit(CurrentTarget);
            if (InputSpriteController.getInputSprite(out spriteOut, c_controllerData.k))
            {
                SpriteDisplay.sprite = spriteOut;
            }
        }


        // no state machine needed, handled by isActive
        EnginePush();
    }

    /* Checks if the key is in the mutex or not, as certain inputs cannot be overlapped
     * 
     */ 
    private void VerifyAndUpdateMutex(KeyCode keyIn)
    {
        // if the current keycode is bound to an existing action
        List<ControlAction> foundActions = GlobalInputController.GetActionForKey(keyIn);
        foreach (ControlAction controlAction in foundActions)
        {
            if (this.InputMutex.MutuallyExclusiveActions.Contains(controlAction)
                && controlAction != InputAction) // verify that we are not infinitely swapping the current action
            {
                MessageServer.SendMessage(MessageID.EDIT_SWAP, new Message((int)c_controllerData.k, (uint)controlAction));
            }
        }
    }

    public void InputUnbind()
    {
        c_controllerData.k = KeyCode.None;

        GlobalInputController.UpdateAction(InputAction, c_controllerData.k);
        GlobalGameData.SetActionSetting(InputAction, c_controllerData.k);

        if (InputSpriteController.getInputSprite(out spriteOut, c_controllerData.k))
        {
            SpriteDisplay.sprite = spriteOut;
        }
    }

    // TODO: swapping a key with pause will not also update the "other mutex" buttons, allowing for a multiple mapping edge case
    public void InputSwap(KeyCode keyIn)
    {
        c_controllerData.k = keyIn;

        GlobalInputController.UpdateAction(InputAction, c_controllerData.k);
        GlobalGameData.SetActionSetting(InputAction, c_controllerData.k);

        if (InputSpriteController.getInputSprite(out spriteOut, c_controllerData.k))
        {
            SpriteDisplay.sprite = spriteOut;
        }
    }
}
