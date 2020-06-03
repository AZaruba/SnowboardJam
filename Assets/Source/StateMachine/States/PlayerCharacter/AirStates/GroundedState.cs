using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : iState
{

    private PlayerData c_playerData;
    private VelocityCartridge cart_velocity;
    private AngleCalculationCartridge cart_angleCalc;

    public GroundedState(ref PlayerData playerData, ref VelocityCartridge vel, ref AngleCalculationCartridge angleCalc)
    {
        this.c_playerData = playerData;
        this.cart_velocity = vel;
        this.cart_angleCalc = angleCalc;
    }
    public void Act()
    {

    }

    /* BUG:
     * In this transition, since the player's direction of travel is backwards from the way they were 
     * traveling if going uphill to uphill, this leads to a rotational mismatch
     * 
     * Refactoring air travel will help a lot in this regard
     * 
     */ 
    public void TransitionAct()
    {
        Vector3 currentPosition = c_playerData.v_currentPosition;
        Vector3 currentDir = c_playerData.v_currentDirection;
        Vector3 currentNormal = c_playerData.v_currentNormal;
        Vector3 currentSurfaceNormal = c_playerData.v_currentSurfaceNormal;
        Quaternion currentRotation = c_playerData.q_currentRotation;

        c_playerData.f_currentJumpCharge = Constants.ZERO_F;
        c_playerData.f_currentAirVelocity = Constants.ZERO_F;
        cart_angleCalc.AlignOrientationWithSurface(ref currentNormal, ref currentDir, ref currentRotation, currentSurfaceNormal);
        c_playerData.v_currentPosition = currentPosition;
        c_playerData.v_currentDirection = currentDir;
        c_playerData.q_currentRotation = currentRotation;
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
