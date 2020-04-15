using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
    [SerializeField] private ZoneType Type;
    [SerializeField] private uint ZoneId;
    [SerializeField] private Transform this_transform;


    void Start()
    {
        GameMasterController.AddZoneToList(ref this_transform, this);
    }

    public ZoneType e_type
    {
        get { return Type; }
        set { Type = value; }
    }

    public uint u_zoneId
    {
        get { return ZoneId; }
        set { ZoneId = value; }
    }
}
