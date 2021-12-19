using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GravityCartridge {

    public static void UpdateAirVelocity(ref float currentAirVelocity, float globalGravity, float terminalVelocity)
    {
        if (currentAirVelocity < terminalVelocity * -1)
        {
            currentAirVelocity = terminalVelocity * -1;
            return;
        }
        currentAirVelocity -= globalGravity * Time.deltaTime;
    }

    public static void Jump(ref float currentAirVelocity, ref float jumpPower)
    {
        currentAirVelocity += jumpPower;
    }
}
