using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraMotionCartridge
{
    /* Known issues:
     * jerky movement is likely caused by the way we change where we look based on a change in velocity/position delta
     * Adding some kind of acceleration smoothing should help with the problem, double checking how we handle 
     * change in direction/velocity should finish it.
     * 
     */ 
    public static void HorizontalAccelerate()
    {

    }

    public static void HorizontalFollow(ref Vector3 cameraPosition, Vector3 targetTranslation, Vector3 targetPosition,
                                        Quaternion targetRotation, Quaternion cameraRotation, float desiredDistance)
    {
        Vector3 projectionNormal = cameraRotation * Vector3.up;
        float translationAmount = (Vector3.ProjectOnPlane(targetPosition, projectionNormal) - Vector3.ProjectOnPlane(cameraPosition, projectionNormal)).magnitude;

        // follow should have weight to it, as just translating causes jerky movement overall
        cameraPosition -= cameraRotation * Vector3.forward * (desiredDistance - translationAmount);
    }

    public static void VerticalAccelerate()
    {

    }

    /// <summary>
    /// Defines behavior for rotating and translating the camera to keep a certain height relative to the player.
    /// Generally overridden by keeping the camera at a certain height above the ground.
    /// </summary>
    /// <param name="cameraPosition">The camera's position to be updated.</param>
    /// <param name="targetTranslation">The translation of the target.</param>
    /// <param name="cameraRotation">The camera's current orientation.</param>
    /// <param name="targetPosition">The target's position before translation.</param>
    /// <param name="angleConstraint">The maximum angle allowed before raw translation kicks in</param>
    public static void VerticalFollow(ref Vector3 cameraPosition, Vector3 targetTranslation,
                                      ref Quaternion cameraRotation, Vector3 targetPosition,
                                      float angleConstraint)
    {
        Vector3 currentDir = cameraRotation * Vector3.forward;
        float currentAngle = Vector3.SignedAngle(Vector3.ProjectOnPlane(currentDir, Vector3.up).normalized,
                                                 currentDir, 
                                                 cameraRotation * Vector3.right);

        // position doesn't move if the angle is valid, position moves the required y value otherwise.
        float targetAngle = Mathf.Abs(currentAngle) >= angleConstraint ? Constants.ZERO_F : Vector3.SignedAngle(targetPosition, 
                                                                                                     targetPosition - targetTranslation, 
                                                                                                     cameraRotation * Vector3.right);

        cameraPosition += currentAngle >= angleConstraint ? Vector3.zero : Vector3.up * targetTranslation.y;
    }

    // Camera needs to keep focus on player, otherwise translation accelerates out of control
    public static void FocusOnPlayer(ref Quaternion cameraRotation, Quaternion targetDir, Vector3 cameraPosition, Vector3 targetPosition,
                                     Vector3 targetTranslation, float forwardOffset, float upwardOffset)
    {
        Vector3 totalOffset = targetTranslation.normalized * forwardOffset + targetDir * Vector3.up.normalized * upwardOffset;
        Vector3 pointAt = targetPosition - cameraPosition + totalOffset;

        cameraRotation = Quaternion.LookRotation(pointAt);
    }

    public static void KeepAboveGround(ref Vector3 cameraPosition, Quaternion cameraRotation, float targetDistance, float currentDistance)
    {
        cameraPosition += cameraRotation * Vector3.up * (targetDistance - currentDistance);
    }
}
