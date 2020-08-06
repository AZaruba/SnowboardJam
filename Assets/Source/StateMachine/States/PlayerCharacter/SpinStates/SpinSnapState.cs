using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The state the player will be in after letting go of the spin direction while
/// spinning. Will snap the player back to the ready state quickly a la SSX
/// </summary>
public class SpinSnapState : iState
{

    private AerialMoveData c_aerialMoveData;
    private PlayerPositionData c_playerPosData;
    private TrickPhysicsData c_trickPhys;
   
    private QuaternionCartridge cart_quatern;

    public SpinSnapState(ref AerialMoveData aerialIn,
        ref PlayerPositionData playerPosIn,
        ref TrickPhysicsData trickIn,
        ref QuaternionCartridge quaternIn)
    {
        this.c_aerialMoveData = aerialIn;
        this.c_playerPosData = playerPosIn;
        this.c_trickPhys = trickIn;
        this.cart_quatern = quaternIn;
    }

    public void Act()
    {
        // Vector3 targetOrientation = c_aerialMoveData.v_lateralDirection;
        Quaternion targetOrientation = Quaternion.LookRotation(c_aerialMoveData.v_lateralDirection, Vector3.up);
        Quaternion currentOrientation = c_playerPosData.q_currentModelRotation;

        // move SNAP * TIME distance to the target
        cart_quatern.ApproachQuaternion(ref currentOrientation, targetOrientation, c_trickPhys.f_resetRate * Time.deltaTime);

        c_playerPosData.q_currentModelRotation = currentOrientation;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.CRASH ||
            cmd == Command.LAND)
        {
            return StateRef.SPIN_IDLE;
        }
        return StateRef.SPIN_RESET;
    }

    public void TransitionAct()
    {
        // nothing to do!
    }
}
