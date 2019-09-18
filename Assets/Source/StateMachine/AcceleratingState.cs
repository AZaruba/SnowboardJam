using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratingState : iState {

    private AccelerationCartridge cart_acceleration;

    public AcceleratingState()
    {
        cart_acceleration = new AccelerationCartridge ();
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

        cart_acceleration.Accelerate(ref f_currentSpeed, ref f_acceleration);

        c_playerData.SetCurrentSpeed(f_currentSpeed);
    }

    /// <summary>
    /// Returns the state after the given command.
    /// </summary>
    /// <returns>An iState following a given Command, or this if none.</returns>
    /// <param name="cmd">The command</param>
    public iState GetNextState(Command cmd)
    {
        if (cmd == Command.COAST)
        {
            return new StationaryState ();
        }
        return this;
    }
}
