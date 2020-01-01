using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : iState
{

    private GravityCartridge cart_gravity;
    private VelocityCartridge cart_velocity;
    private PlayerData c_playerData;

    public JumpingState(ref PlayerData playerData, ref GravityCartridge cart_grav, ref VelocityCartridge cart_vel)
    {
        this.c_playerData = playerData;
        this.cart_gravity = cart_grav;
        this.cart_velocity = cart_vel;
    }

    // TODO: add horizontal movement that takes minimal external input here
    public void Act()
    {
        float airVelocity = c_playerData.f_currentAirVelocity;
        float gravity = c_playerData.f_gravity;
        float terminalVelocity = c_playerData.f_terminalVelocity;
        float currentSpeed = c_playerData.f_currentSpeed;

        Vector3 currentDir = c_playerData.v_currentDirection;
        Vector3 position = c_playerData.v_currentPosition;

        cart_gravity.UpdateAirVelocity(ref airVelocity, ref gravity, ref terminalVelocity);
        cart_velocity.UpdatePosition(ref position, ref currentDir, ref currentSpeed);
        position.y += airVelocity * Time.deltaTime;

        c_playerData.v_currentPosition = position;
        c_playerData.f_currentAirVelocity = airVelocity;
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
        Vector3 previousDirection = c_playerData.v_currentDirection;
        float currentVelocity = c_playerData.f_currentSpeed;
        float airVelocity = previousDirection.normalized.y * currentVelocity;
        float jumpCharge = c_playerData.f_currentJumpCharge;

        previousDirection.y = Constants.ZERO_F; // "flatten direction"

        // scale velocity by the change in magnitude so we don't go faster in a direction
        float magnitudeFactor = previousDirection.magnitude / c_playerData.v_currentDirection.magnitude;

        c_playerData.f_currentAirVelocity = airVelocity + jumpCharge;
        c_playerData.v_currentDirection = previousDirection;
        c_playerData.f_currentSpeed *= magnitudeFactor;
        c_playerData.v_currentDown = Vector3.down;
        c_playerData.f_currentJumpCharge = Constants.ZERO_F;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.LAND)
        {
            return StateRef.GROUNDED;
        }
        return StateRef.AIRBORNE;
    }


}
