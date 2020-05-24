using UnityEngine;

public class AerialMoveData
{
    private Vector3 LateralDirection;

    private float LateralVelocity;
    private float VerticalVelocity;

    public Vector3 v_lateralDirection
    {
        get { return LateralDirection; }
        set { LateralDirection = value; }
    }

    public float f_lateralVelocity
    {
        get { return LateralVelocity; }
        set { LateralVelocity = value; }
    }

    public float f_verticalVelocity
    {
        get { return VerticalVelocity; }
        set { VerticalVelocity = value; }
    }

    public AerialMoveData()
    {
        SetDefaultData();
    }

    public void SetDefaultData()
    {
        LateralDirection = Vector3.up;
        LateralVelocity = 0f;
        VerticalVelocity = 0f;
    }
}
