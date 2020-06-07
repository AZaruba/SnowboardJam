using System.Collections;
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
    void PullDirectionVector(ref Vector3 currentDir, Vector3 surfaceNormal, Vector3 up, float influenceValue, float currentSpeed)
    {

    }

    /// <summary>
    /// Adjusts a float based on an angle of a surface relative to up. The use case for this function is increasing/decreasing
    /// speed and acceleration based on travel relative to a normal.
    /// </summary>
    /// <param name="speedValue">The current top speed</param>
    /// <param name="surfaceNormal">The normal of the surface influencing the value</param>
    /// <param name="up">The relative up vector. As surfaceNormal approaches up, the effect will diminish.</param>
    /// <param name="currentDir">The current direction of travel. Used to determine whether to increase or decrease speed</param>
    void PullVelocity(ref float speedValue, Vector3 surfaceNormal, Vector3 up, Vector3 currentDir)
    {
        // verify if surfaceNormal and currentDir are on the same side of up. Increase if they are the same, decrease if they are different
    }
}
