using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : iState
{

    private PlayerData c_playerData;
    public GroundedState(ref PlayerData playerData)
    {
        this.c_playerData = playerData;
    }
    public void Act()
    {

    }

    public void TransitionAct()
    {
        c_playerData.f_currentJumpCharge = Constants.ZERO_F;
        c_playerData.f_currentAirVelocity = Constants.ZERO_F;

        // add raycast adjustment on land
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.CHARGE)
        {
            return StateRef.CHARGING;
        }
        if (cmd == Command.FALL)
        {
            return StateRef.AIRBORNE;
        }
        if (cmd == Command.CRASH)
        {
            return StateRef.DISABLED;
        }
        return StateRef.GROUNDED;
    }
}
