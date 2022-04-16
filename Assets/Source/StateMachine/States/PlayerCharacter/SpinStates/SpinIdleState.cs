using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinIdleState : iState
{
    private TrickPhysicsData c_physData;
    private PlayerPositionData c_posData;

    public SpinIdleState(ref TrickPhysicsData dataIn, ref PlayerPositionData posDataIn)
    {
        this.c_physData = dataIn;
        this.c_posData = posDataIn;
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

        HandlingCartridge.Turn(flipAxis, currentFlipDegrees, ref root);
        HandlingCartridge.Turn(spinAxis, currentSpinDegrees, ref root);
        HandlingCartridge.SetRotation(ref currentRotation, root);

        IncrementCartridge.DecrementAbs(ref currentFlipRate, c_physData.f_flipDecay * Time.fixedDeltaTime, 0.0f);
        IncrementCartridge.DecrementAbs(ref currentSpinRate, c_physData.f_spinDecay * Time.fixedDeltaTime, 0.0f);

        c_posData.q_centerOfGravityRotation = currentRotation;
        c_physData.f_currentFlipRate = currentFlipRate;
        c_physData.f_currentSpinRate = currentSpinRate;
        c_physData.f_currentSpinDegrees += currentSpinRate * 360f * Time.fixedDeltaTime;
        c_physData.f_currentFlipDegrees += currentFlipRate * 360f * Time.fixedDeltaTime;

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.SPIN_START)
        {
            return StateRef.SPINNING;
        }
        if (cmd == Command.LAND)
        {
            return StateRef.SPIN_DISABLED;
        }
        return StateRef.SPIN_IDLE;
    }

    public void TransitionAct()
    {

    }
}
