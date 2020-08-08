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
    private TrickPhysicsData c_trickPhys;
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
        this.c_trickPhys = trickIn;
        this.cart_rotation = handleIn;
        this.c_scoringData = scoringIn;
    }

    public void Act()
    {
        Quaternion currentOrientation = c_playerPosData.q_currentModelRotation;

        float spinRate = c_trickPhys.f_currentSpinRate;
        float flipRate = c_trickPhys.f_currentFlipRate;
        float spinAmount = c_scoringData.f_currentSpinDegrees;
        float flipAmount = c_scoringData.f_currentFlipDegrees;
        float spinCeiling = c_scoringData.f_currentSpinTarget;
        float flipCeiling = c_scoringData.f_currentFlipTarget;

        Vector3 playerForward = c_playerPosData.v_modelDirection;
        Vector3 spinAxis = Vector3.up;
        Vector3 flipAxis = Vector3.right;

        float currentSpinRate = c_trickPhys.f_currentSpinRate;
        float currentFlipRate = c_trickPhys.f_currentFlipRate;

        float currentSpinDegrees = currentSpinRate * 360f;
        float currentFlipDegrees = currentFlipRate * 360f;

        cart_rotation.ValidateSpinRotation(spinAmount, flipAmount, spinCeiling, flipCeiling, ref spinRate, ref flipRate);
        cart_rotation.Turn(ref playerForward, spinAxis, ref currentSpinDegrees, ref currentOrientation);
        cart_rotation.Turn(ref playerForward, flipAxis, ref currentFlipDegrees, ref currentOrientation);

        c_playerPosData.q_currentModelRotation = currentOrientation;
        c_playerPosData.v_modelDirection = playerForward;
        c_trickPhys.f_currentSpinRate = spinRate;
        c_trickPhys.f_currentFlipRate = flipRate;
        c_scoringData.f_currentSpinDegrees += currentSpinDegrees * Time.deltaTime;
        c_scoringData.f_currentFlipDegrees += currentFlipDegrees * Time.deltaTime;
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
        if (c_trickPhys.f_currentSpinRate > 0)
        {
            c_trickPhys.f_currentSpinRate = c_trickPhys.f_resetRate;
            c_scoringData.f_currentSpinTarget = Mathf.Ceil(c_scoringData.f_currentSpinDegrees / 180) * 180;
        }
        else if (c_trickPhys.f_currentSpinRate < 0)
        {
            c_trickPhys.f_currentSpinRate = c_trickPhys.f_resetRate * -1;
            c_scoringData.f_currentSpinTarget = Mathf.Floor(c_scoringData.f_currentSpinDegrees / 180) * 180;
        }

        if (c_trickPhys.f_currentFlipRate > 0)
        {
            c_trickPhys.f_currentFlipRate = c_trickPhys.f_resetRate;
            c_scoringData.f_currentFlipTarget = Mathf.Ceil(c_scoringData.f_currentFlipDegrees / 360) * 360;
        }
        else if (c_trickPhys.f_currentFlipRate < 0)
        {
            c_trickPhys.f_currentFlipRate = c_trickPhys.f_resetRate * -1;
            c_scoringData.f_currentFlipTarget = Mathf.Floor(c_scoringData.f_currentFlipDegrees / 360) * 360;
        }

    }
}
