using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryState : iState {

    private AccelerationCartridge cart_acceleration;
    private PlayerData c_playerData;

    public StationaryState(PlayerData dataIn)
    {
        ProvideData(dataIn);
        cart_acceleration = new AccelerationCartridge ();
    }

    public void ProvideData(PlayerData dataIn)
    {
        c_playerData = dataIn;
    }

    public void Act()
    {
        c_playerData.SetCurrentSpeed(Decelerate());
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
            return new AcceleratingState (c_playerData);
        }
        return this;
    }

    /// <summary>
    /// Performs deceleration as needed (if object moving)
    /// </summary>
    private float Decelerate()
    {
        float f_currentSpeed = c_playerData.GetCurrentSpeed();
        float f_deceleration = c_playerData.GetAcceleration();

        cart_acceleration.Decelerate(ref f_currentSpeed, ref f_deceleration);

        return f_currentSpeed;
    }
}
