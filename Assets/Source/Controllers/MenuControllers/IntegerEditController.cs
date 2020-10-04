using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class IntegerEditController : iEditController
{
    [SerializeField] public int DefaultValue;
    [SerializeField] public int MinimumValue;
    [SerializeField] public int MaximumValue;
    [SerializeField] public Text ValueDisplay;

    public int i_currentValue;
    public int i_lastStoredValue;


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

    public virtual void EnginePush()
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

        // hack fix: reset the menu action keys as the Video Quality Edit Controller seems to keep the button held on confirmation
        GlobalInputController.ResetKey(ControlAction.CONFIRM);
        GlobalInputController.ResetKey(ControlAction.BACK);
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
