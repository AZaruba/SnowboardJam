using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationCartridge {

    #region PublicFunctions
    public void Accelerate(ref float velocity, ref float acceleration, float surfaceFactor = 1.0f)
    {
        velocity += acceleration * surfaceFactor;
    }

    public void Decelerate(ref float velocity, ref float deceleration, float surfaceFactor = 1.0f)
    {
        velocity -= deceleration * surfaceFactor;
    }
    #endregion
}
