using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum InputType
{
    KEYBOARD_WIN = 0,
    KEYBOARD_MAC,
    KEYBOARD_GENERIC,
    CONTROLLER_XBOX,
    CONTROLLER_PS,
    CONTROLLER_NS,
    CONTROLLER_GENERIC,

}
public static class InputSpriteController
{
    private static Dictionary<KeyCode, Sprite> INPUT_SPRITES_KEYBOARD;
    private static Dictionary<KeyCode, Sprite> INPUT_SPRITES_XBOX;
    private static Dictionary<KeyCode, Sprite> INPUT_SPRITES_PS;
    private static Dictionary<KeyCode, Sprite> INPUT_SPRITES_NS;
    private static Dictionary<KeyCode, Sprite> INPUT_SPRITES_CONTROLLER_GENERIC;

    public static int CONTROLLER_OFFSET = 16;
    public static int CONTROLLER_START_INDEX = (int)InputType.CONTROLLER_XBOX;

    public static void initializeSpriteDictionaries()
    {
        INPUT_SPRITES_KEYBOARD = new Dictionary<KeyCode, Sprite>();
        INPUT_SPRITES_XBOX = new Dictionary<KeyCode, Sprite>();
        INPUT_SPRITES_PS = new Dictionary<KeyCode, Sprite>();
        INPUT_SPRITES_NS = new Dictionary<KeyCode, Sprite>();
        INPUT_SPRITES_CONTROLLER_GENERIC = new Dictionary<KeyCode, Sprite>();
    }

    public static void setInputIconSprites(List<KeyCode> spriteCodesIn, List<Sprite> spritesIn, InputType inputType)
    {
        if (spriteCodesIn.Count != spritesIn.Count)
        {
            // short circuit
            return;
        }

        switch (inputType)
        {
            case InputType.KEYBOARD_WIN:
                for(int i = 0; i < spriteCodesIn.Count; i++)
                {
                    INPUT_SPRITES_KEYBOARD.Add(spriteCodesIn[i], spritesIn[i]);
                }
                break;
            case InputType.CONTROLLER_XBOX:
                for (int i = 0; i < spriteCodesIn.Count; i++)
                {
                    INPUT_SPRITES_XBOX.Add(spriteCodesIn[i], spritesIn[i]);
                }
                break;
            case InputType.CONTROLLER_PS:
                for (int i = 0; i < spriteCodesIn.Count; i++)
                {
                    INPUT_SPRITES_PS.Add(spriteCodesIn[i], spritesIn[i]);
                }
                break;
            case InputType.CONTROLLER_NS:
                for (int i = 0; i < spriteCodesIn.Count; i++)
                {
                    INPUT_SPRITES_NS.Add(spriteCodesIn[i], spritesIn[i]);
                }
                break;
            case InputType.CONTROLLER_GENERIC:
                for (int i = 0; i < spriteCodesIn.Count; i++)
                {
                    INPUT_SPRITES_CONTROLLER_GENERIC.Add(spriteCodesIn[i], spritesIn[i]);
                }
                break;
            default:
                break;
        }
    }

    public static bool getInputSprite(out Sprite spriteOut, KeyCode keyIn, InputType inputType = InputType.KEYBOARD_WIN)
    {
        switch (inputType)
        {
            case InputType.KEYBOARD_WIN:
                if (INPUT_SPRITES_KEYBOARD.ContainsKey(keyIn))
                {
                    spriteOut = INPUT_SPRITES_KEYBOARD[keyIn];
                    return true;
                }
                break;
            case InputType.CONTROLLER_XBOX:
                if (INPUT_SPRITES_XBOX.ContainsKey(keyIn))
                {
                    spriteOut = INPUT_SPRITES_XBOX[keyIn];
                    return true;
                }
                break;
            case InputType.CONTROLLER_PS:
                if (INPUT_SPRITES_PS.ContainsKey(keyIn))
                {
                    spriteOut = INPUT_SPRITES_PS[keyIn];
                    return true;
                }
                break;
            case InputType.CONTROLLER_NS:
                if (INPUT_SPRITES_NS.ContainsKey(keyIn))
                {
                    spriteOut = INPUT_SPRITES_NS[keyIn];
                    return true;
                }
                break;
            case InputType.CONTROLLER_GENERIC:
                if (INPUT_SPRITES_CONTROLLER_GENERIC.ContainsKey(keyIn))
                {
                    spriteOut = INPUT_SPRITES_CONTROLLER_GENERIC[keyIn];
                    return true;
                }
                break;
            default:
                break;
        }

        // return null value, false to inform caller that lookup has failed
        spriteOut = null;
        return false;
    }
}

// TODO: Add key maps for platform-specific keyboards and gamepads
public class InputSpriteCache : MonoBehaviour
{
    [SerializeField] private InputDeviceMap KeyboardMap;

    void Awake()
    {
        InputSpriteController.initializeSpriteDictionaries();
        InputSpriteController.setInputIconSprites(KeyboardMap.SpriteCodes, KeyboardMap.Sprites, InputType.KEYBOARD_WIN);
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                    Debug.Log("KeyCode down: " + kcode);
            }
        }
    }
}
