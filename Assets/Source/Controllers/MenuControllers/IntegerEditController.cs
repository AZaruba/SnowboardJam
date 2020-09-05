using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class IntegerEditController : iEditController
{
    [SerializeField] int DefaultValue;
    [SerializeField] int MinimumValue;
    [SerializeField] int MaximumValue;
    [SerializeField] Text ValueDisplay;

    private int i_currentValue;
    private int i_lastStoredValue;

    private IncrementCartridge cart_incr;

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

    private void Start()
    {
        
        InitializeCarts();
        InitializeData();
        InitializeStateMachine();
        c_controllerData.i = GlobalGameData.GetSettingsInt(CurrentTarget);
        ValueDisplay.text = c_controllerData.i.ToString();
    }

    private void Update()
    {
        if (c_controllerData.b_editorActive == false)
        {
            return;
        }

        float inputAxisValue = GlobalInputController.GetInputValue(GlobalInputController.ControllerData.LeftHorizontalAxis);
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

        if (GlobalInputController.GetInputValue(GlobalInputController.ControllerData.DTrickButton) == KeyValue.PRESSED)
        {
            ConfirmDataEdit(CurrentTarget);
            Deactivate();
        }

        if (GlobalInputController.GetInputValue(GlobalInputController.ControllerData.RTrickButton) == KeyValue.PRESSED)
        {
            CancelDataEdit();
            Deactivate();
        }

        sm_editController.Act();
        EnginePush();
    }

    public void EnginePush()
    {
        ValueDisplay.text = c_controllerData.i.ToString();
    }

    public override void Activate(DataTarget targetIn)
    {
        // update state machine
        this.CurrentTarget = targetIn;
        sm_editController.Execute(Command.MENU_SHOW);
        i_lastStoredValue = c_controllerData.i;
    }

    public override void Deactivate()
    {
        sm_editController.Execute(Command.MENU_HIDE);
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

        c_controllerData.i = DefaultValue;
        c_controllerData.i_max = MaximumValue;
        c_controllerData.i_min = MinimumValue;

        c_controllerData.f = default;
        c_controllerData.res = default;

        c_controllerData.f_currentTickTime = Constants.ZERO_F;
        c_controllerData.f_maxTickTime = Constants.DATA_EDIT_TICK_TIME;
    }
}
