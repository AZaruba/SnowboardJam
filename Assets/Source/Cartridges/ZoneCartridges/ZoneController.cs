using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneController
{
    [SerializeField] private ZoneType Type;

    public ZoneType e_type
    {
        get { return Type; }
        set { Type = value; }
    }
}
