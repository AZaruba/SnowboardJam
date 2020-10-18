using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains the basic information that judges a character's
/// in game information as well as data to be displayed in
/// UI instances
/// </summary>
[CreateAssetMenu(fileName = "CharacterCollisionData", menuName = "Charater Collision Data")]
public class CharacterCollisionData : ScriptableObject
{
    #region PUBLIC_VARIABLES
    public Vector3 FrontRayOffset;
    public Vector3 BackRayOffset;

    public Vector3 HalfExtents;
    public Vector3 CenterOffset;
    #endregion
}
