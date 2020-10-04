using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEditController : iEditController
{
    [SerializeField] public ControlAction InputAction;
    public override void CancelDataEdit()
    {
        throw new System.NotImplementedException();
    }

    public override void ConfirmDataEdit(DataTarget targetIn)
    {
        throw new System.NotImplementedException();
    }

    public override void InitializeData()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeData();
        InitializeStateMachine();
    }

    // Update is called once per frame
    void Update()
    {
        KeyCode keyIn = GlobalInputController.GetAnyKey();
        if (keyIn != KeyCode.None)
        {
            GlobalInputController.UpdateAction(InputAction, keyIn);
        }
    }
}
