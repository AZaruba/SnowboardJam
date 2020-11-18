using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingState : iState
{
    AccelerationCartridge cart_acceleration;
    VelocityCartridge cart_velocity;
    AngleCalculationCartridge cart_angleCalc;
    SurfaceInfluenceCartridge cart_surfInf;

    PlayerData c_playerData;
    PlayerInputData c_playerInputData;
    PlayerPositionData c_playerPositionData;
    CollisionData c_collisionData;

    public SlowingState(ref PlayerData playerData,
                        ref CollisionData collisionData,
                        ref PlayerInputData inputData,
                        ref PlayerPositionData positionData, 
                        ref VelocityCartridge vel,
                        ref AccelerationCartridge accel, 
                        ref AngleCalculationCartridge angleCalc,
                        ref SurfaceInfluenceCartridge surfInf)
    {
        this.c_playerData = playerData;
        this.c_playerInputData = inputData;
        this.c_playerPositionData = positionData;
        this.c_collisionData = collisionData;
        this.cart_velocity = vel;
        this.cart_acceleration = accel;
        this.cart_angleCalc = angleCalc;
        this.cart_surfInf = surfInf;
    }
    public void Act()
    {
        // check for angle when implemented
        float currentVelocity = c_playerData.f_currentSpeed;
        float topSpeed = c_playerData.f_topSpeed;
        float deceleration = c_playerData.f_brakePower;
        float slowScaling = c_playerInputData.f_inputAxisLVert * - 1;
        Vector3 currentPosition = c_playerData.v_currentPosition;
        Vector3 currentDir = c_playerData.v_currentDirection;
        Vector3 currentNormal = c_playerData.v_currentNormal;
        Vector3 currentSurfaceNormal = c_collisionData.v_surfaceNormal;
        Quaternion currentRotation = c_playerData.q_currentRotation;

        cart_acceleration.Decelerate(ref currentVelocity, deceleration * slowScaling);
        cart_surfInf.PullDirectionVector(ref currentDir, currentSurfaceNormal, Vector3.up, 0.0f, ref currentVelocity, deceleration * slowScaling);
        // cart_acceleration.CapSpeed(ref currentVelocity, topSpeed);

        cart_angleCalc.AlignToSurfaceByTail(ref currentPosition,
                                            c_collisionData.v_backOffset,
                                            c_collisionData.v_frontOffset,
                                            c_collisionData.v_frontPoint,
                                            c_collisionData.v_frontNormal,
                                            ref currentRotation,
                                            ref currentDir,
                                            ref currentNormal);

        cart_velocity.UpdatePositionTwo(ref currentPosition, ref currentRotation, ref currentVelocity);

        c_playerData.f_currentSpeed = currentVelocity;
        c_playerData.v_currentPosition = currentPosition;
        c_playerData.v_currentNormal = currentNormal.normalized;
        c_playerData.v_currentDown = currentNormal.normalized * -1;
        c_playerData.v_currentDirection = currentDir.normalized;
        c_playerData.q_currentRotation = currentRotation;
    }

    public void TransitionAct()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.FALL)
        {
            return StateRef.AIRBORNE;
        }
        if (cmd == Command.RIDE)
        {
            return StateRef.RIDING;
        }
        if (cmd == Command.CHARGE)
        {
            return StateRef.CHARGING;
        }
        if (cmd == Command.STOP)
        {
            return StateRef.STATIONARY;
        }
        if (cmd == Command.CRASH)
        {
            return StateRef.CRASHED;
        }
        return StateRef.STOPPING;
    }
}
