using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicMenuControllerData", menuName = "Basic Menu Controller Data")]
public class BasicMenuControllerData : ScriptableObject
{
    #region PUBLIC_VARIABLES
    public float ShortTickTime;
    public float LongTickTime;
    public Vector2 DisabledPosition;
    public Vector2 EnabledPosition;
    public float EnabledOpacity;
    public float DisabledOpacity;
    #endregion
}
