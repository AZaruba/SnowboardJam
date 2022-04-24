using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningState : iState
{
    private TrickPhysicsData c_physData;
    private PlayerPositionData c_playerPosData;
    private PlayerInputData c_playerInput;

    public SpinningState(ref TrickPhysicsData dataIn, ref PlayerPositionData posIn, ref PlayerInputData inputIn)
    {
        this.c_physData = dataIn;
        this.c_playerPosData = posIn;
        this.c_playerInput = inputIn;
    }

    public void Act()
    {
        Quaternion root = c_physData.q_startRotation;
        Quaternion currentRotation = c_physData.q_startRotation;

        Vector3 spinAxis = Vector3.up;
        Vector3 flipAxis = Vector3.right;

        float currentSpinRate = c_physData.f_currentSpinRate;
        float currentFlipRate = c_physData.f_currentFlipRate;

        float currentSpinDegrees = c_physData.f_currentSpinDegrees;
        float currentFlipDegrees = c_physData.f_currentFlipDegrees;

        float flipChargeRate = c_physData.f_flipIncrement * c_playerInput.f_inputAxisLVert;
        float spinChargeRate = c_physData.f_spinIncrement * c_playerInput.f_inputAxisLHoriz;

        HandlingCartridge.Turn(flipAxis, currentFlipDegrees, ref root);
        HandlingCartridge.Turn(spinAxis, currentSpinDegrees, ref root);
        HandlingCartridge.SetRotation(ref currentRotation, root);

        IncrementCartridge.IncrementAbs(ref currentFlipRate, flipChargeRate * Time.fixedDeltaTime, c_physData.f_maxFlipRate);
        IncrementCartridge.IncrementAbs(ref currentSpinRate, spinChargeRate * Time.fixedDeltaTime, c_physData.f_maxSpinRate);

        c_playerPosData.q_centerOfGravityRotation = currentRotation;
        c_physData.f_currentFlipRate = currentFlipRate;
        c_physData.f_currentSpinRate = currentSpinRate;
        c_physData.f_currentSpinDegrees += currentSpinRate * 360f * Time.fixedDeltaTime;
        c_physData.f_currentFlipDegrees += currentFlipRate * 360f * Time.fixedDeltaTime;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.CRASH)
        {
            return StateRef.SPIN_IDLE;
        }
        if (cmd == Command.LAND)
        {
            return StateRef.SPIN_CORRECT;
        }
        if (cmd == Command.SPIN_STOP)
        {
            return StateRef.SPIN_IDLE;
        }
        return StateRef.SPINNING;
    }

    public void TransitionAct()
    {

    }
}
