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
    public void PullDirectionVector(ref Vector3 currentDir, Vector3 surfaceNormal, Vector3 up, float influenceValue, ref float currentSpeed, float currentBrake = 0.0f)
    {
        Vector3 scaledDirection = currentDir * currentSpeed;

        Vector3 scaledPull = (surfaceNormal - up) * (30 - currentBrake) * Time.deltaTime; // TODO: Constant removal

        scaledDirection += scaledPull;

        currentDir = scaledDirection.normalized;
        currentSpeed = scaledDirection.magnitude;
    }

    public static void KeepAboveSurface(ref Vector3 currentPosition, Quaternion currentRotation, Vector3 pointBelow, float desiredDistance)
    {
        if (pointBelow == Vector3.zero)
        {
            return;
        }

        Vector3 inverse = Quaternion.Inverse(currentRotation) * (currentPosition - pointBelow);
        float difference = inverse.y - desiredDistance;

        inverse.y -= difference;
        currentPosition = currentRotation * (inverse + Quaternion.Inverse(currentRotation) * pointBelow);
    }

    public void SwitchReverse(ref bool isReverse, Quaternion travelRotation, Quaternion modelRotation)
    {
        isReverse = Mathf.Abs(Quaternion.Angle(travelRotation, modelRotation)) > 90f;
    }
}
