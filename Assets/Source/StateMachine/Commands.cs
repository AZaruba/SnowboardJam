using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Command {
    
    // Car Movement Commands
    ERROR_CMD = -1,
    RESET,
    REPEAT, // used when we weant to just force a transition

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
    SPIN_STOP,
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

    #region MenuCommands
    SELECT,
    UNSELECT,
    END_TRANSITION,
    #endregion

    #region MenuInputCommands
    MENU_TICK_INPUT,
    MENU_READY,
    MENU_IDLE,
    MENU_SHOW,
    MENU_HIDE,
    #endregion

    #region TimerCommands
    START_TIMER_UP,
    START_TIMER_DOWN,
    STOP_TIMER,
    TICK_TIMER,
    START_COUNTDOWN,
    COUNTDOWN_OVER,
    #endregion
}

public enum StateRef {
    
    ERROR_STATE = -1,
    START_STATE,
    DISABLED,
    PAUSED,
    PRESTART_STATE,

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
    PREVIEW_TRACKING,

    FOLLOWING,
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

    #region SpinStates
    SPIN_IDLE,
    SPINNING,
    SPIN_CHARGE,
    SPIN_RESET,
    #endregion

    #region ScoreDisplayStates
    SCORE_PAUSED,
    SCORE_INCREASING,
    SCORE_STAY,
    #endregion

    #region MenuInputStates
    MENU_DISABLED,
    MENU_WAIT,
    MENU_READY,
    MENU_TICK,
    MENU_HIDDEN,
    MENU_SHOWN,
    #endregion

    #region MenuItemStates
    ITEM_SELECTED,
    ITEM_PRESELECTED,
    ITEM_UNSELECTED,
    ITEM_POSTSELECTED,
    #endregion

    #region TimerStates
    TIMER_INCR,
    TIMER_DECR,
    TIMER_STEP,
    #endregion

    #region TerrainStates
    TERR_SNOW,
    TERR_ICE,
    TERR_POWDER,
    #endregion

    TREE_ROOT,
}