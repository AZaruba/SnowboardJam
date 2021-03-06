﻿using System.Collections;
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
    READY,
    #endregion

    #region PlayerMoveCommands
    STARTMOVE,
    #endregion

    #region CameraPositionCommands
    APPROACH,
    DRAG,
    TRACK,
    REST,
    SPEEDUP,
    SLOWDOWN,
    #endregion

    #region CameraOrientationCommands
    POINT_AT_TARGET,
    POINT_IN_DIRECTION,
    POINT_AT_POSITION,
    #endregion

    #region TrickCommands
    START_TRICK,
    SCORE_TRICK,
    END_TRICK,
    READY_TRICK,
    #endregion

    #region ScoreDisplayCommands
    INCREMENT_SCORE,
    STOP_SCORE,
    PAUSE_SCORE,
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

    #region CameraOrientationStates
    TARGETED,
    DIRECTED,
    POSED,
    #endregion

    #region TrickStates
    TRICK_READY,
    TRICK_TRANSITION,
    TRICKING,
    TRICK_DISABLED,
    #endregion

    #region ScoreDisplayStates
    SCORE_PAUSED,
    SCORE_INCREASING,
    SCORE_STAY,
    #endregion

}