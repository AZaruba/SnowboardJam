using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialState : iPlayerState {

    private GravityCartridge cart_gravity;

    public AerialState(ref GravityCartridge cart_grav)
    {
        cart_gravity = cart_grav;
    }

    // TODO: add horizontal movement that takes minimal external input here
    public void Act(ref PlayerData c_playerData)
    {
        float airVelocity = c_playerData.CurrentAirVelocity;
        float gravity = c_playerData.Gravity;
        float terminalVelocity = c_playerData.f_terminalVelocity;
        Vector3 position = c_playerData.CurrentPosition;

        cart_gravity.UpdateAirVelocity(ref airVelocity, ref gravity, ref terminalVelocity);
        position.y += airVelocity;

        c_playerData.CurrentPosition = position;
        c_playerData.CurrentAirVelocity = airVelocity;
    }

    public void TransitionAct(ref PlayerData c_playerData)
    {
        Debug.Log("AERIAL");
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.LAND)
        {
            return StateRef.RIDING;
        }
        return StateRef.AIRBORNE;
    }


}
