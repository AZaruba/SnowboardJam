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
    CHARGE,
    CRASH,
    BONK,
    LAND,
    FALL,
    JUMP,
    #endregion

    #region CameraCommands
    APPROACH,
    DRAG,
    TRACK,
    REST,
    SPEEDUP,
    SLOWDOWN,
    #endregion
}

public enum StateRef {
    
    ERROR_STATE = -1,
    START_STATE,
    DISABLED,

    #region PlayerStates
    IDLE,
    STATIONARY,
    RIDING,
    STOPPING,
    BONKED,
    CRASHED,
    CARVING,
    CHARGING,
    AIRBORNE,
    JUMPING,
    TRICKING,
    LANDING,
    #endregion

    #region TurnStates

    #endregion

    #region AirStates
    GROUNDED,
    #endregion

    #region CameraStates
    // STATIONARY,
    APPROACHING,
    LEAVING,
    TRACKING,
    APPROACHING_FAST,
    LEAVING_FAST,
    TRACKING_FAST,
    #endregion

}