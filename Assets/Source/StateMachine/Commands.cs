using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Command {
    
    // Car Movement Commands
    ERROR_CMD = -1,
    RESET,

    #region PlayerCommand
    ACCELERATE,
    TURN,
    TURN_END,
    BRAKE,
    COAST,
    STOP,
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
    ACCELERATING,
    COASTING,
    BRAKING,

    ACCELERATING_TURNING,
    COASTING_TURNING,
    BRAKING_TURNING,

    AIRBORNE,
    #endregion

    #region CameraStates
    // STATIONARY,
    TRACKING,
    TRACKING_FAST,
    #endregion

}