using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class iEditController : MonoBehaviour
{
    [SerializeField] TextMenuItemController parent;
    [SerializeField] public BasicMenuControllerData ControllerData;

    public DataTarget CurrentTarget;
    public StateMachine sm_editController;
    public EditControllerData c_controllerData;

    public IncrementCartridge cart_incr;

    private void Start()
    {
        InitializeData();
        InitializeStateMachine();
    }

    private void Update()
    {
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
    }

    public abstract void ConfirmDataEdit(DataTarget targetIn);
    public abstract void CancelDataEdit();
    public abstract void InitializeData();

    public virtual void Activate(DataTarget targetIn)
    {
        // update state machine
        this.CurrentTarget = targetIn;
        sm_editController.Execute(Command.MENU_SHOW);
    }

    public virtual void Deactivate()
    {
        sm_editController.Execute(Command.MENU_HIDE);
        GlobalInputController.FlushNextFrame();
    }    

    public virtual void InitializeStateMachine()
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

}