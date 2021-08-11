using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : iState
{

    private PlayerData c_playerData;
    private AerialMoveData c_aerialMoveData;
    private PlayerPositionData c_positionData;
    private CollisionData c_collisionData;

    public GroundedState(ref PlayerData playerData,
                         ref AerialMoveData aerialMoveData,
                         ref CollisionData collisionData,
                         ref PlayerPositionData positionData)
    {
        this.c_playerData = playerData;
        this.c_aerialMoveData = aerialMoveData;
        this.c_positionData = positionData;
        this.c_collisionData = collisionData;
    }
    public void Act()
    {
        Vector3 currentPosition = c_playerData.v_currentPosition;
        Vector3 currentNormal = c_playerData.v_currentNormal;
        Quaternion currentRotation = c_playerData.q_currentRotation;

        float currentTopSpeed = c_playerData.f_currentTopSpeed;
        float currentAcceleration = c_playerData.f_currentAcceleration;

        AngleCalculationCartridge.AlignToSurfaceByTail(c_collisionData.v_surfaceNormal,
                                            ref currentRotation,
                                            ref currentNormal);

        SurfaceInfluenceCartridge.AdjustAcceleration(ref currentAcceleration,
                                                     c_playerData.f_acceleration,
                                                     c_playerData.f_gravity,
                                                     c_playerData.f_topSpeed / currentTopSpeed,
                                                     currentRotation,
                                                     c_positionData.i_switchStance);

        SurfaceInfluenceCartridge.AdjustTopSpeed(ref currentTopSpeed, c_playerData.f_topSpeed, c_playerData.f_terminalVelocity, currentRotation, c_positionData.i_switchStance);

        c_playerData.v_currentPosition = currentPosition;
        c_playerData.v_currentNormal = currentNormal;
        c_playerData.q_currentRotation = currentRotation;
        c_playerData.f_currentAcceleration = currentAcceleration;
        c_playerData.f_currentTopSpeed = currentTopSpeed;

        c_aerialMoveData.f_verticalVelocity = c_playerData.f_gravity * -1;
    }

    public void TransitionAct()
    {
        Vector3 currentPosition = c_playerData.v_currentPosition;
        Vector3 currentNormal = c_playerData.v_currentNormal;
        Quaternion currentRotation = c_playerData.q_currentRotation;

        c_playerData.f_currentJumpCharge = Constants.ZERO_F;
        c_playerData.f_currentAirVelocity = Constants.ZERO_F;

        c_playerData.v_currentPosition = currentPosition;
        c_playerData.v_currentNormal = currentNormal;
        c_playerData.q_currentRotation = currentRotation;

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
