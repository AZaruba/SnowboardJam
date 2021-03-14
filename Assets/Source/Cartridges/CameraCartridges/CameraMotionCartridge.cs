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

    /* The speed at which the camera follows the player should increase the further it's away from the player,
     * Where we match the player's velocity at max distance and hit zero at the min distance
     * 
     */ 
    public static void HorizontalAccelerate(ref float velocity, float max, float min, Vector3 cameraPosition, Vector3 targetPosition, float targetTranslationMagnitude)
    {
        float dist = Vector3.Distance(cameraPosition, targetPosition);

        targetTranslationMagnitude /= Time.deltaTime; // express magnitude by its velocity, not the timeDelta (as we do this later)

        velocity = ((dist - min) / (max - min)) * targetTranslationMagnitude; // translationMagnitude * 0 at min, translationMagnitude * 1 at max
    }

    public static void HorizontalTranslate(ref Vector3 cameraPosition, Quaternion cameraRotation, float velocity)
    {
        cameraPosition += cameraRotation * Vector3.forward * velocity * Time.deltaTime;
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
        Vector3 totalOffset = targetDir * Vector3.up.normalized * upwardOffset;
        Vector3 pointAt = targetPosition - cameraPosition + totalOffset;

        cameraRotation = Quaternion.LookRotation(pointAt);
    }

    public static void KeepAboveGround(ref Vector3 cameraPosition, Quaternion cameraRotation, float targetDistance, float currentDistance)
    {
        cameraPosition += cameraRotation * Vector3.up * (targetDistance - currentDistance);
    }

    // reorients the camera to preserve direction while rotating alongside the current surface
    // not sure if this is right because the surface might be very rotated
    public static void ReorientToSurface(ref Quaternion cameraRotation, Vector3 groundNormal)
    {
        Vector3 look = cameraRotation * Vector3.forward;

        cameraRotation = Quaternion.LookRotation(look, Vector3.ProjectOnPlane(groundNormal, look).normalized);
    }
}
