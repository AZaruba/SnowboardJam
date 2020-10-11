using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionListEditController : iEditController
{
    [SerializeField] Text ValueDisplay;

    private int i_currentValue;
    private int i_lastStoredValue;
    private Resolution[] l_resolutions;

    private IncrementCartridge cart_incr;

    private void Start()
    {
        l_resolutions = Screen.resolutions;
        i_currentValue = FindCurrentResolutionIndex();

        InitializeCarts();
        InitializeData();
        InitializeStateMachine();
        // get the settings resolution
        c_controllerData.i = GlobalGameData.GetSettingsInt(CurrentTarget);

        // if the resolution isn't valid...
        if (c_controllerData.i < 0 || c_controllerData.i >= l_resolutions.Length)
        {
            // get the arbitary value already loaded
            c_controllerData.i = i_currentValue;
        }
        ValueDisplay.text = l_resolutions[c_controllerData.i].ToString();

    }

    public override void CancelDataEdit()
    {
        c_controllerData.i = i_lastStoredValue;
        EnginePush();
        MessageServer.SendMessage(MessageID.EDIT_END, new Message(c_controllerData.i));
    }

    public override void ConfirmDataEdit(DataTarget targetIn)
    {
        MessageServer.SendMessage(MessageID.EDIT_END, new Message(c_controllerData.i));
        GlobalGameData.SetSettingsValue(targetIn, c_controllerData.i);
    }

    // Update is called once per frame
    public override void UpdateEditor()
    {
        if (c_controllerData.b_editorActive == false)
        {
            return;
        }

        float inputAxisValue = GlobalInputController.GetAnalogInputAction(ControlAction.SPIN_AXIS);
        if (inputAxisValue < -0.5f)
        {
            c_controllerData.b_increasing = false;
            sm_editController.Execute(Command.MENU_TICK_INPUT);
        }
        else if (inputAxisValue > 0.5f)
        {
            c_controllerData.b_increasing = true;
            sm_editController.Execute(Command.MENU_TICK_INPUT);
        }
        else
        {
            // no input, unready
        }

        if (float.Equals(c_controllerData.f_currentTickTime, c_controllerData.f_maxTickTime))
        {
            sm_editController.Execute(Command.MENU_READY);
        }
        else
        {
            sm_editController.Execute(Command.MENU_IDLE);
        }

        if (GlobalInputController.GetInputAction(ControlAction.CONFIRM) == KeyValue.PRESSED)
        {
            ConfirmDataEdit(CurrentTarget);
            Deactivate();
        }

        if (GlobalInputController.GetInputAction(ControlAction.BACK) == KeyValue.PRESSED)
        {
            CancelDataEdit();
            Deactivate();
        }

        sm_editController.Act();
        EnginePush();
    }

    public void EnginePush()
    {
        ValueDisplay.text = l_resolutions[c_controllerData.i].ToString();
    }

    public void InitializeCarts()
    {
        cart_incr = new IncrementCartridge();
    }

    public override void InitializeStateMachine()
    {
        DataEditDisabledState disabledState = new DataEditDisabledState(ref c_controllerData);
        DataEditReadyState readyState = new DataEditReadyState(ref c_controllerData);
        DataEditWaitState waitState = new DataEditWaitState(ref c_controllerData, ref cart_incr);
        IntEditTickState tickState = new IntEditTickState(ref c_controllerData, ref cart_incr);

        sm_editController = new StateMachine(disabledState, StateRef.MENU_DISABLED);
        sm_editController.AddState(readyState, StateRef.MENU_READY);
        sm_editController.AddState(waitState, StateRef.MENU_WAIT);
        sm_editController.AddState(tickState, StateRef.MENU_TICK);
    }

    public override void InitializeData()
    {
        c_controllerData = new EditControllerData();
        c_controllerData.b_editorActive = false;
        c_controllerData.b = default;

        c_controllerData.i = i_currentValue;
        c_controllerData.i_max = l_resolutions.Length - 1;
        c_controllerData.i_min = Constants.ZERO;

        c_controllerData.f = default;
        c_controllerData.res = default;

        c_controllerData.f_currentTickTime = Constants.ZERO_F;
        c_controllerData.f_maxTickTime = Constants.LONG_DATA_EDIT_TICK_TIME;
    }

    private bool VerifyResolutionIndex()
    {
        if (c_controllerData.i < 0 || c_controllerData.i >= l_resolutions.Length)
        {

        }
        for (int i = 0; i < l_resolutions.Length; i++)
        {
            Resolution comPres = l_resolutions[i];
        }

        return false;
    }

    private int FindCurrentResolutionIndex()
    {
        Resolution currentResolution = Screen.currentResolution;
        for (int i = 0; i < l_resolutions.Length; i++)
        {
            Resolution compRes = l_resolutions[i];
            if (compRes.Equals(currentResolution))
            {
                return i;
            }
        }
        return 0;
    }
}
