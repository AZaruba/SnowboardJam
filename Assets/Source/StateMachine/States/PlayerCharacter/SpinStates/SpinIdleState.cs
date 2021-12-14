using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinIdleState : iState
{
    private TrickPhysicsData c_trickPhys;
    private PlayerPositionData c_posData;
    private ScoringData c_scoring;

    public SpinIdleState(ref TrickPhysicsData dataIn, ref ScoringData scoringIn, ref PlayerPositionData posDataIn)
    {
        this.c_trickPhys = dataIn;
        this.c_scoring = scoringIn;
        this.c_posData = posDataIn;
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

        // reset center of gravity rotation to the current rotation on land
        c_posData.q_centerOfGravityRotation = Quaternion.identity;
    }
}
