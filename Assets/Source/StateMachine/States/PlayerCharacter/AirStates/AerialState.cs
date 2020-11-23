using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialState : iState {

    private GravityCartridge cart_gravity;
    private VelocityCartridge cart_velocity;
    private CollisionData c_collisionData;
    private PlayerData c_playerData;
    private AerialMoveData c_aerialMoveData;

    public AerialState(ref PlayerData playerData, ref CollisionData collisionData,
        ref AerialMoveData moveData, ref GravityCartridge cart_grav, ref VelocityCartridge cart_vel)
    {
        this.c_playerData = playerData;
        this.c_aerialMoveData = moveData;
        this.c_collisionData = collisionData;
        this.cart_gravity = cart_grav;
        this.cart_velocity = cart_vel;
    }

    public void Act()
    {
        float vertVelocity = c_aerialMoveData.f_verticalVelocity;
        float latVelocity = c_aerialMoveData.f_lateralVelocity;

        float gravity = c_playerData.f_gravity;
        float terminalVelocity = c_playerData.f_terminalVelocity;

        Vector3 lateralDir = c_aerialMoveData.v_lateralDirection;
        Vector3 playerPos = c_playerData.v_currentPosition;

        cart_gravity.UpdateAirVelocity(ref vertVelocity, gravity, terminalVelocity);
        cart_velocity.UpdateAerialPosition(ref playerPos, lateralDir, vertVelocity, latVelocity);

        if (vertVelocity <= Constants.ZERO_F)
        {
            c_playerData.f_currentRaycastDistance = c_playerData.f_raycastDistance + Mathf.Abs(vertVelocity) * Time.deltaTime;
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
        Vector3 jumpVector = Vector3.up * c_playerData.f_currentJumpCharge;
        Vector3 directVector = c_playerData.v_currentDirection * c_playerData.f_currentSpeed;

        Vector3 totalAerialVector = jumpVector + directVector;
        Vector3 latDir = totalAerialVector;
        float vertVel = totalAerialVector.y;

        // the lateral direction should be flattened
        latDir.y = 0.0f;
        float latVel = latDir.magnitude;
        latDir.Normalize();

        c_aerialMoveData.v_lateralDirection = latDir;
        c_aerialMoveData.f_verticalVelocity = vertVel;
        c_aerialMoveData.f_lateralVelocity = latVel;
        c_playerData.v_currentDown = Vector3.down;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.LAND)
        {
            Vector3 horizontalDir = c_aerialMoveData.v_lateralDirection * c_aerialMoveData.f_lateralVelocity;
            horizontalDir.y = c_aerialMoveData.f_verticalVelocity;

            Vector3 projectedDir = Vector3.ProjectOnPlane(horizontalDir, c_collisionData.v_surfaceNormal);
            c_playerData.f_currentSpeed = projectedDir.magnitude;
            c_aerialMoveData.f_verticalVelocity = c_playerData.f_gravity * -2;
            c_playerData.v_currentDirection = projectedDir.normalized;
            return StateRef.GROUNDED;
        }
        if (cmd == Command.CRASH)
        {
            return StateRef.DISABLED;
        }
        return StateRef.AIRBORNE;
    }


}
