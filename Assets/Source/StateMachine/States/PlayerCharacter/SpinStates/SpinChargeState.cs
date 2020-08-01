using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinChargeState : iState
{
    private TrickPhysicsData c_trickPhys;
    private IncrementCartridge cart_incr;

    public SpinChargeState(ref TrickPhysicsData dataIn, ref IncrementCartridge incrIn)
    {
        this.c_trickPhys = dataIn;
        this.cart_incr = incrIn;
    }

    public void Act()
    {
        float currentSpinCharge = c_trickPhys.f_currentSpinCharge;
        float currentFlipCharge = c_trickPhys.f_currentFlipCharge;

        float flipChargeRate = c_trickPhys.f_flipIncrement;
        float spinChargeRate = c_trickPhys.f_spinIncrement;

        cart_incr.Increment(ref currentSpinCharge, spinChargeRate * Time.deltaTime, c_trickPhys.f_maxSpinRate);
        cart_incr.Increment(ref currentFlipCharge, flipChargeRate * Time.deltaTime, c_trickPhys.f_maxFlipRate);

        c_trickPhys.f_currentFlipCharge = currentFlipCharge;
        c_trickPhys.f_currentSpinCharge = currentSpinCharge;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.JUMP)
        {
            return StateRef.SPINNING;
        }
        if (cmd == Command.CRASH)
        {
            return StateRef.SPIN_IDLE;
        }
        return StateRef.SPIN_CHARGE;
    }

    public void TransitionAct()
    {
        c_trickPhys.f_currentFlipCharge = c_trickPhys.f_minFlipRate;
        c_trickPhys.f_currentSpinCharge = c_trickPhys.f_minSpinRate;
    }
}
