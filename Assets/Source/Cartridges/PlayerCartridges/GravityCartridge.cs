using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCartridge {

    public void UpdateAirVelocity(ref float currentAirVelocity, ref float globalGravity, ref float terminalVelocity)
    {
        if (currentAirVelocity < terminalVelocity * -1)
        {
            currentAirVelocity = terminalVelocity * -1;
            return;
        }
        currentAirVelocity -= globalGravity;
    }

    public void Jump(ref float currentAirVelocity, ref float jumpPower)
    {
        currentAirVelocity += jumpPower;
    }
}
