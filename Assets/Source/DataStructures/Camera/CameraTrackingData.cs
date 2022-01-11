using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrackingData
{
    private Vector3 Position;
    private Vector3 Position_LastFrame;

    public CameraTrackingData()
    {
        this.Position = Vector3.zero;
        this.Position_LastFrame = Vector3.zero;
    }

    public Vector3 v_position
    {
        get { return Position; }
        set { Position = value; }
    }

    public Vector3 v_position_lastFrame
    {
        get { return Position_LastFrame; }
        set { Position_LastFrame = value; }
    }
}
