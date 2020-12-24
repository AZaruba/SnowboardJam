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

    /// <summary>
    /// Moves a position, in a given rotation, along the normal a certain distance. Used
    /// to ensure that the player is above the ground at all times.
    /// </summary>
    /// <param name="currentPosition">The current position to be translated</param>
    /// <param name="currentRotation">The rotation identifying the normal</param>
    /// <param name="contactOffset">The distance to move along the normal</param>
    public static void KeepAboveSurface(ref Vector3 currentPosition,
                                         Quaternion currentRotation,
                                         float contactOffset)
    {
        currentPosition += currentRotation * Vector3.up * contactOffset;
    }

    /// <summary>
    /// Scales acceleration based on the current orientation. The y component of the orientation * forward
    /// vector provides a ratio of how far up or down the player is facing.
    /// </summary>
    /// <param name="acceleration">Current acceleration value.</param>
    /// <param name="defaultAcceleration">Default acceleration value.</param>
    /// <param name="currentRotation">The current player orientation, multiplied by Vector3.forward</param>
    public static void AdjustAcceleration(ref float acceleration,
                                          float defaultAcceleration,
                                          float gravityValue,
                                          Quaternion currentRotation)
    {
        acceleration = defaultAcceleration - (currentRotation * Vector3.forward).y * gravityValue;
    }

    /// <summary>
    /// Adjusts the player's top speed according to the surface incline. Flat surfaces will yield 100%, scaling up to 150%
    /// </summary>
    /// <param name="topSpeed">The current top speed</param>
    /// <param name="currentRotation">The current rotation, relative to the identity quaternion</param>
    public static void AdjustTopSpeed(ref float topSpeed,
                                      Quaternion currentRotation)
    {
        // BUG: top speed just keeps going up!
        float referenceRatio = Quaternion.Angle(Quaternion.identity, currentRotation)/Constants.PERPENDICULAR_F;

        topSpeed *= referenceRatio;
    }

    public void SwitchReverse(ref bool isReverse, Quaternion travelRotation, Quaternion modelRotation)
    {
        isReverse = Mathf.Abs(Quaternion.Angle(travelRotation, modelRotation)) > 90f;
    }
}
