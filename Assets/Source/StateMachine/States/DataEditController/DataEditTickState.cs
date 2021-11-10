using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntEditTickState : iState
{
    private EditControllerData c_ctrlData;
    private IncrementCartridge cart_incr;

    public IntEditTickState(ref EditControllerData editIn, ref IncrementCartridge incr)
    {
        this.c_ctrlData = editIn;
        this.cart_incr = incr;
    }

    public void Act()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.MENU_HIDE)
        {
            return StateRef.MENU_DISABLED;
        }
        if (cmd == Command.MENU_IDLE)
        {
            return StateRef.MENU_WAIT;
        }
        return StateRef.MENU_TICK;
    }

    public void TransitionAct()
    {
        int currentValue = c_ctrlData.i;

        if (c_ctrlData.i_increasing > 0)
        {
            cart_incr.Increment(ref currentValue, Constants.ONE, c_ctrlData.i_max);
        }
        else if (c_ctrlData.i_increasing < 0) // we should never reach this line with == 0 but just in case...
        {
            cart_incr.Decrement(ref currentValue, Constants.ONE, c_ctrlData.i_min);
        }

        c_ctrlData.i = currentValue;

        c_ctrlData.f_currentTickTime = Constants.ZERO_F;
    }
}
