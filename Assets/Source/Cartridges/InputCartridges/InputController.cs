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
}
public static class GlobalInputController
{

    public static ControllerInputData ControllerData;
    private static bool InputInitialized = false;

    private static Dictionary<KeyCode, KeyValue> DigitalInputData;
    private static Dictionary<string, float> AnalogInputData;

    public static bool InitializeInput()
    {
        if (InputInitialized)
        {
            return false;
        }

        DefineInputs();

        DigitalInputData = new Dictionary<KeyCode, KeyValue>();

        DigitalInputData.Add(ControllerData.JumpButton, KeyValue.IDLE);
        DigitalInputData.Add(ControllerData.LTrickButton, KeyValue.IDLE);
        DigitalInputData.Add(ControllerData.DTrickButton, KeyValue.IDLE);
        DigitalInputData.Add(ControllerData.PauseButton, KeyValue.IDLE);

        DigitalInputData.Add(ControllerData.JumpKey, KeyValue.IDLE);
        DigitalInputData.Add(ControllerData.LTrickKey, KeyValue.IDLE);
        DigitalInputData.Add(ControllerData.DTrickKey, KeyValue.IDLE);
        DigitalInputData.Add(ControllerData.PauseKey, KeyValue.IDLE);

        AnalogInputData = new Dictionary<string, float>();

        AnalogInputData.Add(ControllerData.LeftHorizontalAxis, 0);
        AnalogInputData.Add(ControllerData.LeftVerticalAxis, 0);

        InputInitialized = true;
        return true;
    }

    public static void DefineInputs()
    {
        ControllerData = new ControllerInputData();

        ControllerData.JumpButton = DefaultControls.DefaultJump;
        ControllerData.JumpKey = DefaultControls.DefaultJumpKey;

        ControllerData.TuckButton = DefaultControls.DefaultTuck;
        ControllerData.TuckKey = DefaultControls.DefaultTuckKey;

        ControllerData.DTrickButton = DefaultControls.DefaultTrickD;
        ControllerData.DTrickKey = DefaultControls.DefaultTrickDKey;

        ControllerData.LTrickButton = DefaultControls.DefaultTrickL;
        ControllerData.LTrickKey = DefaultControls.DefaultTrickLKey;

        ControllerData.RTrickButton = DefaultControls.DefaultTrickR;
        ControllerData.RTrickKey = DefaultControls.DefaultTrickRKey;

        ControllerData.UTrickButton = DefaultControls.DefaultTrickU;
        ControllerData.UTrickKey = DefaultControls.DefaultTrickUKey;

        ControllerData.PauseButton = DefaultControls.DefaultPause;
        ControllerData.PauseKey = DefaultControls.DefaultPauseKey;

        ControllerData.LeftHorizontalAxis = DefaultControls.DefaultLHoriz;
        ControllerData.LeftVerticalAxis = DefaultControls.DefaultLVerti;
    }

    public static KeyValue GetInputValue(KeyCode keyIn)
    {
        if (DigitalInputData.ContainsKey(keyIn))
        {
            return DigitalInputData[keyIn];
        }
        return KeyValue.BTN_NOT_FOUND;
    }

    public static float GetInputValue(string axisIn)
    {
        if (AnalogInputData.ContainsKey(axisIn))
        {
            return AnalogInputData[axisIn];
        }
        return float.MaxValue;
    }

    public static void CheckAndSetValue(KeyCode keyIn)
    {
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

        if (DigitalInputData.ContainsKey(keyIn))
        {
            DigitalInputData[keyIn] = frameValue;
        }
    }

    public static void CheckAndSetValue(string axisNameIn)
    {
        float frameValue = Input.GetAxis(axisNameIn);

        if (AnalogInputData.ContainsKey(axisNameIn))
        {
            AnalogInputData[axisNameIn] = frameValue;
        }
    }

    public static void UpdateInput()
    {
        CheckAndSetValue(ControllerData.JumpButton);
        CheckAndSetValue(ControllerData.JumpKey);

        CheckAndSetValue(ControllerData.LTrickButton);
        CheckAndSetValue(ControllerData.LTrickKey);

        CheckAndSetValue(ControllerData.DTrickButton);
        CheckAndSetValue(ControllerData.DTrickKey);

        CheckAndSetValue(ControllerData.PauseButton);
        CheckAndSetValue(ControllerData.PauseKey);

        CheckAndSetValue(ControllerData.LeftVerticalAxis);
        CheckAndSetValue(ControllerData.LeftHorizontalAxis);
    }
}

public class InputController : MonoBehaviour
{
    private void Start()
    {
        GlobalInputController.InitializeInput();
    }

    private void Update()
    {
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
    public static string DefaultLHoriz = "Horizontal";
    public static string DefaultLVerti = "Vertical";

    public static KeyCode DefaultPauseKey = KeyCode.Alpha0;
    public static KeyCode DefaultTrickUKey = KeyCode.Alpha1;
    public static KeyCode DefaultTrickRKey = KeyCode.Alpha2;
    public static KeyCode DefaultTrickLKey = KeyCode.Alpha3;
    public static KeyCode DefaultTrickDKey = KeyCode.Alpha4;
    public static KeyCode DefaultJumpKey = KeyCode.Alpha5;
    public static KeyCode DefaultTuckKey = KeyCode.Alpha6;

}

