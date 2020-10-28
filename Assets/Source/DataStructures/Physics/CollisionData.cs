using System.Collections;
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
    private Vector3 CenterNormal;
    private Vector3 AttachPoint;


    private float FrontRayLengthUp;
    private float FrontRayLengthDown;

    private Vector3 SurfaceNormal;

    private bool CollisionDetected;

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

    public Vector3 v_centerNormal
    {
        get { return CenterNormal; }
        set { CenterNormal = value; }
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
        CenterNormal = Vector3.zero;
        AttachPoint = Vector3.zero;

        SurfaceNormal = Vector3.zero;

        FrontRayLengthUp = Constants.ZERO_F;
        FrontRayLengthDown = Constants.ZERO_F;
        CollisionDetected = false;
    }
}
