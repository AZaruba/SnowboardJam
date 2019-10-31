﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialState : iPlayerState {

    private GravityCartridge cart_gravity;
    private VelocityCartridge cart_velocity;

    public AerialState(ref GravityCartridge cart_grav, ref VelocityCartridge cart_vel)
    {
        cart_gravity = cart_grav;
        cart_velocity = cart_vel;
    }

    // TODO: add horizontal movement that takes minimal external input here
    public void Act(ref PlayerData c_playerData)
    {
        float airVelocity = c_playerData.CurrentAirVelocity;
        float gravity = c_playerData.Gravity;
        float terminalVelocity = c_playerData.f_terminalVelocity;
        float currentSpeed = c_playerData.CurrentSpeed;

        Vector3 currentDir = c_playerData.CurrentDirection;
        Vector3 position = c_playerData.CurrentPosition;

        cart_gravity.UpdateAirVelocity(ref airVelocity, ref gravity, ref terminalVelocity);
        cart_velocity.UpdatePosition(ref position, ref currentDir, ref currentSpeed);
        position.y += airVelocity;

        c_playerData.CurrentPosition = position;
        c_playerData.CurrentAirVelocity = airVelocity;
    }

    public void TransitionAct(ref PlayerData c_playerData)
    {
        Vector3 previousDirection = c_playerData.CurrentDirection;
        float currentVelocity = c_playerData.CurrentSpeed;
        float airVelocity = previousDirection.y;

        previousDirection.y = 0.0f; // "flatten direction"

        // scale velocity by the change in magnitude so we don't go faster in a direction
        float magnitudeFactor = previousDirection.magnitude / c_playerData.CurrentDirection.magnitude;
        airVelocity *= (1 - magnitudeFactor) * currentVelocity;

        c_playerData.CurrentAirVelocity = airVelocity;
        c_playerData.CurrentDirection = previousDirection;
        c_playerData.CurrentSpeed *= magnitudeFactor;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.LAND)
        {
            return StateRef.RIDING;
        }
        return StateRef.AIRBORNE;
    }


}
