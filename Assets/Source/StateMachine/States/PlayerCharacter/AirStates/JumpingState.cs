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
        /* Rationale:
         * We want to have the jump up vector be rotated AROUND the forward axis. The reason for this
         * is the player should still be able to jump straight up for game feel, but when traveling with any
         * amount of roll (i.e. along a platform rotated 45 degrees in the forward direction), the jump
         * should be along the roll axis.
         *
         * How do we solve the "quarter pipe" conundrum?
         * - is it just the angle between "up" and the current normal?
         * - ideally it would be the "roll," which would be the angle between global
         *   up and the current up around the forward axis.
         *
         * Implementation:
         * - Project the two vectors (global up and current normal) onto the
         *   plane formed by the current direction, then take the angle between
         *      - CHECK: special case for the current direction as up (making global up zero)
         *
         */
        Vector3 previousDirection = c_playerData.v_currentDirection;
        Vector3 currentNormal = c_playerData.v_currentNormal;
        float currentAirVelocity = Constants.ZERO_F;
        float jumpCharge = c_playerData.f_currentJumpCharge;
        float currentVelocity = c_playerData.f_currentSpeed;

        Vector3 nextDirection = previousDirection;
        Vector3 airDirection = Vector3.zero;
        Vector3 airNormal;

        /* if we're going upward, use global up. If we're going downward, use local up
         */
        if (previousDirection.y < 0.0f)
        {
            airNormal = currentNormal;
        }
        else
        {
            airNormal = Vector3.up;
        }

        /* Making this work:
         *  1) The jump charge value should be the y component of the vector + the jumpcharge
         *  2) The jump direction should be rolled to be the right angle AROUND the forward
         *  3) The aerial velocity is no big deal, the direction should adapt correctly
         */
        currentAirVelocity = jumpCharge;


        c_playerData.v_currentDirection = nextDirection;
        c_playerData.v_currentAirDirection = airDirection;
        c_playerData.v_currentDown = Vector3.down;
        c_playerData.f_currentJumpCharge = Constants.ZERO_F;
        c_playerData.f_currentSpeed = currentVelocity;
        c_playerData.f_currentAirVelocity = currentAirVelocity;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.LAND)
        {
            if (Vector3.Distance(c_playerData.v_currentAirDirection.normalized * -1, c_playerData.v_currentSurfaceNormal) > 0.05f)
            {
                c_playerData.v_currentDirection = c_playerData.v_currentAirDirection.normalized;
            }
            c_playerData.f_currentSpeed += c_playerData.f_currentAirVelocity;
            return StateRef.GROUNDED;
        }
        return StateRef.JUMPING;
    }


}
