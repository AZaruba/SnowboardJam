using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratingState : iState {

    private AccelerationCartridge cart_acceleration;
    private PlayerData c_playerData;

    public AcceleratingState(PlayerData dataIn, AccelerationCartridge cartIn)
    {
        ProvideData(dataIn);
        cart_acceleration = cartIn;
    }

    public void ProvideData(PlayerData dataIn)
    {
        c_playerData = dataIn;
    }

    /// <summary>
    /// Retrieves player's current speed and acceleration, then calls
    /// the Acceleration cart to change the speed. Then puts the speed
    /// back into the playerdata.
    /// </summary>
    public void Act()
    {
        float f_currentSpeed = c_playerData.GetCurrentSpeed();
        float f_acceleration = c_playerData.GetAcceleration();

        cart_acceleration.Accelerate(ref f_currentSpeed, ref f_acceleration);

        c_playerData.SetCurrentSpeed(f_currentSpeed);
    }

    public iState GetNextState(Command cmd)
    {
        return new AcceleratingState (c_playerData, cart_acceleration);
    }
}
