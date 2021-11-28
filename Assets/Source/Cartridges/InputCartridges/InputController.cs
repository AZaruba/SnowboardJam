using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using binary values
public enum KeyValue
{
    BTN_NOT_FOUND = 0b00000,
    IDLE = 0b000001,
    UP = 0b00010,
    PRESSED = 0b00100,
    HELD = 0b01000,
    NEED_RESET = 0b10000,
    AXIS_BIN = 0b01110,
    UP_VALID = 0b00011, // special case where UP is valid if both input types are up at the same time
}

public enum ControlAction
{
    ERROR_ACTION = -1,
    JUMP,
    CROUCH,
    DOWN_GRAB,
    LEFT_GRAB,
    RIGHT_GRAB,
    UP_GRAB,

    TURN_AXIS,
    SPIN_AXIS,
    SLOW_AXIS,
    FLIP_AXIS,

    UP_BIN,
    DOWN_BIN,
    LEFT_BIN,
    RIGHT_BIN,

    CONFIRM,
    BACK,
    PAUSE,
}

public static class GlobalInputController
{

    public static ControllerInputData ControllerData;
    private static bool InputInitialized = false;

    private static bool WatchingForInput = false;
    private static bool InputLocked = false;
    private static bool InputToFlush = false;
    private static float InputLockTimer = 0.0f;
    private static float InputLockTimeLimit = 0.25f;

    // Keyboard Inputs
    private static Dictionary<ControlAction, KeyCode> DigitalActionInput_Keyboard;
    private static Dictionary<ControlAction, KeyValue> DigitalActionValue_Keyboard;

    // Gamepad Inputs
    private static Dictionary<ControlAction, KeyCode> DigitalActionInput_Gamepad;
    private static Dictionary<ControlAction, KeyValue> DigitalActionValue_Gamepad;

    // Analog Inputs
    private static Dictionary<ControlAction, string> AnalogActionInput;
    private static Dictionary<ControlAction, float> AnalogActionValue;

    // used for updating inputs in the menu, as this action directly polls Unity's Input class
    private static Dictionary<KeyCode, KeyValue> ArbitraryInputs;
    private static List<KeyCode> GamepadArbitraryInputs;
    private static List<KeyCode> KeyboardArbitraryInputs;

    private static InputType activeControllerType;

    public static bool InitializeInput()
    {
        if (InputInitialized)
        {
            return false;
        }

        DefineInputs();
        InitializeArbitraryInputs();

        InputInitialized = true;
        return true;
    }

    public static void InitializeKeyboardValidInputs()
    {
        KeyboardArbitraryInputs = new List<KeyCode>() {
            KeyCode.Alpha0,
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
            KeyCode.Alpha0,
            KeyCode.F1,
            KeyCode.F2,
            KeyCode.F3,
            KeyCode.F4,
            KeyCode.F5,
            KeyCode.F6,
            KeyCode.F7,
            KeyCode.F8,
            KeyCode.F9,
            KeyCode.F10,
            KeyCode.F11,
            KeyCode.F12,
            KeyCode.A,
            KeyCode.B,
            KeyCode.C,
            KeyCode.D,
            KeyCode.E,
            KeyCode.F,
            KeyCode.G,
            KeyCode.H,
            KeyCode.I,
            KeyCode.J,
            KeyCode.K,
            KeyCode.L,
            KeyCode.M,
            KeyCode.N,
            KeyCode.O,
            KeyCode.P,
            KeyCode.Q,
            KeyCode.R,
            KeyCode.S,
            KeyCode.T,
            KeyCode.U,
            KeyCode.V,
            KeyCode.W,
            KeyCode.X,
            KeyCode.Y,
            KeyCode.Z,
            KeyCode.Space,
            KeyCode.LeftArrow,
            KeyCode.RightArrow,
            KeyCode.UpArrow,
            KeyCode.DownArrow,
            KeyCode.Return,
            KeyCode.Tab,
            KeyCode.LeftShift,
            KeyCode.RightShift,
            KeyCode.LeftAlt,
            KeyCode.RightAlt,
            KeyCode.LeftControl,
            KeyCode.RightControl,
            KeyCode.Delete,
            KeyCode.Period,
            KeyCode.Comma,
            KeyCode.Semicolon,
            KeyCode.Quote,
            KeyCode.Tilde,
            KeyCode.Delete,
            KeyCode.Plus,
            KeyCode.Minus,
            KeyCode.Slash,
            KeyCode.LeftCommand,
            KeyCode.RightCommand,
            KeyCode.RightApple,
            KeyCode.LeftApple,
            KeyCode.RightWindows,
            KeyCode.LeftWindows
        };
    }

