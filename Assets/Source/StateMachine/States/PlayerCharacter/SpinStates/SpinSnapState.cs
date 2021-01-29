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

    private HandlingCartridge cart_rotation;

    public SpinSnapState(ref AerialMoveData aerialIn,
        ref PlayerPositionData playerPosIn,
        ref TrickPhysicsData trickIn,
        ref HandlingCartridge handleIn,
        ref ScoringData scoringIn)
    {
        this.c_aerialMoveData = aerialIn;
        this.c_playerPosData = playerPosIn;
        this.c_physData = trickIn;
        this.cart_rotation = handleIn;
        this.c_scoringData = scoringIn;
    }

    public void Act()
    {
        Quaternion root = c_physData.q_startRotation;
        Quaternion currentRotation = c_physData.q_startRotation;

        Vector3 playerForward = c_playerPosData.v_modelDirection;
        Vector3 spinAxis = Vector3.up;
        Vector3 flipAxis = Vector3.right;

        float spinCeiling = c_scoringData.f_currentSpinTarget;
        float flipCeiling = c_scoringData.f_currentFlipTarget;

        float currentSpinRate = c_physData.f_currentSpinRate;
        float currentFlipRate = c_physData.f_currentFlipRate;

        float currentSpinDegrees = c_scoringData.f_currentSpinDegrees;
        float currentFlipDegrees = c_scoringData.f_currentFlipDegrees;

        HandlingCartridge.Turn(ref playerForward, flipAxis, currentFlipDegrees, ref root);
        HandlingCartridge.Turn(ref playerForward, spinAxis, currentSpinDegrees, ref root);
        cart_rotation.SetRotation(ref currentRotation, root);
        cart_rotation.ValidateSpinRotation(currentSpinDegrees, currentFlipDegrees, spinCeiling, flipCeiling, ref currentSpinRate, ref currentFlipRate);

        c_playerPosData.q_currentModelRotation = currentRotation;
        c_playerPosData.v_modelDirection = playerForward;
        c_physData.f_currentFlipRate = currentFlipRate;
        c_physData.f_currentSpinRate = currentSpinRate;
        c_scoringData.f_currentSpinDegrees += currentSpinRate * 360f * Time.deltaTime;
        c_scoringData.f_currentFlipDegrees += currentFlipRate * 360f * Time.deltaTime;
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
            c_scoringData.f_currentSpinTarget = Mathf.Ceil(c_scoringData.f_currentSpinDegrees / 180) * 180;
        }
        else if (c_physData.f_currentSpinRate < 0)
        {
            c_physData.f_currentSpinRate = c_physData.f_resetRate * -1;
            c_scoringData.f_currentSpinTarget = Mathf.Floor(c_scoringData.f_currentSpinDegrees / 180) * 180;
        }

        if (c_physData.f_currentFlipRate > 0)
        {
            c_physData.f_currentFlipRate = c_physData.f_resetRate;
            c_scoringData.f_currentFlipTarget = Mathf.Ceil(c_scoringData.f_currentFlipDegrees / 360) * 360;
        }
        else if (c_physData.f_currentFlipRate < 0)
        {
            c_physData.f_currentFlipRate = c_physData.f_resetRate * -1;
            c_scoringData.f_currentFlipTarget = Mathf.Floor(c_scoringData.f_currentFlipDegrees / 360) * 360;
        }

    }
}
