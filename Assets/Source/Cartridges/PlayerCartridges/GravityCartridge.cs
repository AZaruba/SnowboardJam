using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCartridge {

    public void UpdateAirVelocity(ref float currentAirVelocity, ref float globalGravity)
    {
        currentAirVelocity -= globalGravity;
    }

    public void Jump(ref float currentAirVelocity, ref float jumpPower)
    {
        currentAirVelocity += jumpPower;
    }
}
