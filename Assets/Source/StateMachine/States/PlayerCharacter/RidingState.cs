using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidingState : iPlayerState {

    AngleCalculationCartridge cart_angleCalc;
    AccelerationCartridge     cart_acceleration;
    VelocityCartridge         cart_velocity;

    public RidingState(ref AngleCalculationCartridge angleCalc,
        ref AccelerationCartridge acceleration,
        ref VelocityCartridge velocity)
    {
        this.cart_angleCalc = angleCalc;
        this.cart_acceleration = acceleration;
        this.cart_velocity = velocity;
    }

    public void Act(ref PlayerData c_playerData)
    {
        // check for angle when implemented
        float currentVelocity = c_playerData.CurrentSpeed;
        float acceleration = c_playerData.Acceleration;
        Vector3 currentPos = c_playerData.CurrentPosition;
        Vector3 currentDir = c_playerData.CurrentDirection;

        cart_acceleration.Accelerate(ref currentVelocity, ref acceleration);
        cart_velocity.UpdatePosition(ref currentPos, ref currentDir, ref currentVelocity);

        c_playerData.CurrentSpeed = currentVelocity;
        c_playerData.Acceleration = acceleration;
        c_playerData.CurrentPosition = currentPos;
        c_playerData.CurrentDirection = currentDir;
    }

    public StateRef GetNextState(Command cmd)
    {
        return StateRef.RIDING;
    }
}
