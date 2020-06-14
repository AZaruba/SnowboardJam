using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionData
{
    private uint OccupiedZone;
    private Vector3 ModelDirection;
    private Vector3 ModelPosition;

    private bool ModelReversed;

    public PlayerPositionData(Vector3 position, Vector3 direction)
    {
        this.OccupiedZone = uint.MaxValue;
        this.v_modelPosition = position;
        this.v_modelDirection = direction;
        this.b_modelReversed = false;
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

    public bool b_modelReversed
    {
        get { return ModelReversed; }
        set { ModelReversed = value; }
    }
}
