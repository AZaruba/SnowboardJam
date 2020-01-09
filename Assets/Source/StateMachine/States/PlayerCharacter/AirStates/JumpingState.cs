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
        Vector3 currentAirDir = c_playerData.v_currentAirDirection;
        Vector3 position = c_playerData.v_currentPosition;
        Vector3 oldPosition = position;

        cart_gravity.UpdateAirVelocity(ref airVelocity, ref gravity, ref terminalVelocity);
        cart_velocity.UpdatePosition(ref position, ref currentDir, ref currentSpeed);
        position.y += airVelocity * Time.deltaTime;
        currentAirDir.y = airVelocity;

        c_playerData.v_currentPosition = position;
        c_playerData.f_currentAirVelocity = airVelocity;

        c_playerData.v_currentAirDirection = Vector3.Normalize(position - oldPosition);
        if (airVelocity < Constants.ZERO_F)
        {
            c_playerData.f_currentRaycastDistance = (Mathf.Abs(airVelocity) * Time.deltaTime) + c_playerData.f_raycastDistance;
        }
        else
        {
            c_playerData.f_currentRaycastDistance = Constants.ZERO_F;
        }
    }

    public void TransitionAct()
    {
        Vector3 previousDirection = c_playerData.v_currentDirection;
        Vector3 currentUp = c_playerData.v_currentNormal;
        float currentVelocity = c_playerData.f_currentSpeed;
        float jumpCharge = c_playerData.f_currentJumpCharge;
        float airVelocity;

        Vector3 currentVector = previousDirection * currentVelocity;

        // calculate jump vector
        currentUp.Normalize();
        currentUp *= jumpCharge;

        currentVector = currentVector + currentUp;
        currentVector.Normalize();
        Debug.Log(currentVector);
        airVelocity = currentVector.y + currentUp.y;

        previousDirection.y = Constants.ZERO_F; // "flatten direction"
        currentUp.y = Constants.ZERO_F;

        // scale velocity by the change in magnitude so we don't go faster in a direction
        float magnitudeFactor = previousDirection.magnitude / c_playerData.v_currentDirection.magnitude;

        Vector3 airDirection = previousDirection;
        airDirection.y = airVelocity;
        airDirection.Normalize();

        c_playerData.f_currentAirVelocity = airVelocity;
        c_playerData.v_currentDirection = currentVector;
        c_playerData.v_currentAirDirection = airDirection;
        c_playerData.f_currentSpeed *= magnitudeFactor;
        c_playerData.v_currentDown = Vector3.down;
        c_playerData.f_currentJumpCharge = Constants.ZERO_F;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.LAND)
        {
            if (Vector3.Distance(c_playerData.v_currentAirDirection.normalized * -1, c_playerData.v_currentSurfaceNormal) > 0.05f)
            {
                c_playerData.v_currentDirection = c_playerData.v_currentAirDirection;
            }
            c_playerData.f_currentSpeed += c_playerData.f_currentAirVelocity;
            return StateRef.GROUNDED;
        }
        return StateRef.JUMPING;
    }


}
