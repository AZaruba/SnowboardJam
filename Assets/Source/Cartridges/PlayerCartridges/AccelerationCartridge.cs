﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationCartridge {

    #region PublicFunctions
    public void Accelerate(ref float velocity, ref float f_acceleration, float topSpeed, float surfaceFactor = 1.0f)
    {
        if (velocity > topSpeed)
        {
            velocity = topSpeed;
            return;
        }
        velocity += f_acceleration * surfaceFactor * Time.deltaTime;
    }

    public void Decelerate(ref float velocity, float deceleration, float surfaceFactor = 1.0f)
    {
        if (velocity < 0)
        {
            velocity = 0.0f;
            return;
        }
        velocity -= deceleration * surfaceFactor * Time.deltaTime;
    }
    #endregion
}
