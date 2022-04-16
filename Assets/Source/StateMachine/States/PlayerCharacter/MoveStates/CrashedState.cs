using System;
using UnityEngine; 
public class CrashedState : iState
{
    PlayerData c_playerData;
    
    public CrashedState(ref PlayerData playerData)
    {
        this.c_playerData = playerData;
    }

    public void Act()
    {
        float crashTimer = c_playerData.f_currentCrashTimer;
        float crashLimit = c_playerData.f_crashRecoveryTime;

        IncrementCartridge.Increment(ref crashTimer, Time.deltaTime, crashLimit);

        c_playerData.f_currentCrashTimer = crashTimer;
    }

    public void TransitionAct()
    {
        c_playerData.f_currentCrashTimer = Constants.ZERO_F;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.READY)
        {
            // return to ready
            return StateRef.STATIONARY;
        }
        return StateRef.CRASHED;
    }
}
