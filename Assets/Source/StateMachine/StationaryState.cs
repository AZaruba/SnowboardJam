using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryState : iState {

    private AccelerationCartridge cart_acceleration;

    public StationaryState()
    {
        cart_acceleration = new AccelerationCartridge ();
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
    public iState GetNextState(Command cmd)
    {
        // TODO: Replace this with states held by the state machine to prevent constructor calls
        if (cmd == Command.ACCELERATE)
        {
            return new AcceleratingState ();
        }
        return this;
    }

    /// <summary>
    /// Performs deceleration as needed (if object moving)
    /// </summary>
    private float Decelerate(ref PlayerData c_playerData)
    {
        float f_currentSpeed = c_playerData.GetCurrentSpeed();
        float f_deceleration = c_playerData.GetAcceleration();

        cart_acceleration.Decelerate(ref f_currentSpeed, ref f_deceleration);

        return f_currentSpeed;
    }
}
