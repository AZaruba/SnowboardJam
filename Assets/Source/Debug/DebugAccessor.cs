using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAccessor : MonoBehaviour
{
    [SerializeField] DebugFunctions debugInstance;

    public void DisplayFloat(string varName, float num)
    {
        debugInstance.DisplayFloat(varName, num);
    }

    public void DisplayState(string objName, StateRef state)
    {
        debugInstance.DisplayState(objName, state);
    }

    public void DisplayVector3(string objName, Vector3 vec)
    {
        debugInstance.DisplayVector3(objName, vec);
    }
}
