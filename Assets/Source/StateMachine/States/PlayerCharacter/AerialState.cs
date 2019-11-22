using System.Collections;
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
        Vector3 translation = c_playerData.CurrentTranslation;

        cart_gravity.UpdateAirVelocity(ref airVelocity, ref gravity, ref terminalVelocity);
        cart_velocity.UpdatePosition(ref translation, ref currentDir, ref currentSpeed);
        translation.y += airVelocity;

        c_playerData.CurrentTranslation = translation;
        c_playerData.CurrentAirVelocity = airVelocity;
        if (airVelocity < Constants.ZERO_F)
        {
            c_playerData.f_currentRaycastDistance = Mathf.Abs(airVelocity);
        }
        else
        {
            c_playerData.f_currentRaycastDistance = Constants.ZERO_F;
        }
    }

    public void TransitionAct(ref PlayerData c_playerData)
    {
        Debug.Log("AERIAL");
        Vector3 previousDirection = c_playerData.CurrentDirection;
        Vector3 currentTranslation = c_playerData.CurrentTranslation;
        float currentVelocity = c_playerData.CurrentSpeed;
        float airVelocity = previousDirection.y * currentVelocity;

        previousDirection.y = Constants.ZERO_F; // "flatten direction"
        currentTranslation.y = Constants.ZERO_F; // "flatten translation"

        // scale velocity by the change in magnitude so we don't go faster in a direction
        float magnitudeFactor = previousDirection.magnitude / c_playerData.CurrentDirection.magnitude;

        c_playerData.CurrentAirVelocity = airVelocity;
        c_playerData.CurrentDirection = previousDirection;
        c_playerData.CurrentTranslation = currentTranslation;
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
