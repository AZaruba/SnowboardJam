using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionData 
{
    private Vector3 FrontNormal;
    private Vector3 FrontPoint;
    private Vector3 BackNormal;
    private Vector3 BackPoint;
    private Vector3 CenterNormal;
    private Vector3 AttachPoint;
    private float FrontRayLength;

    private Vector3 SurfaceNormal;

    public float f_frontRayLength
    {
        get { return FrontRayLength; }
        set { FrontRayLength = value; }
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

    public CollisionData()
    {
        FrontNormal = Vector3.zero;
        FrontPoint = Vector3.zero;
        BackNormal = Vector3.zero;
        CenterNormal = Vector3.zero;
        AttachPoint = Vector3.zero;

        SurfaceNormal = Vector3.zero;

        FrontRayLength = Constants.ZERO_F;
    }
}
