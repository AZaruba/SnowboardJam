using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoastingState : iState {

    private AccelerationCartridge cart_acceleration;
    private VelocityCartridge cart_velocity;

    public CoastingState(ref AccelerationCartridge cart_a, ref VelocityCartridge cart_v)
    {
        this.cart_acceleration = cart_a;
        this.cart_velocity = cart_v;
    }

    public void Act(ref PlayerData c_playerData)
    {
        c_playerData.SetCurrentSpeed(Decelerate(ref c_playerData));
    }

    /// <summary>
    /// Returns the state after the given command.
    /// </summary>
    /// <returns>An iState following a given Command, or this if none.</returns>
    /// <param name="cmd">The command</param>
    public StateRef GetNextState(Command cmd)
    {
        // TODO: Replace this with states held by the state machine to prevent constructor calls
        if (cmd == Command.ACCELERATE)
        {
            return StateRef.ACCELERATING;
        }
        else if (cmd == Command.STOP)
        {
            return StateRef.STATIONARY;
        }
        return StateRef.COASTING;
    }

    /// <summary>
    /// Performs deceleration as needed (if object moving)
    /// </summary>
    private float Decelerate(ref PlayerData c_playerData)
    {
        float f_currentSpeed = c_playerData.GetCurrentSpeed();
        float f_deceleration = c_playerData.GetAcceleration();

        Vector3 v_currentPosition = c_playerData.GetCurrentPosition();
        Vector3 fwd = Vector3.forward;

        cart_acceleration.Decelerate(ref f_currentSpeed, ref f_deceleration);
        cart_velocity.UpdatePosition(ref v_currentPosition, ref fwd, ref f_currentSpeed);

        c_playerData.SetCurrentPosition(v_currentPosition);

        return f_currentSpeed;
    }
}
