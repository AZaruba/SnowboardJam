using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratingState : iState {

    private AccelerationCartridge cart_acceleration;
    private VelocityCartridge cart_velocity;

    public AcceleratingState(ref AccelerationCartridge cart_a, ref VelocityCartridge cart_v)
    {
        this.cart_acceleration = cart_a;
        this.cart_velocity = cart_v;
    }

    /// <summary>
    /// Retrieves player's current speed and acceleration, then calls
    /// the Acceleration cart to change the speed. Then puts the speed
    /// back into the playerdata.
    /// </summary>
    public void Act(ref PlayerData c_playerData)
    {
        float f_currentSpeed = c_playerData.GetCurrentSpeed();
        float f_acceleration = c_playerData.GetAcceleration();

        Vector3 v_currentPosition = c_playerData.GetCurrentPosition();
        Vector3 fwd = Vector3.forward;

        cart_acceleration.Accelerate(ref f_currentSpeed, ref f_acceleration);
        cart_velocity.UpdatePosition(ref v_currentPosition, ref fwd, ref f_currentSpeed);

        c_playerData.SetCurrentPosition(v_currentPosition);
        c_playerData.SetCurrentSpeed(f_currentSpeed);
    }

    /// <summary>
    /// Returns the state after the given command.
    /// </summary>
    /// <returns>An iState following a given Command, or this if none.</returns>
    /// <param name="cmd">The command</param>
    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.COAST)
        {
            return StateRef.COASTING;
        }
        return StateRef.ACCELERATING;
    }
}
