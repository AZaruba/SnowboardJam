using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinChargeState : iState
{
    private TrickPhysicsData c_trickPhys;
    private PlayerInputData c_playerInput;

    public SpinChargeState(ref TrickPhysicsData dataIn, ref PlayerInputData inputIn)
    {
        this.c_trickPhys = dataIn;
        this.c_playerInput = inputIn;
    }

    public void Act()
    {
        float currentSpinCharge = c_trickPhys.f_currentSpinCharge;
        float currentFlipCharge = c_trickPhys.f_currentFlipCharge;

        float flipChargeRate = c_trickPhys.f_flipIncrement * c_playerInput.f_inputAxisLVert;
        float spinChargeRate = c_trickPhys.f_spinIncrement * c_playerInput.f_inputAxisLHoriz;

        IncrementCartridge.IncrementTethered(ref currentSpinCharge, spinChargeRate * Time.fixedDeltaTime, c_trickPhys.f_maxSpinRate *-1, c_trickPhys.f_maxSpinRate);
        IncrementCartridge.IncrementTethered(ref currentFlipCharge, flipChargeRate * Time.fixedDeltaTime, c_trickPhys.f_maxFlipRate *-1, c_trickPhys.f_maxFlipRate);

        c_trickPhys.f_currentFlipCharge = currentFlipCharge;
        c_trickPhys.f_currentSpinCharge = currentSpinCharge;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.JUMP)
        {
            return StateRef.SPINNING;
        }
        if (cmd == Command.START_BOOST)
        {
            c_trickPhys.f_currentFlipCharge = Constants.ZERO_F;
            c_trickPhys.f_currentSpinCharge = Constants.ZERO_F;
            return StateRef.SPIN_IDLE;
        }
        if (cmd == Command.CRASH)
        {
            return StateRef.SPIN_IDLE;
        }
        return StateRef.SPIN_CHARGE;
    }

    // TODO: Play with whether or not we want to immediately add some value or require letting go of the stick to charge
    public void TransitionAct()
    {
        c_trickPhys.f_currentFlipCharge = c_trickPhys.f_minFlipRate * c_playerInput.f_inputAxisLVert;
        c_trickPhys.f_currentSpinCharge = c_trickPhys.f_minSpinRate * c_playerInput.f_inputAxisLHoriz;
    }
}
