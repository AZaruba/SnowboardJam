using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionData
{
    private uint OccupiedZone;

    public PlayerPositionData()
    {
        this.OccupiedZone = uint.MaxValue;
    }

    public uint u_zone
    {
        get { return OccupiedZone; }
        set { OccupiedZone = value; }
    }
}
