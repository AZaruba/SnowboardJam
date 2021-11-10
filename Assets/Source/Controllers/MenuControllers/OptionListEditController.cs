using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionListEditController : IntegerEditController
{
    [SerializeField] int[] MappedValues;

    private void Start()
    {
        base.InitializeCarts();
        InitializeData();
        base.InitializeStateMachine();

        int optionValue  = GlobalGameData.GetSettingsInt(CurrentTarget);
        for (int i = 0; i < MappedValues.Length; i++)
        {
            if (optionValue == MappedValues[i])
            {
                c_controllerData.i = i;
                break;
            }
        }

        ValueDisplay.text = MappedValues[c_controllerData.i].ToString() + "x";
    }

    public override void CancelDataEdit()
    {
        c_controllerData.i = i_lastStoredValue;
        EnginePush();
        MessageServer.SendMessage(MessageID.EDIT_END, new Message(MappedValues[c_controllerData.i]));
    }

    public override void ConfirmDataEdit(DataTarget targetIn)
    {
        MessageServer.SendMessage(MessageID.EDIT_END, new Message(MappedValues[c_controllerData.i]));
        GlobalGameData.SetSettingsValue(targetIn, MappedValues[c_controllerData.i]);
    }

    public override void EnginePush()
    {
        ValueDisplay.text = MappedValues[c_controllerData.i].ToString() + "x";
    }

    public override void InitializeData()
    {
        c_controllerData = new EditControllerData();
        c_controllerData.b_editorActive = false;
        c_controllerData.b = default;

        c_controllerData.i = DefaultValue;
        c_controllerData.i_max = MappedValues.Length - 1;
        c_controllerData.i_min = Constants.ZERO;

        c_controllerData.f = default;
        c_controllerData.res = default;

        c_controllerData.f_currentTickTime = Constants.ZERO_F;
        c_controllerData.f_maxTickTime = Constants.LONG_DATA_EDIT_TICK_TIME;
    }
}
