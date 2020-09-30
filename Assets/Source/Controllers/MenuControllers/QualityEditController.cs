using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityEditController : IntegerEditController
{
    public void Start()
    {
        InitializeCarts();
        InitializeData();
        InitializeStateMachine();
        ValueDisplay.text = QualitySettings.names[c_controllerData.i];

    }
    public override void EnginePush()
    {
        ValueDisplay.text = QualitySettings.names[c_controllerData.i];
    }

    public override void InitializeData()
    {
        c_controllerData = new EditControllerData();
        c_controllerData.b_editorActive = false;
        c_controllerData.b = default;

        c_controllerData.i = GlobalGameData.GetSettingsInt(CurrentTarget);
        c_controllerData.i_max = QualitySettings.names.Length - 1;
        c_controllerData.i_min = Constants.ZERO;

        c_controllerData.f = default;
        c_controllerData.res = default;

        c_controllerData.f_currentTickTime = Constants.ZERO_F;
        c_controllerData.f_maxTickTime = Constants.LONG_DATA_EDIT_TICK_TIME;
    }
}
