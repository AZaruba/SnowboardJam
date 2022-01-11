using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCorrectState : iState
{
    private TrickPhysicsData c_trickPhys;
    private PlayerData c_playerData;
    private PlayerPositionData c_posData;

    public SpinCorrectState(ref TrickPhysicsData dataIn, ref PlayerData playerData,ref PlayerPositionData posDataIn)
    {
        this.c_trickPhys = dataIn;
        this.c_playerData = playerData;
        this.c_posData = posDataIn;
    }

    public void Act()
    {
        Quaternion currentModelRotation = c_posData.q_currentModelRotation;
        float frameSpinValue = c_trickPhys.f_groundResetRate * 360f * c_trickPhys.i_groundResetDir * Time.fixedDeltaTime;

        // round off the last frame of rotation if we hit our target
        if (Quaternion.Angle(c_playerData.q_currentRotation, c_posData.q_currentModelRotation) < Constants.ROTATION_TOLERANCE)
        {
            c_trickPhys.f_groundResetTarget = Constants.ZERO_F;
        }
        /*
        if (Mathf.Abs(c_trickPhys.f_groundResetRotation + frameSpinValue) > Mathf.Abs(c_trickPhys.f_groundResetTarget))
        {
            frameSpinValue = c_trickPhys.f_groundResetTarget - c_trickPhys.f_groundResetRotation;
            c_trickPhys.f_groundResetTarget = Constants.ZERO_F;
        }
        */

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
        if (cmd == Command.FALL)
        {
            return StateRef.SPIN_IDLE;
        }
        return StateRef.SPIN_CORRECT;
    }

    public void TransitionAct()
    {
        c_trickPhys.f_groundResetTarget = c_trickPhys.f_currentSpinDegrees % 180;
        c_trickPhys.i_groundResetDir = Mathf.FloorToInt(Mathf.Sign(c_trickPhys.f_groundResetTarget));

        c_trickPhys.f_currentFlipCharge = Constants.ZERO_F;
        c_trickPhys.f_currentSpinCharge = Constants.ZERO_F;
        c_trickPhys.f_currentFlipRate = Constants.ZERO_F;
        c_trickPhys.f_currentSpinRate = Constants.ZERO_F;

        c_trickPhys.f_currentFlipDegrees = Constants.ZERO_F;
        c_trickPhys.f_currentSpinDegrees = Constants.ZERO_F;

        // reset center of gravity rotation to the current rotation on land
        c_posData.q_centerOfGravityRotation = Quaternion.identity;

        // find the reset target
        /* Reset Target defined as:
         * - on land, we are rotating in some direction defined by the sign of the spin rate
         * - there is an angle in that direction to get to our desired forward defined by rotation * forward * switch
         * - that angle is our target correction
         */
        c_trickPhys.f_groundResetRotation = Constants.ZERO_F;
    }
}