    public static void InitializeGamepadValidInputs()
    {

        GamepadArbitraryInputs = new List<KeyCode>() {
            KeyCode.Joystick1Button0,
            KeyCode.Joystick1Button1,
            KeyCode.Joystick1Button2,
            KeyCode.Joystick1Button3,
            KeyCode.Joystick1Button4,
            KeyCode.Joystick1Button5,
            KeyCode.Joystick1Button6,
            KeyCode.Joystick1Button7,
            KeyCode.Joystick1Button8,
            KeyCode.Joystick1Button9,
            KeyCode.Joystick1Button10,
            KeyCode.Joystick1Button11,
            KeyCode.Joystick1Button12,
            KeyCode.Joystick1Button13,
            KeyCode.Joystick1Button14,
            KeyCode.Joystick1Button15,
            KeyCode.Joystick1Button16,
            KeyCode.Joystick1Button17,
            KeyCode.Joystick1Button18,
            KeyCode.Joystick1Button19,
        };
    }

    public static void InitializeArbitraryInputs()
    {
        InitializeKeyboardValidInputs();
        InitializeGamepadValidInputs();

        // filter out "any joystick" inputs
        List<KeyCode> filteredKeys = new List<KeyCode>() {
            KeyCode.JoystickButton0,
            KeyCode.JoystickButton1,
            KeyCode.JoystickButton2,
            KeyCode.JoystickButton3,
            KeyCode.JoystickButton4,
            KeyCode.JoystickButton5,
            KeyCode.JoystickButton6,
            KeyCode.JoystickButton7,
            KeyCode.JoystickButton8,
            KeyCode.JoystickButton9,
            KeyCode.JoystickButton10,
            KeyCode.JoystickButton11,
            KeyCode.JoystickButton12,
            KeyCode.JoystickButton13,
            KeyCode.JoystickButton14,
            KeyCode.JoystickButton15,
            KeyCode.JoystickButton16,
            KeyCode.JoystickButton17,
            KeyCode.JoystickButton18,
            KeyCode.JoystickButton19,
            KeyCode.Escape, // "cheese it" button
        };


        ArbitraryInputs = new Dictionary<KeyCode, KeyValue>();
        foreach (KeyCode keyIn in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (!ArbitraryInputs.ContainsKey(keyIn) && !filteredKeys.Contains(keyIn))
            {
                ArbitraryInputs.Add(keyIn, KeyValue.IDLE);
            }
        }
    }

