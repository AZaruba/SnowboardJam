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

    private static Sprite EMPTY_SPRITE;

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
                    if (spriteCodesIn[i] != KeyCode.None)
                    {
                        INPUT_SPRITES_KEYBOARD.Add(spriteCodesIn[i], spritesIn[i]);
                    }
                }
                break;
            case InputType.CONTROLLER_XBOX:
                for (int i = 0; i < spriteCodesIn.Count; i++)
                {
                    if (spriteCodesIn[i] != KeyCode.None)
                    {
                        INPUT_SPRITES_XBOX.Add(spriteCodesIn[i], spritesIn[i]);
                    }
                }
                break;
            case InputType.CONTROLLER_PS:
                for (int i = 0; i < spriteCodesIn.Count; i++)
                {
                    if (spriteCodesIn[i] != KeyCode.None)
                    {
                        INPUT_SPRITES_PS.Add(spriteCodesIn[i], spritesIn[i]);
                    }
                }
                break;
            case InputType.CONTROLLER_NS:
                for (int i = 0; i < spriteCodesIn.Count; i++)
                {
                    if (spriteCodesIn[i] != KeyCode.None)
                    {
                        INPUT_SPRITES_NS.Add(spriteCodesIn[i], spritesIn[i]);
                    }
                }
                break;
            case InputType.CONTROLLER_GENERIC:
                for (int i = 0; i < spriteCodesIn.Count; i++)
                {
                    if (spriteCodesIn[i] != KeyCode.None)
                    {
                        INPUT_SPRITES_CONTROLLER_GENERIC.Add(spriteCodesIn[i], spritesIn[i]);
                    }
                }
                break;
            default:
                break;
        }
    }

    public static bool getInputSprite(out Sprite spriteOut, KeyCode keyIn, InputType inputType = InputType.KEYBOARD_GENERIC)
    {
        if (keyIn == KeyCode.None)
        {
            spriteOut = EMPTY_SPRITE;
            return true;
        }

        if (INPUT_SPRITES_KEYBOARD.ContainsKey(keyIn) && inputType == InputType.KEYBOARD_GENERIC)
        {
            spriteOut = INPUT_SPRITES_KEYBOARD[keyIn];
            return true;
        }
        if (INPUT_SPRITES_XBOX.ContainsKey(keyIn) && inputType == InputType.CONTROLLER_GENERIC)
        {
            // TODO: differentiate between PS and Xbox
            spriteOut = INPUT_SPRITES_XBOX[keyIn];
            return true;
        }

        // return null value, false to inform caller that lookup has failed
        spriteOut = null;
        return false;
    }

    public static void SetEmptySprite(Sprite spriteIn)
    {
        EMPTY_SPRITE = spriteIn;
    }

    public static Sprite EmptySprite()
    {
        return EMPTY_SPRITE;
    }

    public static List<KeyCode> GetKeyCodes(InputType type)
    {
        switch(type)
        {
            case InputType.KEYBOARD_GENERIC:
                return new List<KeyCode>(INPUT_SPRITES_KEYBOARD.Keys);
                break;
            case InputType.CONTROLLER_XBOX:
                return new List<KeyCode>(INPUT_SPRITES_XBOX.Keys);
                break;
            case InputType.CONTROLLER_PS:
                return new List<KeyCode>(INPUT_SPRITES_PS.Keys);
                break;
        }
        return new List<KeyCode>();
    }
}

// TODO: Add key maps for platform-specific keyboards and gamepads
public class InputSpriteCache : MonoBehaviour
{
    [SerializeField] private InputDeviceMap KeyboardMap;
    [SerializeField] private InputDeviceMap PSMap;
    [SerializeField] private InputDeviceMap XboxMap;
    [SerializeField] private Sprite EmptyInputSprite;

    void Awake()
    {
        InputSpriteController.initializeSpriteDictionaries();
        InputSpriteController.setInputIconSprites(KeyboardMap.SpriteCodes, KeyboardMap.Sprites, InputType.KEYBOARD_WIN);
        InputSpriteController.setInputIconSprites(PSMap.SpriteCodes, PSMap.Sprites, InputType.CONTROLLER_PS);
        InputSpriteController.setInputIconSprites(XboxMap.SpriteCodes, XboxMap.Sprites, InputType.CONTROLLER_XBOX);
        InputSpriteController.SetEmptySprite(EmptyInputSprite);
    }
}
