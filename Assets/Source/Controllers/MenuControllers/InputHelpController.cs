using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputHelpController : MonoBehaviour
{
    [SerializeField] public ControlAction InputAction;
    [SerializeField] public Image SpriteDisplay;

    private iMessageClient c_messageClient;

    // Start is called before the first frame update
    void Start()
    {
        InitializeSpriteDisplay();
        this.c_messageClient = new InputHelpPromptMessageClient(this);
        MessageServer.Subscribe(ref c_messageClient, MessageID.EDIT_DISPLAY_UPDATE);
        MessageServer.Subscribe(ref c_messageClient, MessageID.INPUT_TYPE_CHANGED);
    }

    private void InitializeSpriteDisplay()
    {
        InputSpriteController.getInputSprite(out Sprite spriteOut, 
            GlobalInputController.GetInputKey(InputAction, GlobalInputController.GetActiveControllerType()),
            GlobalInputController.GetActiveControllerType());
        SpriteDisplay.sprite = spriteOut;
    }
}
