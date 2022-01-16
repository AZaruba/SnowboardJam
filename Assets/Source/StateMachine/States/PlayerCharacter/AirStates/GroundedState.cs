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
        Vector3 currentNormal = c_collisionData.v_surfaceNormal;
        Quaternion currentRotation = c_playerData.q_currentRotation;
        Quaternion currentModelRotation = c_positionData.q_currentModelRotation;

        AngleCalculationCartridge.AlignToSurfaceByTail(c_collisionData.v_surfaceNormal,
                                            ref currentRotation,
                                            ref currentNormal,
                                            1);
        AngleCalculationCartridge.AlignToSurfaceByTail(c_collisionData.v_surfaceNormal,
                                            ref currentModelRotation,
                                            ref currentNormal,
                                            1);

        c_playerData.v_currentNormal = currentNormal;
        c_playerData.q_currentRotation = currentRotation;
        c_positionData.q_currentModelRotation = currentModelRotation;

        c_aerialMoveData.f_verticalVelocity = Vector3.Dot(c_playerData.q_currentRotation * Vector3.forward, Vector3.up) * c_playerData.f_currentSpeed;
    }

    public void TransitionAct()
    {
        c_playerData.f_currentJumpCharge = Constants.ZERO_F;
        c_playerData.f_currentAirVelocity = Constants.ZERO_F;

        c_playerData.f_currentRaycastDistance = c_playerData.f_raycastDistance;
        c_playerData.v_currentDown = c_collisionData.v_surfaceNormal * -1;

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.CHARGE)
        {
            // move this to leaving state so forced transition doesn't reset jump power
            c_playerData.f_currentJumpCharge = c_playerData.f_baseJumpPower;
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