    public static void DefineKeyboardInputs()
    {
        DigitalActionValue_Keyboard[ControlAction.JUMP] = KeyValue.IDLE;
        DigitalActionInput_Keyboard[ControlAction.JUMP] = GlobalGameData.GetActionSetting(ControlAction.JUMP);

        DigitalActionValue_Keyboard[ControlAction.DOWN_GRAB] = KeyValue.IDLE;
        DigitalActionInput_Keyboard[ControlAction.DOWN_GRAB] = GlobalGameData.GetActionSetting(ControlAction.DOWN_GRAB);

        DigitalActionValue_Keyboard[ControlAction.LEFT_GRAB] = KeyValue.IDLE;
        DigitalActionInput_Keyboard[ControlAction.LEFT_GRAB] = GlobalGameData.GetActionSetting(ControlAction.LEFT_GRAB);

        DigitalActionValue_Keyboard[ControlAction.RIGHT_GRAB] = KeyValue.IDLE;
        DigitalActionInput_Keyboard[ControlAction.RIGHT_GRAB] = GlobalGameData.GetActionSetting(ControlAction.RIGHT_GRAB);

        DigitalActionValue_Keyboard[ControlAction.UP_GRAB] = KeyValue.IDLE;
        DigitalActionInput_Keyboard[ControlAction.UP_GRAB] = GlobalGameData.GetActionSetting(ControlAction.UP_GRAB);

        DigitalActionValue_Keyboard[ControlAction.PAUSE] = KeyValue.IDLE;
        DigitalActionInput_Keyboard[ControlAction.PAUSE] = GlobalGameData.GetActionSetting(ControlAction.PAUSE);

        DigitalActionValue_Keyboard[ControlAction.CONFIRM] = KeyValue.IDLE;
        DigitalActionInput_Keyboard[ControlAction.CONFIRM] = GlobalGameData.GetActionSetting(ControlAction.CONFIRM);

        DigitalActionValue_Keyboard[ControlAction.BACK] = KeyValue.IDLE;
        DigitalActionInput_Keyboard[ControlAction.BACK] = GlobalGameData.GetActionSetting(ControlAction.BACK);

        DigitalActionValue_Keyboard[ControlAction.DOWN_BIN] = KeyValue.IDLE;
        DigitalActionInput_Keyboard[ControlAction.DOWN_BIN] = GlobalGameData.GetActionSetting(ControlAction.DOWN_BIN);

        DigitalActionValue_Keyboard[ControlAction.LEFT_BIN] = KeyValue.IDLE;
        DigitalActionInput_Keyboard[ControlAction.LEFT_BIN] = GlobalGameData.GetActionSetting(ControlAction.LEFT_BIN);

        DigitalActionValue_Keyboard[ControlAction.RIGHT_BIN] = KeyValue.IDLE;
        DigitalActionInput_Keyboard[ControlAction.RIGHT_BIN] = GlobalGameData.GetActionSetting(ControlAction.RIGHT_BIN);

        DigitalActionValue_Keyboard[ControlAction.UP_BIN] = KeyValue.IDLE;
        DigitalActionInput_Keyboard[ControlAction.UP_BIN] = GlobalGameData.GetActionSetting(ControlAction.UP_BIN);

    }

    public static void DefineGamepadInputs()
    {
        DigitalActionValue_Gamepad[ControlAction.JUMP] = KeyValue.IDLE;
        DigitalActionInput_Gamepad[ControlAction.JUMP] = GlobalGameData.GetActionSetting(ControlAction.JUMP, InputType.CONTROLLER_GENERIC);

        DigitalActionValue_Gamepad[ControlAction.DOWN_GRAB] = KeyValue.IDLE;
        DigitalActionInput_Gamepad[ControlAction.DOWN_GRAB] = GlobalGameData.GetActionSetting(ControlAction.DOWN_GRAB, InputType.CONTROLLER_GENERIC);

        DigitalActionValue_Gamepad[ControlAction.LEFT_GRAB] = KeyValue.IDLE;
        DigitalActionInput_Gamepad[ControlAction.LEFT_GRAB] = GlobalGameData.GetActionSetting(ControlAction.LEFT_GRAB, InputType.CONTROLLER_GENERIC);

        DigitalActionValue_Gamepad[ControlAction.RIGHT_GRAB] = KeyValue.IDLE;
        DigitalActionInput_Gamepad[ControlAction.RIGHT_GRAB] = GlobalGameData.GetActionSetting(ControlAction.RIGHT_GRAB, InputType.CONTROLLER_GENERIC);

        DigitalActionValue_Gamepad[ControlAction.UP_GRAB] = KeyValue.IDLE;
        DigitalActionInput_Gamepad[ControlAction.UP_GRAB] = GlobalGameData.GetActionSetting(ControlAction.UP_GRAB, InputType.CONTROLLER_GENERIC);

        DigitalActionValue_Gamepad[ControlAction.PAUSE] = KeyValue.IDLE;
        DigitalActionInput_Gamepad[ControlAction.PAUSE] = GlobalGameData.GetActionSetting(ControlAction.PAUSE, InputType.CONTROLLER_GENERIC);

        DigitalActionValue_Gamepad[ControlAction.CONFIRM] = KeyValue.IDLE;
        DigitalActionInput_Gamepad[ControlAction.CONFIRM] = GlobalGameData.GetActionSetting(ControlAction.CONFIRM, InputType.CONTROLLER_GENERIC);

        DigitalActionValue_Gamepad[ControlAction.BACK] = KeyValue.IDLE;
        DigitalActionInput_Gamepad[ControlAction.BACK] = GlobalGameData.GetActionSetting(ControlAction.BACK, InputType.CONTROLLER_GENERIC);

        DigitalActionValue_Gamepad[ControlAction.DOWN_BIN] = KeyValue.IDLE;
        DigitalActionInput_Gamepad[ControlAction.DOWN_BIN] = GlobalGameData.GetActionSetting(ControlAction.DOWN_BIN, InputType.CONTROLLER_GENERIC);

        DigitalActionValue_Gamepad[ControlAction.LEFT_BIN] = KeyValue.IDLE;
        DigitalActionInput_Gamepad[ControlAction.LEFT_BIN] = GlobalGameData.GetActionSetting(ControlAction.LEFT_BIN, InputType.CONTROLLER_GENERIC);

        DigitalActionValue_Gamepad[ControlAction.RIGHT_BIN] = KeyValue.IDLE;
        DigitalActionInput_Gamepad[ControlAction.RIGHT_BIN] = GlobalGameData.GetActionSetting(ControlAction.RIGHT_BIN, InputType.CONTROLLER_GENERIC);

        DigitalActionValue_Gamepad[ControlAction.UP_BIN] = KeyValue.IDLE;
        DigitalActionInput_Gamepad[ControlAction.UP_BIN] = GlobalGameData.GetActionSetting(ControlAction.UP_BIN, InputType.CONTROLLER_GENERIC);


    }

