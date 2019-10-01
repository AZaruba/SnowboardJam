using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratingTurningState : iState {

    private AccelerationCartridge cart_acceleration;
    private VelocityCartridge cart_velocity;
    private HandlingCartridge cart_handling;

    public AcceleratingTurningState(ref AccelerationCartridge cart_a, 
                                    ref VelocityCartridge cart_v, 
                                    ref HandlingCartridge cart_h)
    {
        this.cart_acceleration = cart_a;
        this.cart_velocity = cart_v;
        this.cart_handling = cart_h;
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
        float f_turnSpeed = c_playerData.GetTurnSpeed();

        Vector3 v_currentPosition = c_playerData.GetCurrentPosition();
        Vector3 v_currentDirection = c_playerData.GetCurrentDirection();

        cart_acceleration.Accelerate(ref f_currentSpeed, ref f_acceleration);
        cart_handling.Turn(ref v_currentDirection, ref f_turnSpeed);
        cart_velocity.UpdatePosition(ref v_currentPosition, ref v_currentDirection, ref f_currentSpeed);

        c_playerData.SetCurrentPosition(v_currentPosition);
        c_playerData.SetCurrentSpeed(f_currentSpeed);
        c_playerData.SetCurrentDirection(v_currentDirection);
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
            return StateRef.COASTING_TURNING;
        }
        if (cmd == Command.TURN_END)
        {
            return StateRef.ACCELERATING;
        }
        return StateRef.ACCELERATING;
    }
}
