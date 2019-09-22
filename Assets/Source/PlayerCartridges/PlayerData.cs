using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {
    
    #region Members
    [SerializeField] private float f_topSpeed;
    [SerializeField] private float f_acceleration;

    private float f_currentSpeed;

    private Vector3 v_currentPosition;
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
    #endregion

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
}
