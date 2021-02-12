using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraMotionCartridge
{
    public static void HorizontalFollow(ref Vector3 cameraPosition, Vector3 targetTranslation, Vector3 targetPosition,
                                        Quaternion targetRotation, Quaternion cameraRotation, float desiredDistance)
    {
        Vector3 projectionNormal = cameraRotation * Vector3.up;
        float translationAmount = (Vector3.ProjectOnPlane(targetPosition, projectionNormal) - Vector3.ProjectOnPlane(cameraPosition, projectionNormal)).magnitude;

        // cameraRotation *= Quaternion.AngleAxis(rotationAmount, cameraRotation * Vector3.up);
        cameraPosition -= cameraRotation * Vector3.forward * (desiredDistance - translationAmount);
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
        // cameraRotation *= Quaternion.AngleAxis(targetAngle, Vector3.right); Camera rotation is unncessary as focusing on thep layer handles itS
    }

    // Camera needs to keep focus on player, otherwise translation accelerates out of control
    public static void FocusOnPlayer(ref Quaternion cameraRotation, Vector3 cameraPosition, Vector3 targetPosition,
                                     Vector3 targetTranslation, float forwardOffset)
    {
        Vector3 totalOffset = targetTranslation.normalized * forwardOffset;
        Vector3 pointAt = targetPosition - cameraPosition + totalOffset;

        cameraRotation = Quaternion.LookRotation(pointAt);
    }

    public static void KeepAboveGround(ref Vector3 cameraPosition, Quaternion cameraRotation, float targetDistance, float currentDistance)
    {
        cameraPosition += cameraRotation * Vector3.up * (targetDistance - currentDistance);
    }
}
