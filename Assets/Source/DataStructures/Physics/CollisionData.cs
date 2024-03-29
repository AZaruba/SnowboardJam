﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionData 
{
    private Vector3 FrontNormal;
    private Vector3 FrontPoint;
    private Vector3 FrontOffset;
    private Vector3 BackNormal;
    private Vector3 BackPoint;
    private Vector3 BackOffset;
    private Vector3 PreviousPosition;
    private Vector3 AttachPoint;

    private Vector3 ObstacleNormal;
    private Vector3 ObstaclePoint;

    private float FrontRayLengthUp;
    private float FrontRayLengthDown;
    private float ObstacleRayLength;

    private float CurrentObstacleAngle;
    private float ContactOffset;

    private Vector3 SurfaceNormal;

    private bool CollisionDetected;

    public float f_obstacleAngle
    {
        get { return CurrentObstacleAngle; }
        set { CurrentObstacleAngle = value; }
    }
    public float f_obstacleRayLength
    {
        get { return ObstacleRayLength; }
        set { ObstacleRayLength = value; }
    }

    public float f_frontRayLengthUp
    {
        get { return FrontRayLengthUp; }
        set { FrontRayLengthUp = value; }
    }

    public float f_frontRayLengthDown
    {
        get { return FrontRayLengthDown; }
        set { FrontRayLengthDown = value; }
    }

    public float f_contactOffset
    {
        get { return ContactOffset; }
        set { ContactOffset = value; }
    }

    public Vector3 v_obstacleNormal
    {
        get { return ObstacleNormal; }
        set { ObstacleNormal = value; }
    }

    public Vector3 v_obstaclePoint
    {
        get { return ObstaclePoint; }
        set { ObstaclePoint = value; }
    }

    public Vector3 v_frontNormal
    {
        get { return FrontNormal; }
        set { FrontNormal = value; }
    }

    public Vector3 v_frontPoint
    {
        get { return FrontPoint; }
        set { FrontPoint = value; }
    }

    public Vector3 v_frontOffset
    {
        get { return FrontOffset; }
        set { FrontOffset = value; }
    }

    public Vector3 v_backNormal
    {
        get { return BackNormal; }
        set { BackNormal = value; }
    }
    public Vector3 v_backPoint
    {
        get { return BackPoint; }
        set { BackPoint = value; }
    }

    public Vector3 v_backOffset
    {
        get { return BackOffset; }
        set { BackOffset = value; }
    }

    public Vector3 v_previousPosition
    {
        get { return PreviousPosition; }
        set { PreviousPosition = value; }
    }

    public Vector3 v_attachPoint
    {
        get { return AttachPoint; }
        set { AttachPoint = value; }
    }

    public Vector3 v_surfaceNormal
    {
        get { return SurfaceNormal; }
        set { SurfaceNormal = value; }
    }

    public bool b_collisionDetected
    {
        get { return CollisionDetected; }
        set { CollisionDetected = value; }
    }

    public CollisionData(Vector3 frontOffsetIn, Vector3 backOffsetIn)
    {
        FrontNormal = Vector3.zero;
        FrontPoint = Vector3.zero;
        FrontOffset = frontOffsetIn;
        BackNormal = Vector3.zero;
        BackOffset = backOffsetIn;
        PreviousPosition = Vector3.zero;
        AttachPoint = Vector3.zero;

        SurfaceNormal = Vector3.zero;

        FrontRayLengthUp = Constants.ZERO_F;
        FrontRayLengthDown = Constants.ZERO_F;
        ContactOffset = Constants.ZERO_F;
        CollisionDetected = false;

        CurrentObstacleAngle = 45f;
    }
}
