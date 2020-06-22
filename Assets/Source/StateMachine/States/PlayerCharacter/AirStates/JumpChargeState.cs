using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpChargeState : iState
{
    private PlayerData c_playerData;
    private IncrementCartridge cart_increment;
    
    public JumpChargeState(ref PlayerData playerData, ref IncrementCartridge incr)
    {
        this.c_playerData = playerData;
        this.cart_increment = incr;
    }

    public void Act()
    {
        float chargeCap = c_playerData.f_jumpPower;
        float chargeValue = c_playerData.f_currentJumpCharge;
        float chargeDelta = c_playerData.f_jumpChargeRate;

        cart_increment.Increment(ref chargeValue, chargeDelta * Time.deltaTime, chargeCap);

        c_playerData.f_currentJumpCharge = chargeValue;
    }

    public void TransitionAct()
    {
        c_playerData.f_currentJumpCharge = c_playerData.f_baseJumpPower;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.FALL)
        {
            c_playerData.f_currentJumpCharge = 0.0f;
            return StateRef.AIRBORNE;
        }
        if (cmd == Command.JUMP)
        {
            return StateRef.AIRBORNE;
        }
        if (cmd == Command.CRASH)
        {
            return StateRef.DISABLED;
        }
        return StateRef.CHARGING;
    }
}

