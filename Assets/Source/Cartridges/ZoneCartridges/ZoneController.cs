using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
    [SerializeField] private ZoneType Type;
    [SerializeField] private uint ZoneId;
    [SerializeField] private Transform this_transform;

    private static Dictionary<uint, ZoneType> TypeLookup; // could this be moved elsewhere? Gamemaster "reverse" lookup?
    
    public static void AddZoneToLookup(uint intIn, ZoneType typeIn)
    {
        if (TypeLookup == null)
        {
            TypeLookup = new Dictionary<uint, ZoneType>();
        }

        TypeLookup.Add(intIn, typeIn);
    }

    public static ZoneType GetZoneType(uint idIn)
    {
        ZoneType typeOut;
        if (TypeLookup.TryGetValue(idIn, out typeOut))
        {
            return typeOut;
        }
        return ZoneType.ERROR_ZONE;
    }

    void Start()
    {
        GameMasterController.AddZoneToList(ref this_transform, this);
        AddZoneToLookup(ZoneId, Type);
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
