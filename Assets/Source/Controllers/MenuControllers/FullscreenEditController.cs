using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenEditController : IntegerEditController
{
    public void Start()
    {
        InitializeCarts();
        InitializeData();
        InitializeStateMachine();
        ValueDisplay.text = GetMappedText();

    }
    public override void EnginePush()
    {
        ValueDisplay.text = GetMappedText();
    }

    private string GetMappedText()
    {
        switch(c_controllerData.i)
        {
            case 0:
                return "Fullscreen";
            case 1:
                return "Windowed Fullscreen";
            case 2:
                return "Maximized Window";
            case 3:
                return "Windowed";
        }
        return "";
    }

    public override void InitializeData()
    {
        c_controllerData = new EditControllerData();
        c_controllerData.b_editorActive = false;
        c_controllerData.b = default;

        c_controllerData.i = GlobalGameData.GetSettingsInt(CurrentTarget);
        c_controllerData.i_max = (int)FullScreenMode.Windowed;
        c_controllerData.i_min = Constants.ZERO;

        c_controllerData.f = default;
        c_controllerData.res = default;

        c_controllerData.f_currentTickTime = Constants.ZERO_F;
        c_controllerData.f_maxTickTime = Constants.LONG_DATA_EDIT_TICK_TIME;
    }
}
