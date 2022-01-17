using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialState : iState {

    private CollisionData c_collisionData;
    private PlayerData c_playerData;
    private AerialMoveData c_aerialMoveData;
    private PlayerPositionData c_positionData;

    public AerialState(ref PlayerData playerData, ref CollisionData collisionData,
        ref AerialMoveData moveData, ref PlayerPositionData positionData)
    {
        this.c_playerData = playerData;
        this.c_aerialMoveData = moveData;
        this.c_collisionData = collisionData;
        this.c_positionData = positionData;
    }

    public void Act()
    {
        float vertVelocity = c_aerialMoveData.f_verticalVelocity;
        float latVelocity = c_aerialMoveData.f_lateralVelocity;

        float gravity = c_playerData.f_gravity;
        float terminalVelocity = c_playerData.f_terminalVelocity;

        Vector3 lateralDir = c_aerialMoveData.v_lateralDirection;
        Vector3 playerPos = c_playerData.v_currentPosition;

        GravityCartridge.UpdateAirVelocity(ref vertVelocity, gravity, terminalVelocity);
        VelocityCartridge.UpdateAerialPosition(ref playerPos, lateralDir, vertVelocity, latVelocity);

        if (vertVelocity <= Constants.ZERO_F)
        {
            c_playerData.f_currentRaycastDistance = c_playerData.f_raycastDistance + Mathf.Abs(vertVelocity) * Time.fixedDeltaTime;
        }
        else
        {
            c_playerData.f_currentRaycastDistance = Constants.ZERO_F;
        }

        c_playerData.v_currentPosition = playerPos;
        c_aerialMoveData.f_verticalVelocity = vertVelocity;
    }

    public void TransitionAct()
    {
        Vector3 directionVector = c_playerData.q_currentRotation * Vector3.forward;
        Vector3 rotationVector = c_playerData.q_currentRotation * Vector3.forward;
        float verticalVelocity = Vector3.Dot(c_playerData.q_currentRotation * Vector3.forward, Vector3.up) * c_playerData.f_currentSpeed;

        // remove y and normalize to get to horizontal direction
        directionVector.y = Constants.ZERO_F;
        float horizontalVelocity = Vector3.Dot(c_playerData.q_currentRotation * Vector3.forward, directionVector.normalized) * c_playerData.f_currentSpeed;

        c_aerialMoveData.f_verticalVelocity = verticalVelocity + c_playerData.f_currentJumpCharge;
        c_aerialMoveData.f_lateralVelocity = horizontalVelocity;
        c_aerialMoveData.v_lateralDirection = directionVector.normalized;

        c_playerData.v_currentNormal = Vector3.up;
        c_playerData.v_currentDown = Vector3.down;
        rotationVector.y = Constants.ZERO_F;
        c_playerData.q_currentRotation = Quaternion.LookRotation(rotationVector.normalized, Vector3.up);
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.LAND)
        {
            Vector3 horizontalDir = c_aerialMoveData.v_lateralDirection * c_aerialMoveData.f_lateralVelocity;
            horizontalDir.y = c_aerialMoveData.f_verticalVelocity;

            Vector3 projectedDir = Vector3.ProjectOnPlane(horizontalDir, c_collisionData.v_surfaceNormal);
            c_playerData.f_currentSpeed = projectedDir.magnitude;
            c_aerialMoveData.f_verticalVelocity = c_playerData.f_gravity * -1;

            return StateRef.GROUNDED;
        }
        if (cmd == Command.CRASH)
        {
            return StateRef.DISABLED;
        }
        return StateRef.AIRBORNE;
    }


}
