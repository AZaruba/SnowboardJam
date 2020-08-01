using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinIdleState : iState
{
    private TrickPhysicsData c_trickPhys;

    public SpinIdleState(ref TrickPhysicsData dataIn)
    {
        this.c_trickPhys = dataIn;
    }

    public void Act()
    {
        
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.CHARGE)
        {
            return StateRef.SPIN_CHARGE;
        }
        return StateRef.SPIN_IDLE;
    }

    public void TransitionAct()
    {
        c_trickPhys.f_currentFlipCharge = 0.0f;
        c_trickPhys.f_currentSpinCharge = 0.0f;
        c_trickPhys.f_currentFlipRate = 0.0f;
        c_trickPhys.f_currentSpinRate = 0.0f;
    }
}
