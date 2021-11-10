using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterController 
{
    /* Data Structures required:
     * Players list
     *     - trick score
     *     - rank (potentially extrapolated by position/zones)
     *     - is player finished
     * 
     * Zones lookup
     *     - reference to the transform of each zone
     *     - attached ZoneController component
     *     - enables cached lookup on raycast hit so zones can send appropriate messages
     */
    private static uint u_nextAvailableId = 0;
    private static Dictionary<Transform, ZoneController> l_zones;

    public static uint GetNextAvailableID()
    {
        uint availableId = u_nextAvailableId;
        u_nextAvailableId++;
        return availableId;
    }

    public static bool AddZoneToList(ref Transform transformIn, ZoneController controllerIn)
    {
        if (l_zones == null)
        {
            l_zones = new Dictionary<Transform, ZoneController>();
        }

        if (l_zones.ContainsKey(transformIn))
        {
            return false;
        }

        l_zones.Add(transformIn, controllerIn);

        return true;
    }

    public static ZoneController LookupZoneController(Transform transformIn)
    {
        if (l_zones.TryGetValue(transformIn, out ZoneController zOut))
        {
            return zOut;
        }
        else
        {
            return null;
        }
    }
}
