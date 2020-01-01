using System;
public class CrashedState
{
    IncrementCartridge cart_timer;
    
    public CrashedState(ref IncrementCartridge timer)
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
