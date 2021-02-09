using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraMotionCartridge
{
    public static void HorizontalFollow(ref Vector3 cameraPosition, Vector3 targetTranslation, Vector3 targetPosition,
                                        ref Quaternion cameraRotation, float desiredDistance)
    {
        Vector3 finalOffset = targetPosition - targetTranslation - targetPosition;
        float rotationAmount = Vector3.SignedAngle(targetPosition - cameraPosition, finalOffset, cameraRotation * Vector3.up);
        float translationAmount = (targetPosition - cameraPosition).magnitude;

        cameraRotation *= Quaternion.AngleAxis(rotationAmount, cameraRotation * Vector3.up);
        cameraPosition += cameraRotation * Vector3.forward * (translationAmount - desiredDistance);
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
        cameraRotation *= Quaternion.AngleAxis(targetAngle, Vector3.right);
    }

    // Camera needs to keep focus on player, otherwise translation accelerates out of control
    public static void FocusOnPlayer()
    {

    }
}
