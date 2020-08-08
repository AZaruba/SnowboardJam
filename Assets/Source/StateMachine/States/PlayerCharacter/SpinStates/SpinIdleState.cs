using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinIdleState : iState
{
    private TrickPhysicsData c_trickPhys;
    private ScoringData c_scoring;

    public SpinIdleState(ref TrickPhysicsData dataIn, ref ScoringData scoringIn)
    {
        this.c_trickPhys = dataIn;
        this.c_scoring = scoringIn;
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

        c_scoring.f_currentFlipDegrees = 0.0f;
        c_scoring.f_currentSpinDegrees = 0.0f;
        c_scoring.f_currentFlipTarget = 0.0f;
        c_scoring.f_currentSpinTarget = 0.0f;
    }
}
