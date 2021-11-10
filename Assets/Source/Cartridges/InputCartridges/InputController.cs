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

    SAFETY_CONFIRM,
    SAFETY_BACK,
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

    private static Dictionary<ControlAction, KeyCode> DigitalActionInput;
    private static Dictionary<ControlAction, KeyValue> DigitalActionValue;
    private static Dictionary<ControlAction, string> AnalogActionInput;
    private static Dictionary<ControlAction, float> AnalogActionValue;

    // used for updating inputs in the menu, as this action directly polls Unity's Input class
    private static Dictionary<KeyCode, KeyValue> ArbitraryInputs;

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

    public static void InitializeArbitraryInputs()
    {
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
            KeyCode.JoystickButton19
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

    public static void DefineInputs()
    {
        ControllerData = new ControllerInputData();

        DigitalActionInput = new Dictionary<ControlAction, KeyCode>();
        DigitalActionValue = new Dictionary<ControlAction, KeyValue>();
        AnalogActionInput = new Dictionary<ControlAction, string>();
        AnalogActionValue = new Dictionary<ControlAction, float>();

        DigitalActionValue[ControlAction.JUMP] = KeyValue.IDLE;
        DigitalActionInput[ControlAction.JUMP] = GlobalGameData.GetActionSetting(ControlAction.JUMP);

        DigitalActionValue[ControlAction.DOWN_GRAB] = KeyValue.IDLE;
        DigitalActionInput[ControlAction.DOWN_GRAB] = GlobalGameData.GetActionSetting(ControlAction.DOWN_GRAB);

        DigitalActionValue[ControlAction.LEFT_GRAB] = KeyValue.IDLE;
        DigitalActionInput[ControlAction.LEFT_GRAB] = GlobalGameData.GetActionSetting(ControlAction.LEFT_GRAB);

        DigitalActionValue[ControlAction.RIGHT_GRAB] = KeyValue.IDLE;
        DigitalActionInput[ControlAction.RIGHT_GRAB] = GlobalGameData.GetActionSetting(ControlAction.RIGHT_GRAB);

        DigitalActionValue[ControlAction.UP_GRAB] = KeyValue.IDLE;
        DigitalActionInput[ControlAction.UP_GRAB] = GlobalGameData.GetActionSetting(ControlAction.UP_GRAB);

        DigitalActionValue[ControlAction.PAUSE] = KeyValue.IDLE;
        DigitalActionInput[ControlAction.PAUSE] = GlobalGameData.GetActionSetting(ControlAction.PAUSE);

        DigitalActionValue[ControlAction.CONFIRM] = KeyValue.IDLE;
        DigitalActionInput[ControlAction.CONFIRM] = GlobalGameData.GetActionSetting(ControlAction.CONFIRM);

        DigitalActionValue[ControlAction.BACK] = KeyValue.IDLE;
        DigitalActionInput[ControlAction.BACK] = GlobalGameData.GetActionSetting(ControlAction.BACK);

        DigitalActionValue[ControlAction.DOWN_BIN] = KeyValue.IDLE;
        DigitalActionInput[ControlAction.DOWN_BIN] = GlobalGameData.GetActionSetting(ControlAction.DOWN_BIN);

        DigitalActionValue[ControlAction.LEFT_BIN] = KeyValue.IDLE;
        DigitalActionInput[ControlAction.LEFT_BIN] = GlobalGameData.GetActionSetting(ControlAction.LEFT_BIN);

        DigitalActionValue[ControlAction.RIGHT_BIN] = KeyValue.IDLE;
        DigitalActionInput[ControlAction.RIGHT_BIN] = GlobalGameData.GetActionSetting(ControlAction.RIGHT_BIN);

        DigitalActionValue[ControlAction.UP_BIN] = KeyValue.IDLE;
        DigitalActionInput[ControlAction.UP_BIN] = GlobalGameData.GetActionSetting(ControlAction.UP_BIN);

        AnalogActionInput[ControlAction.SPIN_AXIS] = GlobalGameData.GetAnalogActionSetting(ControlAction.SPIN_AXIS);
        AnalogActionInput[ControlAction.TURN_AXIS] = GlobalGameData.GetAnalogActionSetting(ControlAction.SPIN_AXIS);
        AnalogActionInput[ControlAction.FLIP_AXIS] = GlobalGameData.GetAnalogActionSetting(ControlAction.FLIP_AXIS);
        AnalogActionInput[ControlAction.SLOW_AXIS] = GlobalGameData.GetAnalogActionSetting(ControlAction.FLIP_AXIS);

        AnalogActionValue[ControlAction.SPIN_AXIS] = Constants.ZERO_F;
        AnalogActionValue[ControlAction.TURN_AXIS] = Constants.ZERO_F;
        AnalogActionValue[ControlAction.FLIP_AXIS] = Constants.ZERO_F;
        AnalogActionValue[ControlAction.SLOW_AXIS] = Constants.ZERO_F;

        DigitalActionValue[ControlAction.SAFETY_BACK] = KeyValue.IDLE;
        DigitalActionInput[ControlAction.SAFETY_BACK] = GlobalGameData.GetActionSetting(ControlAction.SAFETY_BACK);

        DigitalActionValue[ControlAction.SAFETY_CONFIRM] = KeyValue.IDLE;
        DigitalActionInput[ControlAction.SAFETY_CONFIRM] = GlobalGameData.GetActionSetting(ControlAction.SAFETY_CONFIRM);
    }

    public static KeyCode GetInputKey(ControlAction actIn)
    {
        if (DigitalActionInput.ContainsKey(actIn))
        {
            return DigitalActionInput[actIn];
        }
        return KeyCode.None;
    }
    public static KeyValue GetInputAction(ControlAction actIn)
    {
        // assume no keys are assigned to one and not the other
        if (DigitalActionInput.ContainsKey(actIn))
        {
            if (actIn == ControlAction.CONFIRM)
            {
                KeyValue safetyConfirm = DigitalActionValue[ControlAction.SAFETY_CONFIRM];
                if ((safetyConfirm & KeyValue.AXIS_BIN) != 0)
                {
                    return safetyConfirm;
                }
            }
            else if (actIn == ControlAction.BACK)
            {
                KeyValue safetyBack = DigitalActionValue[ControlAction.SAFETY_BACK];
                if ((safetyBack & KeyValue.AXIS_BIN) != 0)
                {
                    return safetyBack;
                }
            }

            return DigitalActionValue[actIn];
        }
        return KeyValue.BTN_NOT_FOUND;
    }

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

    public static float GetAnalogInputAction(ControlAction actIn)
    {
        float returnValue = float.MaxValue;
        if (AnalogActionInput.ContainsKey(actIn))
        {
            if (GetBinaryAnalogAction(actIn, out returnValue))
            {
                return returnValue;
            }
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
        if (!DigitalActionInput.ContainsKey(actIn))
        {
            return;
        }
        KeyCode keyIn = DigitalActionInput[actIn];
        KeyValue frameValue = DigitalActionValue[actIn];
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

        DigitalActionValue[actIn] = frameValue;
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
        if (DigitalActionInput.ContainsKey(actIn))
        {
            DigitalActionValue[actIn] = KeyValue.IDLE;
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

        CheckAndSetKeyValue(ControlAction.SAFETY_CONFIRM);
        CheckAndSetKeyValue(ControlAction.SAFETY_BACK);
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

        ResetKey(ControlAction.SAFETY_CONFIRM);
        ResetKey(ControlAction.SAFETY_BACK);
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
        if (DigitalActionInput.ContainsKey(actIn))
        {
            DigitalActionInput.Remove(actIn);
            DigitalActionValue.Remove(actIn);

            // reset value
            DigitalActionInput[actIn] = keyIn;
            DigitalActionValue[actIn] = KeyValue.IDLE;

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
        if (DigitalActionInput.ContainsValue(keyIn))
        {
           foreach(ControlAction action in DigitalActionInput.Keys)
           {
                if (DigitalActionInput[action] == keyIn)
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

    public KeyCode SafetyConfirm;
    public KeyCode SafetyBack;

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

        KeyboardLayout.DefaultTuck = KeyCode.E;
        KeyboardLayout.DefaultBack = KeyCode.L;
        KeyboardLayout.DefaultConfirm = KeyCode.K;

        KeyboardLayout.DefaultBinU = KeyCode.UpArrow;
        KeyboardLayout.DefaultBinR = KeyCode.RightArrow;
        KeyboardLayout.DefaultBinL = KeyCode.LeftArrow;
        KeyboardLayout.DefaultBinD = KeyCode.DownArrow;

        KeyboardLayout.SafetyConfirm = KeyCode.Return;
        KeyboardLayout.SafetyBack = KeyCode.Backspace;

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

        PSLayout.SafetyConfirm = KeyCode.Return;
        PSLayout.SafetyBack = KeyCode.Backspace;

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

        XboxLayout.SafetyConfirm = KeyCode.Return;
        XboxLayout.SafetyBack = KeyCode.Backspace;

        XboxLayout.DefaultLHoriz = "Horizontal";
        XboxLayout.DefaultLVerti = "Vertical";
    }

    public static void InitializeMaps()
    {
        XboxToPS = new Dictionary<KeyCode, KeyCode>();
        PSToXbox = new Dictionary<KeyCode, KeyCode>();
    }
}

