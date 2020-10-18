using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionData 
{
    private Vector3 FrontNormal;
    private Vector3 BackNormal;
    private Vector3 CenterNormal;
    private Vector3 AttachPoint;
    private Vector3 FrontPoint; // no back point needed as that's always a known quantity
    private Vector3 FrontOffset;

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

    public CollisionData()
    {
        FrontNormal = Vector3.zero;
        FrontPoint = Vector3.zero;
        FrontOffset = Vector3.zero;
        BackNormal = Vector3.zero;
        CenterNormal = Vector3.zero;
        AttachPoint = Vector3.zero;
    }
}
