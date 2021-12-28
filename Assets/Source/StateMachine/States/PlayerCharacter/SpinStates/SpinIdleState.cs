using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinIdleState : iState
{
    private TrickPhysicsData c_trickPhys;
    private PlayerPositionData c_posData;

    public SpinIdleState(ref TrickPhysicsData dataIn, ref PlayerPositionData posDataIn)
    {
        this.c_trickPhys = dataIn;
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
        c_trickPhys.f_currentFlipCharge = Constants.ZERO_F;
        c_trickPhys.f_currentSpinCharge = Constants.ZERO_F;
        c_trickPhys.f_currentFlipRate = Constants.ZERO_F;
        c_trickPhys.f_currentSpinRate = Constants.ZERO_F;

        c_trickPhys.f_currentFlipDegrees = Constants.ZERO_F;
        c_trickPhys.f_currentSpinDegrees = Constants.ZERO_F;

        c_trickPhys.f_groundResetRotation = Constants.ZERO_F;
        c_trickPhys.f_groundResetTarget = Constants.ZERO_F;

        // reset center of gravity rotation to the current rotation on land
        c_posData.q_centerOfGravityRotation = Quaternion.identity;
    }
}
