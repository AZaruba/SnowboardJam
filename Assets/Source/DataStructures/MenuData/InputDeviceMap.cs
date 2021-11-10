using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputDeviceMap", menuName = "Input Device Map")]
public class InputDeviceMap : ScriptableObject
{
    [SerializeField] public List<KeyCode> SpriteCodes;
    [SerializeField] public List<Sprite> Sprites;
}
