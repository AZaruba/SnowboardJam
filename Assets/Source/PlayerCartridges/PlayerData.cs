using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {
    
    #region Members
    [SerializeField] private float f_topSpeed;
    [SerializeField] private float f_acceleration;
    [SerializeField] private float f_turnSpeed;

    private float f_currentSpeed;

    private Vector3 v_currentPosition;
    private Vector3 v_currentDirection;
    #endregion

    #region IOMembers

    private float f_inputAxisTurn;

    #endregion

    #region Getters
    /// <summary>
    /// Gets the top speed.
    /// </summary>
    /// <returns>f_topSpeed</returns>
    public float GetTopSpeed()
    {
        return f_topSpeed;
    }

    /// <summary>
    /// Gets the acceleration.
    /// </summary>
    /// <returns>f_acceleration</returns>
    public float GetAcceleration()
    {
        return f_acceleration;
    }

    public float GetTurnSpeed()
    {
        return f_turnSpeed;
    }

    /// <summary>
    /// Gets the current speed.
    /// </summary>
    /// <returns>f_currentSpeed</returns>
    public float GetCurrentSpeed()
    {
        return f_currentSpeed;
    }

    /// <summary>
    /// Gets the current position.
    /// </summary>
    /// <returns>v_currentPosition</returns>
    public Vector3 GetCurrentPosition()
    {
        return v_currentPosition;
    }

    /// <summary>
    /// Gets the current direction.
    /// </summary>
    /// <returns>v_currentDirection</returns>
    public Vector3 GetCurrentDirection()
    {
        return v_currentDirection;
    }
    #endregion

    /// Setters should be only for "active" data (that is, data that is used by the engine)
    #region Setters
    /// <summary>
    /// Sets the current speed.
    /// </summary>
    /// <param name="newSpeed">The new f_currentSpeed</param>
    public void SetCurrentSpeed(float newSpeed)
    {
        if (newSpeed > f_topSpeed)
        {
            f_currentSpeed = f_topSpeed;
        } 
        else if (newSpeed < 0)
        {
            f_currentSpeed = 0.0f;
        }
        else
        {
            f_currentSpeed = newSpeed;
        }
    }

    /// <summary>
    /// Sets the current position.
    /// </summary>
    /// <param name="newPosition">The new v_currentPosition</param>
    public void SetCurrentPosition(Vector3 newPosition)
    {
        v_currentPosition = newPosition;
    }
    #endregion

    void Start()
    {
        f_currentSpeed = 0.0f;
    }

    /// <summary>
    /// Sets the current direction.
    /// </summary>
    /// <param name="newDirection">The new v_currentDirection.</param>
    public void SetCurrentDirection(Vector3 newDirection)
    {
        v_currentDirection = newDirection;
    }

    /// <summary>
    /// Gets f_inputAxisTurn;
    /// </summary>
    /// <returns>f_inputAxisTurn</returns>
    public float GetInputAxisTurn()
    {
        return f_inputAxisTurn;
    }

    /// <summary>
    /// Sets f_inputAxisTurn
    /// </summary>
    /// <param name="axisValue">A float between -1 and 1</param></param>
    public void SetInputAxisTurn(float axisValue)
    {
        f_inputAxisTurn = axisValue;
    }
}
