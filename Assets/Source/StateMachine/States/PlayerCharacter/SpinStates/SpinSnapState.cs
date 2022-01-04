using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The state the player will be in after letting go of the spin direction while
/// spinning. Will snap the player back to the ready state quickly a la SSX
/// </summary>
public class SpinSnapState : iState
{

    private AerialMoveData c_aerialMoveData;
    private PlayerPositionData c_playerPosData;
    private TrickPhysicsData c_physData;
    private ScoringData c_scoringData;

    public SpinSnapState(ref AerialMoveData aerialIn,
        ref PlayerPositionData playerPosIn,
        ref TrickPhysicsData trickIn,
        ref ScoringData scoringIn)
    {
        this.c_aerialMoveData = aerialIn;
        this.c_playerPosData = playerPosIn;
        this.c_physData = trickIn;
        this.c_scoringData = scoringIn;
    }

    public void Act()
    {
        Quaternion root = c_physData.q_startRotation;
        Quaternion currentRotation = c_physData.q_startRotation;

        Vector3 spinAxis = Vector3.up;
        Vector3 flipAxis = Vector3.right;

        float spinCeiling = c_scoringData.f_currentSpinTarget;
        float flipCeiling = c_scoringData.f_currentFlipTarget;

        float currentSpinRate = c_physData.f_currentSpinRate;
        float currentFlipRate = c_physData.f_currentFlipRate;

        float currentSpinDegrees = c_physData.f_currentSpinDegrees;
        float currentFlipDegrees = c_physData.f_currentFlipDegrees;

        HandlingCartridge.Turn(flipAxis, currentFlipDegrees, ref root);
        HandlingCartridge.Turn(spinAxis, currentSpinDegrees, ref root);
        HandlingCartridge.SetRotation(ref currentRotation, root);
        HandlingCartridge.ValidateSpinRotation(currentSpinDegrees, currentFlipDegrees, spinCeiling, flipCeiling, ref currentSpinRate, ref currentFlipRate);

        c_playerPosData.q_centerOfGravityRotation = currentRotation;
        c_physData.f_currentFlipRate = currentFlipRate;
        c_physData.f_currentSpinRate = currentSpinRate;
        c_physData.f_currentSpinDegrees += currentSpinRate * 360f * Time.fixedDeltaTime;
        c_physData.f_currentFlipDegrees += currentFlipRate * 360f * Time.fixedDeltaTime;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.CRASH ||
            cmd == Command.LAND)
        {
            return StateRef.SPIN_IDLE;
        }
        return StateRef.SPIN_RESET;
    }

    public void TransitionAct()
    {
        // nothing to do!
        if (c_physData.f_currentSpinRate > 0)
        {
            c_physData.f_currentSpinRate = c_physData.f_resetRate;
            c_scoringData.f_currentSpinTarget = Mathf.Ceil(c_physData.f_currentSpinDegrees / 180) * 180;
        }
        else if (c_physData.f_currentSpinRate < 0)
        {
            c_physData.f_currentSpinRate = c_physData.f_resetRate * -1;
            c_scoringData.f_currentSpinTarget = Mathf.Floor(c_physData.f_currentSpinDegrees / 180) * 180;
        }

        if (c_physData.f_currentFlipRate > 0)
        {
            c_physData.f_currentFlipRate = c_physData.f_resetRate;
            c_scoringData.f_currentFlipTarget = Mathf.Ceil(c_physData.f_currentFlipDegrees / 360) * 360;
        }
        else if (c_physData.f_currentFlipRate < 0)
        {
            c_physData.f_currentFlipRate = c_physData.f_resetRate * -1;
            c_scoringData.f_currentFlipTarget = Mathf.Floor(c_physData.f_currentFlipDegrees / 360) * 360;
        }

    }
}