    public static void DefineInputs()
    {
        ControllerData = new ControllerInputData();

        DigitalActionInput_Keyboard = new Dictionary<ControlAction, KeyCode>();
        DigitalActionValue_Keyboard = new Dictionary<ControlAction, KeyValue>();

        DigitalActionInput_Gamepad = new Dictionary<ControlAction, KeyCode>();
        DigitalActionValue_Gamepad = new Dictionary<ControlAction, KeyValue>();

        AnalogActionInput = new Dictionary<ControlAction, string>();
        AnalogActionValue = new Dictionary<ControlAction, float>();

        DefineKeyboardInputs();
        DefineGamepadInputs();

        AnalogActionInput[ControlAction.SPIN_AXIS] = GlobalGameData.GetAnalogActionSetting(ControlAction.SPIN_AXIS);
        AnalogActionInput[ControlAction.TURN_AXIS] = GlobalGameData.GetAnalogActionSetting(ControlAction.SPIN_AXIS);
        AnalogActionInput[ControlAction.FLIP_AXIS] = GlobalGameData.GetAnalogActionSetting(ControlAction.FLIP_AXIS);
        AnalogActionInput[ControlAction.SLOW_AXIS] = GlobalGameData.GetAnalogActionSetting(ControlAction.FLIP_AXIS);

        AnalogActionValue[ControlAction.SPIN_AXIS] = Constants.ZERO_F;
        AnalogActionValue[ControlAction.TURN_AXIS] = Constants.ZERO_F;
        AnalogActionValue[ControlAction.FLIP_AXIS] = Constants.ZERO_F;
        AnalogActionValue[ControlAction.SLOW_AXIS] = Constants.ZERO_F;
    }

    public static KeyCode GetInputKey(ControlAction actIn)
    {
        if (DigitalActionInput_Keyboard.ContainsKey(actIn))
        {
            return DigitalActionInput_Keyboard[actIn];
        }
        return KeyCode.None;
    }

    // TODO: play a game that allows both inputs and analyze how messing with both input types works
    public static bool GetInputAction(ControlAction actIn, KeyValue desiredValue)
    {
        /*
         * IDLE true = key IDLE and pad IDLE
         * PRS true = key PRESS or pad PRESS
         * HLD true = key HLD or pad HLD
         * UP true = key UP and pad IDLE OR pad IDLE and key UP
         */


        KeyValue ControllerValue = KeyValue.BTN_NOT_FOUND;
        KeyValue KeyboardValue = KeyValue.BTN_NOT_FOUND;
        // assume no keys are assigned to one and not the other
        if (DigitalActionInput_Gamepad.ContainsKey(actIn))
        {
            ControllerValue = DigitalActionValue_Gamepad[actIn];
        }
        if (DigitalActionInput_Keyboard.ContainsKey(actIn))
        {
            KeyboardValue = DigitalActionValue_Keyboard[actIn];
        }

        switch(desiredValue)
        {
            case KeyValue.IDLE:
                return ControllerValue == desiredValue && KeyboardValue == desiredValue;
                break;
            case KeyValue.PRESSED:
                return ControllerValue == desiredValue || KeyboardValue == desiredValue;
                break;
            case KeyValue.HELD:
                return ControllerValue == desiredValue || KeyboardValue == desiredValue;
                break;
            case KeyValue.UP:
                return (ControllerValue == desiredValue && (KeyboardValue & KeyValue.UP_VALID) != KeyValue.BTN_NOT_FOUND) || 
                       (KeyboardValue == desiredValue && (ControllerValue & KeyValue.UP_VALID) != KeyValue.BTN_NOT_FOUND);
                break;
        }

        return false;
    }

