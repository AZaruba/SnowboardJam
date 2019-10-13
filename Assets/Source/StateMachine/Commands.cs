using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Command {
    
    // Car Movement Commands
    ERROR_CMD = -1,
    RESET,

    #region PlayerCommand
    RIDE,
    TURN,
    SLOW,
    STOP, // when velocity reaches zero
    LAND,
    FALL,
    JUMP,
    #endregion

    #region CameraCommands
    TRACK,
    REST,
    TRACK_FAST,
    #endregion
}

public enum StateRef {
    
    ERROR_STATE = -1,
    START_STATE,

    #region PlayerStates
    STATIONARY,
    RIDING,
    STOPPING,
    CARVING,
    CARVING_STOPPING,
    CHARGING,
    CARVING_CHARGING,
    AIRBORNE,
    JUMPING,
    TRICKING,
    LANDING,
    #endregion

    #region CameraStates
    // STATIONARY,
    TRACKING,
    TRACKING_FAST,
    #endregion

}