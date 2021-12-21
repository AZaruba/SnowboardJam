using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardState : iState
{
    private PlayerPositionData c_positionData;

    public ForwardState(ref PlayerPositionData positionDataIn)
    {
        c_positionData = positionDataIn;
    }

    public void Act()
    {
        // no actions taken currently
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.SWITCH_STANCE)
        {
            return StateRef.SWITCH_STANCE;
        }
        return StateRef.FORWARD_STANCE;
    }

    public void TransitionAct()
    {
        c_positionData.i_switchStance = Constants.FORWARD_STANCE;
    }

}

public class SwitchState : iState
{
    private PlayerPositionData c_positionData;

    public SwitchState(ref PlayerPositionData positionDataIn)
    {
        c_positionData = positionDataIn;
    }

    public void Act()
    {
        // no actions taken currently
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.SWITCH_STANCE)
        {
            return StateRef.FORWARD_STANCE;
        }
        return StateRef.SWITCH_STANCE;
    }

    public void TransitionAct()
    {
        c_positionData.i_switchStance = Constants.SWITCH_STANCE;
    }

}
