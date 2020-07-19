using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains the basic information that judges a character's
/// in game information as well as data to be displayed in
/// UI instances
/// </summary>
[CreateAssetMenu(fileName = "CharacterAttributeData", menuName = "Charater Attribute Data")]
public class CharacterAttributeData : ScriptableObject
{
    #region PUBLIC_VARIABLES
    public string Name;
    public CharacterSelection selectionId;
    public int Speed;
    public int Balance;
    public int Tricks;
    #endregion
}
