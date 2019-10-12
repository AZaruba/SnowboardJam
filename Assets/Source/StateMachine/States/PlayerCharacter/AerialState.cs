using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialState : iPlayerState {

    private GravityCartridge cart_gravity;

    public AerialState(ref GravityCartridge cart_grav)
    {
        cart_gravity = cart_grav;
    }

    public void Act(ref PlayerData c_playerData)
    {
        float airVelocity = c_playerData.CurrentAirVelocity;
        float gravity = c_playerData.Gravity;
        Vector3 position = c_playerData.CurrentPosition;

        cart_gravity.UpdateAirVelocity(ref airVelocity, ref gravity);
        position.y += airVelocity;

        c_playerData.CurrentPosition = position;
        c_playerData.CurrentSpeed = airVelocity;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.LAND)
        {
            return StateRef.STATIONARY;
        }
        return StateRef.AIRBORNE;
    }


}
