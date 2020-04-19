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

    private static ControllerInputData ControllerData;

    private static Dictionary<KeyCode, KeyValue> DigitalInputData;
    private static Dictionary<string, float> AnalogInputData;
    // TODO: Add menu analog input (the single tick then "scroll" update)

    public static bool InitializeInput(ControllerInputData dataIn)
    {
        ControllerData = dataIn;

        if (DigitalInputData != null && AnalogInputData != null)
        {
            return false;
        }
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

        return true;
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
    [SerializeField] ControllerInputData ControllerData;

    private void Start()
    {
        GlobalInputController.InitializeInput(ControllerData);
    }

    private void Update()
    {
        GlobalInputController.UpdateInput();
    }
}

