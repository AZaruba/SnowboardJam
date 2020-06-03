﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialState : iState {

    private GravityCartridge cart_gravity;
    private VelocityCartridge cart_velocity;
    private PlayerData c_playerData;
    private AerialMoveData c_aerialMoveData;

    public AerialState(ref PlayerData playerData, ref AerialMoveData moveData, ref GravityCartridge cart_grav, ref VelocityCartridge cart_vel)
    {
        this.c_playerData = playerData;
        this.c_aerialMoveData = moveData;
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
        /* Fixing this up:
         *
         * 1) there should be a small hop in the normal direction
         * 2) any boost from jump charge should go straight up
         *
         */ 

        Vector3 latDir = c_playerData.v_currentDirection.normalized;
        float groundVel = c_playerData.f_currentSpeed;

        float vertVel = (latDir.y/latDir.magnitude) * groundVel;
        float latVel = (Mathf.Abs(latDir.x) + Mathf.Abs(latDir.z))/latDir.magnitude * groundVel;

        vertVel += c_playerData.f_currentJumpCharge; // * latDir.y?

        c_aerialMoveData.f_verticalVelocity = vertVel;
        c_aerialMoveData.f_lateralVelocity = latVel;

        // the lateral direction should be flattened
        latDir.y = 0.0f;
        latDir.Normalize();
        c_aerialMoveData.v_lateralDirection = latDir;
    }

    // TODO: Fix behavior when we he the ground, we're back to just bumping back up
    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.LAND)
        {
            Vector3 horizontalDir = c_aerialMoveData.v_lateralDirection * c_aerialMoveData.f_lateralVelocity;
            horizontalDir.y = c_aerialMoveData.f_verticalVelocity;

            // if these vectors are equal, then we are landing on a like-plane of the one we jumped off of
            Vector3 projectedDir = Vector3.ProjectOnPlane(horizontalDir, c_playerData.v_currentSurfaceNormal);
            if (horizontalDir.normalized != c_playerData.v_currentSurfaceNormal*-1)
            {
                c_playerData.v_currentDirection = projectedDir.normalized;
            }
            c_playerData.f_currentSpeed = projectedDir.magnitude;
            return StateRef.GROUNDED;
        }
        if (cmd == Command.CRASH)
        {
            return StateRef.DISABLED;
        }
        return StateRef.AIRBORNE;
    }


}
