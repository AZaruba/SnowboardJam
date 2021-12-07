using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMessageClient : iMessageClient
{
    // should we just store the message then let classes access that directly?
    ClientID clientID;
    private ActiveMenuData data_out;

    public MenuMessageClient(ref ActiveMenuData menuData)
    {
        clientID = ClientID.PAUSE_MENU_CLIENT;
        data_out = menuData;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        if (id == MessageID.EDIT_START)
        {
            data_out.b_editorActive = true;
            data_out.b_menuActive = false;
            data_out.b_menuConfirmActive = false;
            return true;
        }
        if (id == MessageID.EDIT_END)
        {
            data_out.b_menuActive = true;
            data_out.b_editorActive = false;
            data_out.b_menuConfirmActive = false;
            return true;
        }
        return false;
    }
}

public class InputHelpPromptMessageClient : iMessageClient
{
    InputHelpController parent;
    InputType activeType;

    public InputHelpPromptMessageClient(InputHelpController parentIn)
    {
        this.parent = parentIn;
        activeType = InputType.KEYBOARD_GENERIC;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        if (id == MessageID.EDIT_DISPLAY_UPDATE)
        {
            if (message.getInputType() != activeType)
            {
                return false;
            }

            if (parent.InputAction == (ControlAction)message.getUint())
            {
                if (InputSpriteController.getInputSprite(out Sprite spriteOut, (KeyCode)message.getInt(), message.getInputType()))
                {
                    parent.SpriteDisplay.sprite = spriteOut;
                    return true;
                }
                return false;
            }
        }
        if (id == MessageID.INPUT_TYPE_CHANGED)
        {
            this.activeType = message.getInputType();
            if (InputSpriteController.getInputSprite(out Sprite spriteOut,
                                                     GlobalInputController.GetInputKey(parent.InputAction, message.getInputType()),
                                                     message.getInputType()))
            {
                parent.SpriteDisplay.sprite = spriteOut;
                return true;
            }
            return false;
        }
        return false;
    }
}

public class InputEditMessageClient : iMessageClient
{
    InputEditController parent;

    public InputEditMessageClient(InputEditController parentIn)
    {
        this.parent = parentIn;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        if (id == MessageID.EDIT_SWAP)
        {
            if (parent.InputAction == (ControlAction)message.getUint())
            {
                parent.InputUnbind();
            }
        }
        if (id == MessageID.EDIT_RESET)
        {
            if (parent.InputAction == (ControlAction)message.getUint() && parent.InputType == message.getInputType())
            {
                parent.InputSwap((KeyCode)message.getInt());
            }
        }
            
        return false;
    }
}