using System.Collections;
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

    // TODO: add horizontal movement that takes minimal external input here
    public void ActOld()
    {
        float airVelocity = c_playerData.f_currentAirVelocity;
        float gravity = c_playerData.f_gravity;
        float terminalVelocity = c_playerData.f_terminalVelocity;
        float currentSpeed = c_playerData.f_currentSpeed;

        Vector3 currentDir = c_playerData.v_currentDirection;
        Vector3 position = c_playerData.v_currentPosition;
        Vector3 oldPosition = position;

        cart_gravity.UpdateAirVelocity(ref airVelocity, gravity, terminalVelocity);
        cart_velocity.UpdatePosition(ref position, ref currentDir, ref currentSpeed);
        position.y += airVelocity * Time.deltaTime;

        c_playerData.v_currentPosition = position;
        c_playerData.f_currentAirVelocity = airVelocity;
        c_playerData.v_currentAirDirection = Vector3.Normalize(position - oldPosition);
        c_playerData.v_currentDown = c_playerData.v_currentAirDirection;
        if (airVelocity <= Constants.ZERO_F)
        {
            c_playerData.f_currentRaycastDistance = c_playerData.f_raycastDistance + Mathf.Abs(airVelocity) * Time.deltaTime;
        }
        else
        {
            c_playerData.f_currentRaycastDistance = Constants.ZERO_F;
        }
    }
    
    // TODO: Fix the lateral movement preservation (if we're flat, why did it get reduced once we fall?)
    public void TransitionAct()
    {

        Vector3 latDir = c_playerData.v_currentDirection.normalized;
        float groundVel = c_playerData.f_currentSpeed;

        float vertVel = (latDir.y/latDir.magnitude) * groundVel;
        float latVel = (Mathf.Abs(latDir.x + latDir.z)/latDir.magnitude) * groundVel;

        vertVel += c_playerData.f_currentJumpCharge; // * latDir.y?

        c_aerialMoveData.f_verticalVelocity = vertVel;
        c_aerialMoveData.f_lateralVelocity = latVel;

        // the lateral direction should be flattened
        latDir.y = 0.0f;
        latDir.Normalize();
        c_aerialMoveData.v_lateralDirection = latDir;
    }

    public void TransitionActOld()
    {
        Vector3 previousDirection = c_playerData.v_currentDirection;
        float currentVelocity = c_playerData.f_currentSpeed;
        float airVelocity = previousDirection.y * currentVelocity;
        Vector3 airDirection = previousDirection;
        airDirection.Normalize();

        previousDirection.y = Constants.ZERO_F; // "flatten direction"

        // scale velocity by the change in magnitude so we don't go faster in a direction
        // float magnitudeFactor = previousDirection.magnitude / c_playerData.v_currentDirection.magnitude;

        c_playerData.f_currentAirVelocity = airVelocity;
        c_playerData.v_currentDirection = previousDirection.normalized;
        c_playerData.v_currentAirDirection = airDirection;
        c_playerData.f_currentSpeed *= previousDirection.magnitude;
        c_playerData.v_currentDown = Vector3.down;
        c_playerData.f_currentJumpCharge = Constants.ZERO_F;
    }

    // TODO: Fix behavior when we he the ground, we're back to just bumping back up
    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.LAND)
        {
            return StateRef.GROUNDED;
        }
        if (cmd == Command.CRASH)
        {
            return StateRef.DISABLED;
        }
        return StateRef.AIRBORNE;
    }


}
