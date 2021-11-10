using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionData
{
    private uint OccupiedZone;
    private Vector3 ModelDirection;
    private Vector3 ModelPosition;
    private Quaternion ModelRotation;

    private int ModelReversed;

    public PlayerPositionData(Vector3 position, Vector3 direction, Quaternion rotation)
    {
        this.OccupiedZone = uint.MaxValue;
        this.v_modelPosition = position;
        this.v_modelDirection = direction;
        this.i_switchStance = Constants.REGULAR_STANCE;
        this.q_currentModelRotation = rotation;
    }

    public uint u_zone
    {
        get { return OccupiedZone; }
        set { OccupiedZone = value; }
    }

    public Vector3 v_modelDirection
    {
        get { return ModelDirection; }
        set { ModelDirection = value; }
    }

    public Vector3 v_modelPosition
    {
        get { return ModelPosition; }
        set { ModelPosition = value; }
    }

    public Quaternion q_currentModelRotation
    {
        get { return ModelRotation; }
        set { ModelRotation = value; }
    }

    public int i_switchStance
    {
        get { return ModelReversed; }
        set { ModelReversed = value; }
    }
}
