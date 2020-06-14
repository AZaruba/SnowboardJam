﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceInfluenceCartridge
{
    /// <summary>
    /// This function will rotate a direction Vector3 based on the angle of a surface normal relative to an upward vector.
    /// The use case for this function is rotating the direction of a moving object downhill on curved slopes.
    /// </summary>
    /// <param name="currentDir">The current direction of travel</param>
    /// <param name="surfaceNormal">The normal of the surface influencing currentDir</param>
    /// <param name="up">The relative up vector. As surfaceNormal approaches up, the effect will diminish</param>
    /// <param name="influenceValue">The magnitude of the rotation. Not explicitly an angle.</param>
    /// <param name="currentSpeed">The velocity of the influenced object. As speed increases, the influence decreases</param>
    public void PullDirectionVector(ref Vector3 currentDir, Vector3 surfaceNormal, Vector3 up, float influenceValue, ref float currentSpeed, float currentBrake = 0.0f)
    {
        Vector3 scaledDirection = currentDir * currentSpeed;

        Vector3 scaledPull = (surfaceNormal - up) * (30 - currentBrake) * Time.deltaTime; // TODO: Constant removal

        scaledDirection += scaledPull;
         
        currentDir = scaledDirection.normalized;
        currentSpeed = scaledDirection.magnitude;
    }

    /// <summary>
    /// Adjusts a float based on an angle of a surface relative to up. The use case for this function is increasing/decreasing
    /// speed and acceleration based on travel relative to a normal.
    /// </summary>
    /// <param name="speedValue">The current top speed</param>
    /// <param name="surfaceNormal">The normal of the surface influencing the value</param>
    /// <param name="up">The relative up vector. As surfaceNormal approaches up, the effect will diminish.</param>
    /// <param name="currentDir">The current direction of travel. Used to determine whether to increase or decrease speed</param>
    public void PullVelocity(ref float speedValue, Vector3 surfaceNormal, Vector3 up, Vector3 currentDir)
    {
        Vector3 scaledDirection = currentDir * speedValue;

        float magnitude = Vector3.Angle(up, currentDir) - 90;

        // TODO: find an adequate offset for acceleration to cap downward velocity while ensuring upward velocity eventually hits zero
        speedValue += (magnitude - 20) * Time.deltaTime;
    }
}
