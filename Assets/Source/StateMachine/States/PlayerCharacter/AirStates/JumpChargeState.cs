using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpChargeState : iState
{
    private PlayerData c_playerData;
    private CollisionData c_collisionData;
    private IncrementCartridge cart_increment;
    
    public JumpChargeState(ref PlayerData playerData, ref CollisionData collisionData, ref IncrementCartridge incr)
    {
        this.c_playerData = playerData;
        this.c_collisionData = collisionData;
        this.cart_increment = incr;
    }

    public void Act()
    {
        float chargeCap = c_playerData.f_jumpPower;
        float chargeValue = c_playerData.f_currentJumpCharge;
        float chargeDelta = c_playerData.f_jumpChargeRate;

        cart_increment.Increment(ref chargeValue, chargeDelta * Time.deltaTime, chargeCap);

        c_playerData.f_currentJumpCharge = chargeValue;

        Vector3 currentPosition = c_playerData.v_currentPosition;
        Vector3 currentDir = c_playerData.v_currentDirection;
        Vector3 currentNormal = c_playerData.v_currentNormal;
        Quaternion currentRotation = c_playerData.q_currentRotation;

        float currentTopSpeed = c_playerData.f_currentTopSpeed;
        float currentAcceleration = c_playerData.f_currentAcceleration;

        AngleCalculationCartridge.AlignToSurfaceByTail(ref currentPosition,
                                            c_collisionData.v_surfaceNormal,
                                            ref currentRotation,
                                            ref currentDir,
                                            ref currentNormal);

        SurfaceInfluenceCartridge.AdjustAcceleration(ref currentAcceleration,
                                                     c_playerData.f_acceleration,
                                                     c_playerData.f_gravity,
                                                     c_playerData.f_topSpeed / currentTopSpeed,
                                                     currentRotation);

        SurfaceInfluenceCartridge.AdjustTopSpeed(ref currentTopSpeed, c_playerData.f_topSpeed, currentRotation);

        c_playerData.v_currentPosition = currentPosition;
        c_playerData.v_currentDirection = currentDir;
        c_playerData.v_currentNormal = currentNormal;
        c_playerData.q_currentRotation = currentRotation;

        c_playerData.f_currentAcceleration = currentAcceleration;
        c_playerData.f_currentTopSpeed = currentTopSpeed;
    }

    public void TransitionAct()
    {
        c_playerData.f_currentJumpCharge = c_playerData.f_baseJumpPower;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.FALL)
        {
            return StateRef.AIRBORNE;
        }
        if (cmd == Command.JUMP)
        {
            c_playerData.f_currentJumpCharge = 0.0f;

            // edit position to ensure we are already the first jump frame ABOVE the ground here
            Vector3 upwardTranslation = Vector3.up * 0.0f; // some value equal to the board size

            c_playerData.v_currentPosition += upwardTranslation;
            return StateRef.AIRBORNE;
        }
        if (cmd == Command.CRASH)
        {
            return StateRef.DISABLED;
        }
        return StateRef.CHARGING;
    }
}

