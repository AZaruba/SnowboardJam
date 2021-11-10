using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Trick
{
    public Trick(int pv, int mpv)
    {
        pointValue = pv;
        minimumPointValue = mpv;
    }
    public int pointValue;
    public int minimumPointValue;
}

public enum TrickName
{
    BLANK_TRICK = -1,
    AIR,

    #region UpTricks
    NOSEGRAB,
    ROCKET,
    CRAIL,
    #endregion

    #region LeftTricks
    METHOD,
    STALEFISH,
    MELON,
    #endregion

    #region RightTricks
    INDY,
    WEDDLE,
    STIFFY,
    #endregion

    #region DownTricks
    TAILGRAB,
    JAPAN,
    SEATBELT,
    #endregion
}