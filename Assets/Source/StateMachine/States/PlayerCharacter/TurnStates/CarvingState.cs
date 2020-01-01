using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: The Turn function causes strange behavior on slopes and angle changes
public class CarvingState : iState {

    HandlingCartridge         cart_handling;
    PlayerData                c_playerData;

    //TODO: Investigate breaking out certain actions into a separate state machine
    //TODO: Give player data to the state machine and possibly the states by reference
    public CarvingState(ref PlayerData playerData,
                        ref HandlingCartridge handling)
    {
        this.c_playerData = playerData;
        this.cart_handling = handling;
    }

    public void Act()
    {
        Vector3 currentDir = c_playerData.v_currentDirection;
        Vector3 currentNormal = c_playerData.v_currentNormal;
        Quaternion currentRotation = c_playerData.q_currentRotation;

        float inputAxis = c_playerData.f_inputAxisTurn * c_playerData.f_turnSpeed;

        cart_handling.Turn(ref currentDir, ref currentNormal, ref inputAxis, ref currentRotation);

        c_playerData.v_currentDirection = currentDir.normalized;
        c_playerData.q_currentRotation = currentRotation;
    }

    public void TransitionAct()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.FALL)
        {
            return StateRef.DISABLED;
        }
        if (cmd == Command.RIDE)
        {
            return StateRef.RIDING;
        }
        return StateRef.CARVING;
    }
}
