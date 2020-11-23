using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : iState
{

    private PlayerData c_playerData;
    private PlayerPositionData c_positionData;
    private CollisionData c_collisionData;
    private VelocityCartridge cart_velocity;
    private AngleCalculationCartridge cart_angleCalc;
    private SurfaceInfluenceCartridge cart_surfInf;

    public GroundedState(ref PlayerData playerData,
                         ref CollisionData collisionData,
                         ref PlayerPositionData positionData,
                         ref VelocityCartridge vel, 
                         ref AngleCalculationCartridge angleCalc,
                         ref SurfaceInfluenceCartridge surfInf)
    {
        this.c_playerData = playerData;
        this.c_positionData = positionData;
        this.c_collisionData = collisionData;
        this.cart_velocity = vel;
        this.cart_angleCalc = angleCalc;
        this.cart_surfInf = surfInf;
    }
    public void Act()
    {
        Vector3 currentPosition = c_playerData.v_currentPosition;
        Vector3 currentDir = c_playerData.v_currentDirection;
        Vector3 currentNormal = c_playerData.v_currentNormal;
        Quaternion currentRotation = c_playerData.q_currentRotation;

        SurfaceInfluenceCartridge.KeepAboveSurface2(ref currentPosition,
                                                    c_collisionData.v_surfaceNormal,
                                                    c_collisionData.v_frontOffset,
                                                    c_collisionData.v_backOffset,
                                                    c_collisionData.v_frontPoint,
                                                    c_collisionData.v_backPoint,
                                                    Quaternion.Inverse(currentRotation));

        AngleCalculationCartridge.AlignToSurfaceByTail(ref currentPosition,
                                            c_collisionData.v_backOffset,
                                            c_collisionData.v_backNormal,
                                            c_collisionData.v_frontOffset,
                                            c_collisionData.v_frontPoint,
                                            c_collisionData.v_frontNormal,
                                            ref currentRotation,
                                            ref currentDir,
                                            ref currentNormal);

        c_playerData.v_currentPosition = currentPosition;
        c_playerData.v_currentDirection = currentDir;
        c_playerData.v_currentNormal = currentNormal;
        c_playerData.q_currentRotation = currentRotation;
    }

    public void TransitionAct()
    {
        bool isReversed = c_positionData.b_modelReversed;
        Vector3 currentPosition = c_playerData.v_currentPosition;
        Vector3 currentDir = c_playerData.v_currentDirection;
        Vector3 currentNormal = c_playerData.v_currentNormal;
        Vector3 currentSurfaceNormal = c_collisionData.v_surfaceNormal;
        Quaternion currentRotation = c_playerData.q_currentRotation;
        Quaternion currentModelRotation = c_positionData.q_currentModelRotation;

        c_playerData.f_currentJumpCharge = Constants.ZERO_F;
        c_playerData.f_currentAirVelocity = Constants.ZERO_F;
        //cart_angleCalc.AlignOrientationWithSurface(ref currentNormal, ref currentDir, ref currentRotation, currentSurfaceNormal);
        cart_surfInf.SwitchReverse(ref isReversed, currentRotation, currentModelRotation);
        c_playerData.v_currentPosition = currentPosition;
        c_playerData.v_currentDirection = currentDir;
        c_playerData.v_currentNormal = currentNormal;
        c_playerData.q_currentRotation = currentRotation;
        c_positionData.b_modelReversed = isReversed;

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
