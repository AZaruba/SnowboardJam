using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraData : MonoBehaviour {

    #region Members
    [SerializeField] private float f_followDistance;
    [SerializeField] private Vector3 v_offsetVector;
    [SerializeField] private GameObject go_target;

    private float f_fieldOfView;

    private Vector3 v_currentPosition;
    private Vector3 v_currentDirection;

    private Vector3 v_targetPosition;
    #endregion

    #region Getters

    public float GetFollowDistance()
    {
        return f_followDistance;
    }

    public float GetFOV()
    {
        return f_fieldOfView;
    }

    public Vector3 GetCurrentPosition()
    {
        return v_currentPosition;
    }

    public Vector3 GetCurrentDirection()
    {
        return v_currentDirection;
    }

    public Vector3 GetOffsetVector()
    {
        return v_offsetVector;
    }

    public Vector3 GetTargetPosition()
    {
        return v_targetPosition;
    }

    public GameObject GetTarget()
    {
        return go_target;
    }
    #endregion

    #region Setters
    public void SetFollowDistance(float newFollowDistance)
    {
        f_followDistance = newFollowDistance;
    }

    public void SetFOV(float newFOV)
    {
        f_fieldOfView = newFOV;
    }

    public void SetCurrentPosition(Vector3 newPosition)
    {
        v_currentPosition = newPosition;
    }

    public void SetCurrentDirection(Vector3 newDirection)
    {
        v_currentDirection = newDirection;
    }

    public void SetOffsetVector(Vector3 newOffset)
    {
        v_offsetVector = newOffset;
    }

    public void SetTargetPosition(Vector3 newPosition)
    {
        v_targetPosition = newPosition;
    }
    #endregion
}
