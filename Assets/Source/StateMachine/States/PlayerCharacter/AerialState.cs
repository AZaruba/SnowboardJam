using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialState : iState {

    private GravityCartridge cart_gravity;
    private VelocityCartridge cart_velocity;
    private PlayerData c_playerData;

    public AerialState(ref PlayerData playerData, ref GravityCartridge cart_grav, ref VelocityCartridge cart_vel)
    {
        this.c_playerData = playerData;
        this.cart_gravity = cart_grav;
        this.cart_velocity = cart_vel;
    }

    // TODO: add horizontal movement that takes minimal external input here
    public void Act()
    {
        float airVelocity = c_playerData.CurrentAirVelocity;
        float gravity = c_playerData.Gravity;
        float terminalVelocity = c_playerData.f_terminalVelocity;
        float currentSpeed = c_playerData.CurrentSpeed;

        Vector3 currentDir = c_playerData.CurrentDirection;
        Vector3 position = c_playerData.CurrentPosition;

        cart_gravity.UpdateAirVelocity(ref airVelocity, ref gravity, ref terminalVelocity);
        cart_velocity.UpdatePosition(ref position, ref currentDir, ref currentSpeed);
        position.y += airVelocity * Time.deltaTime;

        c_playerData.CurrentPosition = position;
        c_playerData.CurrentAirVelocity = airVelocity;
        if (airVelocity < Constants.ZERO_F)
        {
            c_playerData.f_currentRaycastDistance = Mathf.Abs(airVelocity) * Time.deltaTime;
        }
        else
        {
            c_playerData.f_currentRaycastDistance = Constants.ZERO_F;
        }
    }

    public void TransitionAct()
    {
        Vector3 previousDirection = c_playerData.CurrentDirection;
        float currentVelocity = c_playerData.CurrentSpeed;
        float airVelocity = previousDirection.normalized.y * currentVelocity;

        previousDirection.y = Constants.ZERO_F; // "flatten direction"

        // scale velocity by the change in magnitude so we don't go faster in a direction
        float magnitudeFactor = previousDirection.magnitude / c_playerData.CurrentDirection.magnitude;

        c_playerData.CurrentAirVelocity = airVelocity;
        c_playerData.CurrentDirection = previousDirection;
        c_playerData.CurrentSpeed *= magnitudeFactor;
        c_playerData.CurrentDown = Vector3.down;
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
