using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioRef
{
    ERROR_CLIP = -1,
    NO_AUDIO,

    RIDE_SNOW,
    RIDE_POWDER,
    RIDE_ICE,

    SLOW_SNOW,
    SLOW_POWDER,
    SLOW_ICE,

    TURN_SNOW,
    TURN_POWDER,
    TURN_ICE,

    JUMP,
    GRAB_BOARD,
}

public class AudioBank : MonoBehaviour
{
    public List<AudioRef> AudioReferences;
    public List<AudioClip> AudioClips;
}
