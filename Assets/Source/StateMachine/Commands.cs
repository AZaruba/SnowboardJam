using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Command {

    // Car Movement Commands
    ERROR_CMD = -1,
    RESET,
    ACCELERATE,
    TURN,
    TURN_END,
    BRAKE,
    COAST,
    STOP,

}

public enum StateRef {
    ERROR_STATE = -1,
    START_STATE,

    STATIONARY,
    ACCELERATING,
    COASTING,
    BRAKING,

    ACCELERATING_TURNING,
    COASTING_TURNING,
    BRAKING_TURNING,

    AIRBORNE,
}