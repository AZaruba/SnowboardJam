using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum KeyValue
{
    BTN_NOT_FOUND,
    IDLE,
    UP,
    PRESSED,
    HELD,
    NEED_RESET,
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

    CONFIRM,
    BACK,
    PAUSE,
}

public static class GlobalInputController
{

    public static ControllerInputData ControllerData;
    private static bool InputInitialized = false;

    private static bool InputLocked = false;
    private static bool InputToFlush = false;
    private static float InputLockTimer = 0.0f;
    private static float InputLockTimeLimit = 0.25f;

    private static Dictionary<ControlAction, KeyCode> DigitalActionInput;
    private static Dictionary<ControlAction, KeyValue> DigitalActionValue;
    private static Dictionary<ControlAction, string> AnalogActionInput;
    private static Dictionary<ControlAction, float> AnalogActionValue;
    //private static Dictionary<KeyCode, KeyValue> DigitalInputData;
    //private static Dictionary<string, float> AnalogInputData;

    public static bool InitializeInput()
    {
        if (InputInitialized)
        {
            return false;
        }

        DefineInputs();

        InputInitialized = true;
        return true;
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

        AnalogActionInput[ControlAction.SPIN_AXIS] = DefaultControls.DefaultLHoriz;
        AnalogActionInput[ControlAction.TURN_AXIS] = DefaultControls.DefaultLHoriz;
        AnalogActionInput[ControlAction.FLIP_AXIS] = DefaultControls.DefaultLVerti;
        AnalogActionInput[ControlAction.SLOW_AXIS] = DefaultControls.DefaultLVerti;

        AnalogActionValue[ControlAction.SPIN_AXIS] = Constants.ZERO_F;
        AnalogActionValue[ControlAction.TURN_AXIS] = Constants.ZERO_F;
        AnalogActionValue[ControlAction.FLIP_AXIS] = Constants.ZERO_F;
        AnalogActionValue[ControlAction.SLOW_AXIS] = Constants.ZERO_F;
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
            return DigitalActionValue[actIn];
        }
        return KeyValue.BTN_NOT_FOUND;
    }

    public static float GetAnalogInputAction(ControlAction actIn)
    {
        if (AnalogActionInput.ContainsKey(actIn))
        {
            return AnalogActionValue[actIn];
        }
        return float.MaxValue;
    }

    public static void CheckAndSetKeyValue(ControlAction actIn)
    {
        if (!DigitalActionInput.ContainsKey(actIn))
        {
            return;
        }
        KeyCode keyIn = DigitalActionInput[actIn];
        KeyValue frameValue = KeyValue.IDLE;

        if (Input.GetKeyDown(keyIn))
        {
            frameValue = KeyValue.PRESSED;
        }
        else if (Input.GetKey(keyIn))
        {
            frameValue = KeyValue.HELD;
        }
        else if (Input.GetKeyUp(keyIn))
        {
            frameValue = KeyValue.UP;
        }

        DigitalActionValue[actIn] = frameValue;
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

    public static KeyCode GetAnyKey()
    {
        if (Input.anyKeyDown)
        {
            foreach(KeyCode keyIn in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyIn))
                {
                    return keyIn;
                }
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

    private void Update()
    {
        GlobalInputController.FlushInputs();
        if (GlobalInputController.InputIsLocked())
        {
            return;
        }
        GlobalInputController.UpdateInput();
    }
}

public static class DefaultControls
{
    public static KeyCode DefaultPause = KeyCode.P;
    public static KeyCode DefaultTrickU = KeyCode.I;
    public static KeyCode DefaultTrickR = KeyCode.L;
    public static KeyCode DefaultTrickL = KeyCode.J;
    public static KeyCode DefaultTrickD = KeyCode.K;
    public static KeyCode DefaultJump = KeyCode.Space;
    public static KeyCode DefaultTuck = KeyCode.E;
    public static KeyCode DefaultBack = KeyCode.L;
    public static KeyCode DefaultConfirm = KeyCode.K;

    public static string DefaultLHoriz = "Horizontal";
    public static string DefaultLVerti = "Vertical";
}

