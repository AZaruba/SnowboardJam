using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCorrectState : iState
{
    private TrickPhysicsData c_trickPhys;
    private PlayerData c_playerData;
    private PlayerPositionData c_posData;
    private ScoringData c_scoring;

    public SpinCorrectState(ref TrickPhysicsData dataIn, ref PlayerData playerData, ref ScoringData scoringIn, ref PlayerPositionData posDataIn)
    {
        this.c_trickPhys = dataIn;
        this.c_playerData = playerData;
        this.c_scoring = scoringIn;
        this.c_posData = posDataIn;
    }

    public void Act()
    {
        Quaternion currentModelRotation = c_posData.q_currentModelRotation;
        float frameSpinValue = c_trickPhys.f_currentSpinRate * Time.deltaTime;

        // round off the last frame of rotation if we hit our target
        if (c_trickPhys.f_groundResetRotation + frameSpinValue > c_trickPhys.f_groundResetTarget)
        {
            frameSpinValue = c_trickPhys.f_groundResetTarget - c_trickPhys.f_groundResetRotation;
        }

        HandlingCartridge.Turn(c_playerData.q_currentRotation * Vector3.up, frameSpinValue, ref currentModelRotation);

        c_posData.q_currentModelRotation = currentModelRotation;
        c_trickPhys.f_groundResetRotation += frameSpinValue;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.SPIN_CORRECT_END)
        {
            return StateRef.SPIN_IDLE;
        }
        return StateRef.SPIN_CORRECT;
    }

    public void TransitionAct()
    {
        c_trickPhys.f_currentFlipCharge = 0.0f;
        c_trickPhys.f_currentSpinCharge = 0.0f;
        c_trickPhys.f_currentFlipRate = 0.0f;

        c_scoring.f_currentFlipDegrees = 0.0f;
        c_scoring.f_currentSpinDegrees = 0.0f;

        // reset center of gravity rotation to the current rotation on land
        c_posData.q_centerOfGravityRotation = Quaternion.identity;

        // find the reset target
        /* Reset Target defined as:
         * - on land, we are rotating in some direction defined by the sign of the spin rate
         * - there is an angle in that direction to get to our desired forward defined by rotation * forward * switch
         * - that angle is our target correction
         */ 

        // reset the spin rate after because we want to rotate in this direction
        c_trickPhys.f_currentSpinRate = c_trickPhys.f_groundResetRate;
    }
}