    /*
    public static bool GetBinaryAnalogAction(ControlAction actIn, out float valueOut)
    {
        valueOut = Constants.ZERO_F;

        if (actIn == ControlAction.TURN_AXIS ||
            actIn == ControlAction.SPIN_AXIS)
        {
            valueOut = (GetInputAction(ControlAction.RIGHT_BIN) & KeyValue.AXIS_BIN) != 0 ? 1 : valueOut;
            valueOut = (GetInputAction(ControlAction.LEFT_BIN) & KeyValue.AXIS_BIN) != 0 ? -1 : valueOut;
        }
        else if (actIn == ControlAction.SLOW_AXIS ||
            actIn == ControlAction.FLIP_AXIS)
        {
            valueOut = (GetInputAction(ControlAction.UP_BIN) & KeyValue.AXIS_BIN) != 0 ? 1 : valueOut;
            valueOut = (GetInputAction(ControlAction.DOWN_BIN) & KeyValue.AXIS_BIN) != 0 ? -1 : valueOut;
        }

        return !valueOut.Equals(Constants.ZERO_F);
    }
    */

    public static float GetAnalogInputAction(ControlAction actIn)
    {
        float returnValue = float.MaxValue;
        if (AnalogActionInput.ContainsKey(actIn))
        {
            return AnalogActionValue[actIn];
        }
        return returnValue;
    }

    /// <summary>
    /// FixedUpdate-friendly input checking, as we check for
    /// "pressed" and "up" states, which aren't reliable by default.
    /// 
    /// A function of the previous frame's value and the current frame's
    /// value is used to dictate what the current frame value is
    /// </summary>
    /// <param name="actIn">The ControlAction we are currently checking.</param>
    public static void CheckAndSetKeyValue(ControlAction actIn)
    {
        if (DigitalActionInput_Keyboard.ContainsKey(actIn))
        {
            KeyCode keyIn = DigitalActionInput_Keyboard[actIn];
            KeyValue frameValue = DigitalActionValue_Keyboard[actIn];
            bool inputValue = Input.GetKey(keyIn);

            /* Pseudo
             * Get current key value
             * 
             * check Input.GetKey()
             *     | true | false
             *-----|------|---------
             * IDLE| PRES | IDLE
             *-----|------|---------
             * HELD| HELD | UP
             * ----|------|---------
             * PRES| HELD | UP
             * ----|------|---------
             * UP  | PRES | IDLE
             */

            switch (frameValue)
            {
                case KeyValue.IDLE:
                    frameValue = inputValue ? KeyValue.PRESSED : KeyValue.IDLE;
                    break;
                case KeyValue.PRESSED:
                    frameValue = inputValue ? KeyValue.HELD : KeyValue.UP;
                    break;
                case KeyValue.HELD:
                    frameValue = inputValue ? KeyValue.HELD : KeyValue.UP;
                    break;
                case KeyValue.UP:
                    frameValue = inputValue ? KeyValue.PRESSED : KeyValue.IDLE;
                    break;
            }

            DigitalActionValue_Keyboard[actIn] = frameValue;
        }

        if (DigitalActionInput_Gamepad.ContainsKey(actIn))
        {
            KeyCode keyIn = DigitalActionInput_Gamepad[actIn];
            KeyValue frameValue = DigitalActionValue_Gamepad[actIn];
            bool inputValue = Input.GetKey(keyIn);

            /* Pseudo
             * Get current key value
             * 
             * check Input.GetKey()
             *     | true | false
             *-----|------|---------
             * IDLE| PRES | IDLE
             *-----|------|---------
             * HELD| HELD | UP
             * ----|------|---------
             * PRES| HELD | UP
             * ----|------|---------
             * UP  | PRES | IDLE
             */

            switch (frameValue)
            {
                case KeyValue.IDLE:
                    frameValue = inputValue ? KeyValue.PRESSED : KeyValue.IDLE;
                    break;
                case KeyValue.PRESSED:
                    frameValue = inputValue ? KeyValue.HELD : KeyValue.UP;
                    break;
                case KeyValue.HELD:
                    frameValue = inputValue ? KeyValue.HELD : KeyValue.UP;
                    break;
                case KeyValue.UP:
                    frameValue = inputValue ? KeyValue.PRESSED : KeyValue.IDLE;
                    break;
            }

            DigitalActionValue_Gamepad[actIn] = frameValue;
        }
    }

