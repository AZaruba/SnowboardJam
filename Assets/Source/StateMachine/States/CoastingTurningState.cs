using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoastingTurningState : iState {

    private AccelerationCartridge cart_acceleration;
    private VelocityCartridge cart_velocity;
    private HandlingCartridge cart_handling;

    public CoastingTurningState(ref AccelerationCartridge cart_a, ref VelocityCartridge cart_v, ref HandlingCartridge cart_h)
    {
        this.cart_acceleration = cart_a;
        this.cart_velocity = cart_v;
        this.cart_handling = cart_h;
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
        else if (cmd == Command.TURN_END)
        {
            return StateRef.COASTING;
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
        float f_turnSpeed = c_playerData.GetTurnSpeed();

        Vector3 v_currentPosition = c_playerData.GetCurrentPosition();
        Vector3 v_currentDirection = c_playerData.GetCurrentDirection();

        cart_acceleration.Decelerate(ref f_currentSpeed, ref f_deceleration);
        cart_handling.Turn(ref v_currentDirection, ref f_turnSpeed);
        cart_velocity.UpdatePosition(ref v_currentPosition, ref v_currentDirection, ref f_currentSpeed);

        c_playerData.SetCurrentPosition(v_currentPosition);
        c_playerData.SetCurrentDirection(v_currentDirection);

        return f_currentSpeed;
    }
}
