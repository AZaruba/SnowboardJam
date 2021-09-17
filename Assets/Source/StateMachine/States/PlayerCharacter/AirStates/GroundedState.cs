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
        Vector3 currentNormal = c_playerData.v_currentNormal;
        Quaternion currentRotation = c_playerData.q_currentRotation;
        Quaternion currentModelRotation = c_positionData.q_currentModelRotation;

        c_playerData.v_currentPosition -= c_collisionData.v_attachPoint;

        AngleCalculationCartridge.AlignToSurfaceByTail(c_collisionData.v_surfaceNormal,
                                            ref currentRotation,
                                            ref currentNormal);
        AngleCalculationCartridge.AlignToSurfaceByTail(c_collisionData.v_surfaceNormal,
                                            ref currentModelRotation,
                                            ref currentNormal);


        c_playerData.v_currentNormal = currentNormal;
        c_playerData.q_currentRotation = currentRotation;
        c_positionData.q_currentModelRotation = currentModelRotation;

        c_aerialMoveData.f_verticalVelocity = c_playerData.f_gravity * -1;
    }

    public void TransitionAct()
    {
        c_playerData.f_currentJumpCharge = Constants.ZERO_F;
        c_playerData.f_currentAirVelocity = Constants.ZERO_F;

        c_playerData.v_currentPosition -= c_collisionData.v_attachPoint;

        /*
        Vector3 currentNormal = c_playerData.v_currentNormal;
        Quaternion currentRotation = c_playerData.q_currentRotation;
        Quaternion currentModelRotation = c_positionData.q_currentModelRotation;

        //c_playerData.v_currentPosition += c_collisionData.v_attachPoint;

        AngleCalculationCartridge.AlignToSurfaceByTail(c_collisionData.v_surfaceNormal,
                                            ref currentRotation,
                                            ref currentNormal);
        AngleCalculationCartridge.AlignToSurfaceByTail(c_collisionData.v_surfaceNormal,
                                            ref currentModelRotation,
                                            ref currentNormal);


        c_playerData.v_currentNormal = currentNormal;
        c_playerData.q_currentRotation = currentRotation;
        c_positionData.q_currentModelRotation = currentModelRotation;
        */

        c_playerData.f_currentRaycastDistance = c_playerData.f_raycastDistance;
        c_playerData.v_currentDown = c_collisionData.v_surfaceNormal * -1;

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