    /// <summary>
    /// Checks and sets the values for all keys listed in the system keycode enum.
    /// This is used to poll for input within FixedUpdate for ANY key. It should
    /// only run when rebinding a key in the menu.
    /// </summary>
    public static void CheckAndSetValueArbitrary()
    {
        foreach (KeyCode keyIn in new List<KeyCode>(ArbitraryInputs.Keys))
        {
            KeyValue frameValue = ArbitraryInputs[keyIn];
            bool inputValue = Input.GetKey(keyIn);

            switch (frameValue)
            {
                case KeyValue.IDLE:
                    frameValue = inputValue ? KeyValue.PRESSED : KeyValue.IDLE;
                    break;
                case KeyValue.PRESSED:
                    frameValue = inputValue ? KeyValue.HELD : KeyValue.UP;
                    break;
                case KeyValue.HELD:
                    frameValue = inputValue ? KeyValue.HELD : KeyValue.UP;
                    break;
                case KeyValue.UP:
                    frameValue = inputValue ? KeyValue.PRESSED : KeyValue.IDLE;
                    break;
            }

            ArbitraryInputs[keyIn] = frameValue;
        }
    }

    public static void FlushArbitraryInputs()
    {
        foreach (KeyCode keyIn in new List<KeyCode>(ArbitraryInputs.Keys))
        {
            ArbitraryInputs[keyIn] = KeyValue.IDLE;
        }
    }

    public static void ResetKey(ControlAction actIn)
    {
        if (DigitalActionInput_Keyboard.ContainsKey(actIn))
        {
            DigitalActionValue_Keyboard[actIn] = KeyValue.IDLE;
        }
    }

    public static void CheckAndSetAxisValue(ControlAction actIn)
    {
        if (!AnalogActionInput.ContainsKey(actIn))
        {
            return;
        }
        string axisIn = AnalogActionInput[actIn];

        AnalogActionValue[actIn] = Input.GetAxisRaw(axisIn);
    }

    public static void UpdateInput()
    {
        if (InputToFlush)
        {
            FlushInputs();
            InputToFlush = false;
            return;
        }
        CheckAndSetKeyValue(ControlAction.JUMP);
        CheckAndSetKeyValue(ControlAction.LEFT_GRAB);
        CheckAndSetKeyValue(ControlAction.RIGHT_GRAB);
        CheckAndSetKeyValue(ControlAction.DOWN_GRAB);
        CheckAndSetKeyValue(ControlAction.UP_GRAB);

        CheckAndSetKeyValue(ControlAction.PAUSE);
        CheckAndSetKeyValue(ControlAction.CONFIRM);
        CheckAndSetKeyValue(ControlAction.BACK);

        CheckAndSetKeyValue(ControlAction.LEFT_BIN);
        CheckAndSetKeyValue(ControlAction.RIGHT_BIN);
        CheckAndSetKeyValue(ControlAction.DOWN_BIN);
        CheckAndSetKeyValue(ControlAction.UP_BIN);

        CheckAndSetAxisValue(ControlAction.TURN_AXIS);
        CheckAndSetAxisValue(ControlAction.SPIN_AXIS);
        CheckAndSetAxisValue(ControlAction.SLOW_AXIS);
        CheckAndSetAxisValue(ControlAction.FLIP_AXIS);
    }

    public static void FlushInputs()
    {
        ResetKey(ControlAction.JUMP);
        ResetKey(ControlAction.LEFT_GRAB);
        ResetKey(ControlAction.RIGHT_GRAB);
        ResetKey(ControlAction.DOWN_GRAB);
        ResetKey(ControlAction.UP_GRAB);

        ResetKey(ControlAction.UP_BIN);
        ResetKey(ControlAction.LEFT_BIN);
        ResetKey(ControlAction.RIGHT_BIN);
        ResetKey(ControlAction.DOWN_BIN);

        ResetKey(ControlAction.PAUSE);
        ResetKey(ControlAction.CONFIRM);
        ResetKey(ControlAction.BACK);
    }

