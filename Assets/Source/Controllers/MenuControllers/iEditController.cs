using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class iEditController : MonoBehaviour
{
    [SerializeField] TextMenuItemController parent;

    private DataTarget CurrentTarget;
    private StateMachine sm_editController;

    private void Start()
    {
        InitializeStateMachine();
    }

    private void Update()
    {
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
    }

    public abstract void ConfirmDataEdit(DataTarget targetIn);
    public abstract void CancelDataEdit();

    public virtual void Activate(DataTarget targetIn)
    {
        // update state machine
        this.CurrentTarget = targetIn;
        sm_editController.Execute(Command.MENU_SHOW);
    }

    public virtual void Deactivate()
    {
        sm_editController.Execute(Command.MENU_HIDE);
    }    

    private void InitializeStateMachine()
    {
        sm_editController = new StateMachine();
    }
}

/* TODO:
 * 
 * 1) Implement float, int, resolution, and boolean edit
 * 2) Create "deactivated" state for menu that prevents
 *    The menu from being manipulated
 * 3) update value and save PlayerPrefs on confirm or exit
 * 4) Add "default" that checks if PlayerPrefs values have
 *    been initialized and adds default values if not
 */ 
