using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningState : iState
{
    private TrickPhysicsData c_physData;
    private PlayerPositionData c_playerPosData;
    private ScoringData c_scoringData;
    private IncrementCartridge cart_incr;

    public SpinningState(ref TrickPhysicsData dataIn, ref PlayerPositionData posIn,
        ref IncrementCartridge incrIn, ref ScoringData scoringIn)
    {
        this.c_physData = dataIn;
        this.c_playerPosData = posIn;
        this.cart_incr = incrIn;
        this.c_scoringData = scoringIn;
    }

    public void Act()
    {
        Quaternion root = c_physData.q_startRotation;
        Quaternion currentRotation = c_physData.q_startRotation;

        Vector3 spinAxis = Vector3.up;
        Vector3 flipAxis = Vector3.right;

        float currentSpinRate = c_physData.f_currentSpinRate;
        float currentFlipRate = c_physData.f_currentFlipRate;

        float currentSpinDegrees = c_scoringData.f_currentSpinDegrees;
        float currentFlipDegrees = c_scoringData.f_currentFlipDegrees;

        HandlingCartridge.Turn(flipAxis, currentFlipDegrees, ref root);
        HandlingCartridge.Turn(spinAxis, currentSpinDegrees, ref root);
        HandlingCartridge.SetRotation(ref currentRotation, root);

        cart_incr.DecrementAbs(ref currentFlipRate, c_physData.f_flipDecay * Time.deltaTime, 0.0f);
        cart_incr.DecrementAbs(ref currentSpinRate, c_physData.f_spinDecay * Time.deltaTime, 0.0f);

        c_playerPosData.q_centerOfGravityRotation = currentRotation;
        c_physData.f_currentFlipRate = currentFlipRate;
        c_physData.f_currentSpinRate = currentSpinRate;
        c_scoringData.f_currentSpinDegrees += currentSpinRate * 360f * Time.deltaTime;
        c_scoringData.f_currentFlipDegrees += currentFlipRate * 360f * Time.deltaTime;
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
            return StateRef.SPIN_RESET;
        }
        return StateRef.SPINNING;
    }

    public void TransitionAct()
    {
        c_physData.f_currentFlipRate = c_physData.f_currentFlipCharge;
        c_physData.f_currentSpinRate = c_physData.f_currentSpinCharge;
        c_physData.q_startRotation = c_playerPosData.q_centerOfGravityRotation;
    }
}