    public static void FlushNextFrame()
    {
        InputToFlush = true;
    }

    public static void LockConfirm()
    {
        ResetKey(ControlAction.CONFIRM);
    }

    public static void LockInput()
    {
        // InputLocked = true;
        FlushInputs();
    }

    public static bool InputIsLocked()
    {
        return InputLocked;
    }

    public static IEnumerator StartLockTimer()
    {
        while (InputLockTimer < InputLockTimeLimit)
        {
            InputLockTimer += Time.deltaTime;
            yield return null;
        }

        InputLockTimer = 0.0f;
        InputLocked = false;
        UpdateInput();
    }

    public static void StartWatchForAnyInput()
    {
        WatchingForInput = true;
    }

    public static void StopWatchForAnyInput()
    {
        FlushArbitraryInputs();
        WatchingForInput = false;
    }

    public static bool WatchForAnyInput()
    {
        return WatchingForInput;
    }

    public static KeyCode GetAnyKey()
    {
        foreach(KeyCode keyIn in ArbitraryInputs.Keys)
        {
            if (ArbitraryInputs[keyIn] == KeyValue.PRESSED)
            {
                return keyIn;
            }
        }
        return KeyCode.None;
    }

    public static bool UpdateAction(ControlAction actIn, KeyCode keyIn)
    {
        // check if action is mapped to key
        // and if key is in the key dictionary
        // set them all and in controllerdata
        if (DigitalActionInput_Keyboard.ContainsKey(actIn))
        {
            DigitalActionInput_Keyboard.Remove(actIn);
            DigitalActionValue_Keyboard.Remove(actIn);

            // reset value
            DigitalActionInput_Keyboard[actIn] = keyIn;
            DigitalActionValue_Keyboard[actIn] = KeyValue.IDLE;

            return true;
        }
        return false;
    }

    public static bool UpdateAction(ControlAction actIn, string axisIn)
    {
        if (AnalogActionInput.ContainsKey(actIn))
        {
            AnalogActionInput.Remove(actIn);
            AnalogActionValue.Remove(actIn);

            // reset value
            AnalogActionInput[actIn] = axisIn;
            AnalogActionValue[actIn] = Constants.ZERO_F;

            return true;
        }
        return false;
    }

    public static List<ControlAction> GetActionForKey(KeyCode keyIn)
    {
        List<ControlAction> actionsOut = new List<ControlAction>();
        if (DigitalActionInput_Keyboard.ContainsValue(keyIn))
        {
           foreach(ControlAction action in DigitalActionInput_Keyboard.Keys)
           {
                if (DigitalActionInput_Keyboard[action] == keyIn)
                {
                    actionsOut.Add(action);
                }
           }
        }
        return actionsOut;
    }

    public static InputType GetActiveControllerType()
    {
        return activeControllerType;
    }

    public static void SetActiveControllerType(InputType typeIn)
    {
        activeControllerType = typeIn;
    }
}

public class InputController : MonoBehaviour
{
    private void Awake()
    {
        GlobalGameData.CheckAndSetDefaults();

        GlobalInputController.InitializeInput();
        if (GlobalInputController.InputIsLocked())
        {
            StartCoroutine(GlobalInputController.StartLockTimer());
        }
    }

    private void FixedUpdate()
    {
        if (GlobalInputController.InputIsLocked())
        {
            return;
        }
        if (GlobalInputController.WatchForAnyInput())
        {
            GlobalInputController.CheckAndSetValueArbitrary();
        }
        GlobalInputController.UpdateInput();
    }
}

public class DefaultControlLayout
{
    public KeyCode DefaultPause;
    public KeyCode DefaultTrickU;
    public KeyCode DefaultTrickR;
    public KeyCode DefaultTrickL;
    public KeyCode DefaultTrickD;
    public KeyCode DefaultJump;

    public KeyCode DefaultTuck;
    public KeyCode DefaultBack;
    public KeyCode DefaultConfirm;

    public KeyCode DefaultBinU;
    public KeyCode DefaultBinR;
    public KeyCode DefaultBinL;
    public KeyCode DefaultBinD;

