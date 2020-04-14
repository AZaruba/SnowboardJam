using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : iState
{

    private PlayerData c_playerData;
    private VelocityCartridge cart_velocity;

    public GroundedState(ref PlayerData playerData, ref VelocityCartridge vel)
    {
        this.c_playerData = playerData;
        this.cart_velocity = vel;
    }
    public void Act()
    {

    }

    public void TransitionAct()
    {
        Vector3 currentPosition = c_playerData.v_currentPosition;
        Vector3 previousPosition = currentPosition;
        Vector3 currentNormal = c_playerData.v_currentNormal;
        Vector3 currentSurfaceNormal = c_playerData.v_currentSurfaceNormal;
        Vector3 currentSurfacePosition = c_playerData.v_currentSurfaceAttachPoint;
        Quaternion currentRotation = c_playerData.q_currentRotation;

        c_playerData.f_currentJumpCharge = Constants.ZERO_F;
        c_playerData.f_currentAirVelocity = Constants.ZERO_F;
        cart_velocity.SurfaceAdjustment(ref currentPosition, previousPosition, currentSurfacePosition, Vector3.down, currentSurfaceNormal, currentRotation);

        c_playerData.v_currentPosition = currentPosition;
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
