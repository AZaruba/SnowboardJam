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

    /* The goal of KeepAboveSurface2 is to:
     * 1) find whether the player is above or below the surface at any point
     * 2) check to see how far away they are
     * 3) find some translation applied to the current position that remedies the clipping/floating
     * 
     */ 
    public static void KeepAboveSurface2(ref Vector3 currentPosition,
                                         Vector3 surfaceNormal,
                                         Vector3 noseOffset,
                                         Vector3 tailOffset,
                                         Vector3 nosePoint,
                                         Vector3 tailPoint,
                                         Quaternion inverseRotation)
    {
        // the result should be two vectors that ONLY have a y component
        float translationMagnitude = Constants.ZERO_F;

        Vector3 resultTailVec = ((inverseRotation * tailOffset) - (inverseRotation * tailPoint));
        Vector3 resultNoseVec = ((inverseRotation * noseOffset) - (inverseRotation * nosePoint));

        float resultTail = ((inverseRotation * tailOffset) - (inverseRotation * tailPoint)).y;
        float resultNose = ((inverseRotation * noseOffset) - (inverseRotation * nosePoint)).y;

        // 1) move up the most
        if (resultTail > 0.0f)
        {
            translationMagnitude = resultNose > resultTail ? resultNose : resultTail;
        }
        // 2) move up if needed
        else
        {
            if (resultNose < 0.0f)
            {
                translationMagnitude = resultNose;
            }
            // 3) move down the least
            else
            {
                translationMagnitude = resultTail < resultNose ? resultNose : resultTail;
            }
        }

        currentPosition += surfaceNormal.normalized * (translationMagnitude * -1);
    }

    public static void KeepAboveSurface(ref Vector3 currentPosition,
                                         Vector3 surfaceNormal,
                                         Vector3 noseOffset,
                                         Vector3 tailOffset,
                                         Vector3 nosePoint,
                                         Vector3 tailPoint,
                                         Quaternion inverseRotation)
    {
        Vector3 totalTranslation = Vector3.ProjectOnPlane(tailOffset - tailPoint, surfaceNormal) + tailPoint - tailOffset;

        currentPosition += totalTranslation + (surfaceNormal.normalized * 0.1f);
    }

    public void SwitchReverse(ref bool isReverse, Quaternion travelRotation, Quaternion modelRotation)
    {
        isReverse = Mathf.Abs(Quaternion.Angle(travelRotation, modelRotation)) > 90f;
    }
}