    public string DefaultLHoriz;
    public string DefaultLVerti;
}

public static class DefaultControls
{
    public static DefaultControlLayout KeyboardLayout;
    public static DefaultControlLayout PSLayout;
    public static DefaultControlLayout XboxLayout;

    public static Dictionary<KeyCode, KeyCode> XboxToPS;
    public static Dictionary<KeyCode, KeyCode> PSToXbox;

    public static void InitializeLayouts()
    {
        KeyboardLayout = new DefaultControlLayout();
        PSLayout = new DefaultControlLayout();
        XboxLayout = new DefaultControlLayout();
            // KBM
        KeyboardLayout.DefaultPause = KeyCode.P;
        KeyboardLayout.DefaultTrickU = KeyCode.I;
        KeyboardLayout.DefaultTrickR = KeyCode.L;
        KeyboardLayout.DefaultTrickL = KeyCode.J;
        KeyboardLayout.DefaultTrickD = KeyCode.K;
        KeyboardLayout.DefaultJump = KeyCode.Space;

        KeyboardLayout.DefaultTuck = KeyCode.LeftControl;
        KeyboardLayout.DefaultBack = KeyCode.L;
        KeyboardLayout.DefaultConfirm = KeyCode.K;

        KeyboardLayout.DefaultBinU = KeyCode.UpArrow;
        KeyboardLayout.DefaultBinR = KeyCode.RightArrow;
        KeyboardLayout.DefaultBinL = KeyCode.LeftArrow;
        KeyboardLayout.DefaultBinD = KeyCode.DownArrow;

        KeyboardLayout.DefaultLHoriz = "Horizontal";
        KeyboardLayout.DefaultLVerti = "Vertical";

        // PS
        PSLayout.DefaultPause = KeyCode.Joystick1Button9;
        PSLayout.DefaultTrickU = KeyCode.Joystick1Button3;
        PSLayout.DefaultTrickR = KeyCode.Joystick1Button2;
        PSLayout.DefaultTrickL = KeyCode.Joystick1Button0;
        PSLayout.DefaultTrickD = KeyCode.Joystick1Button1;
        PSLayout.DefaultJump = KeyCode.Joystick1Button5;

        PSLayout.DefaultTuck = KeyCode.Joystick1Button4;
        PSLayout.DefaultBack = KeyCode.Joystick1Button2;
        PSLayout.DefaultConfirm = KeyCode.Joystick1Button1;

        PSLayout.DefaultBinU = KeyCode.UpArrow;
        PSLayout.DefaultBinR = KeyCode.RightArrow;
        PSLayout.DefaultBinL = KeyCode.LeftArrow;
        PSLayout.DefaultBinD = KeyCode.DownArrow;

        PSLayout.DefaultLHoriz = "Horizontal";
        PSLayout.DefaultLVerti = "Vertical";

        // Xbox
        XboxLayout.DefaultPause = KeyCode.Joystick1Button7;
        XboxLayout.DefaultTrickU = KeyCode.Joystick1Button3;
        XboxLayout.DefaultTrickR = KeyCode.Joystick1Button1;
        XboxLayout.DefaultTrickL = KeyCode.Joystick1Button2;
        XboxLayout.DefaultTrickD = KeyCode.Joystick1Button0;
        XboxLayout.DefaultJump = KeyCode.Joystick1Button5;

        XboxLayout.DefaultTuck = KeyCode.Joystick1Button4;
        XboxLayout.DefaultBack = KeyCode.Joystick1Button1;
        XboxLayout.DefaultConfirm = KeyCode.Joystick1Button0;

        XboxLayout.DefaultBinU = KeyCode.UpArrow;
        XboxLayout.DefaultBinR = KeyCode.RightArrow;
        XboxLayout.DefaultBinL = KeyCode.LeftArrow;
        XboxLayout.DefaultBinD = KeyCode.DownArrow;

        XboxLayout.DefaultLHoriz = "Horizontal";
        XboxLayout.DefaultLVerti = "Vertical";
    }

    // TODO: generate Xbox/Playstation conversion maps to allow for input rebinding
    public static void InitializeMaps()
    {
        XboxToPS = new Dictionary<KeyCode, KeyCode>();
        PSToXbox = new Dictionary<KeyCode, KeyCode>();
    }
}

// TODO: come up with a solution for binding the xbox triggers as buttons
