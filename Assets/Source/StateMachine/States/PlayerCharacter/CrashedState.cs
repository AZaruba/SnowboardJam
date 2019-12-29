using System;
public class CrashedState
{
    TimerCartridge cart_timer;
    
    public CrashedState(ref TimerCartridge timer)
    {
        this.cart_timer = timer;
    }

    public void Act()
    {

    }

    public void TransitionAct()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.REST)
        {
            // return to ready
            return StateRef.STATIONARY;
        }
        return StateRef.CRASHED;
    }
}
