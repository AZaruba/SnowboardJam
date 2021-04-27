using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraOrientationCartridge
{
    public static void AccelerateRotationalVelocity()
    {

    }

    public static void CalculateVerticalRotation(out float angleBetween, Quaternion rotationIn)
    {
        Vector3 projectedDir = Vector3.ProjectOnPlane(rotationIn * Vector3.forward, Vector3.up).normalized;
        angleBetween = Vector3.SignedAngle(rotationIn * Vector3.forward, projectedDir, rotationIn * Vector3.right);
    }

    /* The vertical motion of the camera might be bidirectional?
     * 
     */ 
    public static void CalculateVerticalDistance(out float distance, float camY, float targetY)
    {
        distance = targetY - camY;
    }

    public static void CalculateVerticalVelocity(ref float velocityIn, float currentAngle, float maxAngle, float desiredAngle, float targetVelocity, float capRatio = 1.1f)
    {
        float absVelocity = (currentAngle - desiredAngle) / (maxAngle - desiredAngle) * targetVelocity;
        velocityIn = Mathf.Abs(absVelocity) > Mathf.Abs(targetVelocity * capRatio) ? targetVelocity * capRatio : absVelocity;
    }

    public static void TranslateVerticalPosition(ref Vector3 position, float velocity)
    {
        position += Vector3.down * velocity * Time.deltaTime;
    }

    /* 
     * Gimbal Lock on going straight up, need to refactor into quaternions
     */
    public static void CalculateHorizontalRotation(out Quaternion rotationOut, Quaternion currentRotation,
                                                   Quaternion playerRotation, Vector3 playerOffset)
    {
        Vector3 planeDir = Vector3.up;
        Vector3 playerDir = Vector3.ProjectOnPlane(playerOffset, planeDir).normalized;
        Vector3 flatCameraDir = Vector3.ProjectOnPlane(currentRotation * Vector3.forward, planeDir).normalized;

        rotationOut = Quaternion.LookRotation(playerDir, planeDir) * Quaternion.Inverse(Quaternion.LookRotation(flatCameraDir, planeDir)).normalized;
    }

    public static void CalculateHorizontalDistance(out float distance, Vector3 camPosition, Vector3 targetPosition, Quaternion rotation)
    {
        camPosition.y = 0;
        targetPosition.y = 0;
        distance = Vector3.Distance(camPosition, targetPosition);
    }

    public static void AccelerateTranslationalVelocity(ref float velocityIn, float currentDist, float maxDist, float minDist, float targetVelocity, float capRatio = 1.1f)
    {
        velocityIn = Mathf.Min((currentDist - minDist) / (maxDist - minDist) * targetVelocity, targetVelocity * capRatio);
    }

    /* NOTE:
     * This causes out-of-control acceleration if the camera is not reorienting correctly, could this pose issues later?
     * This needs to be flattened like the rotation calculation
     */ 
    public static void TranslateHorizontalPosition(ref Vector3 position, Vector3 direction, float velocity)
    {
        direction.y = 0;
        Quaternion flatRotation = Quaternion.LookRotation(direction, Vector3.up);
        position += flatRotation * Vector3.forward * velocity * Time.deltaTime;
    }

    public static void RotateAboutAxis(ref Quaternion rotation, Quaternion desiredRotation)
    {
        rotation *= desiredRotation;
    }

    public static void ApplyRotation(ref Quaternion currentRotation, Quaternion flatRotation)
    {
        currentRotation = flatRotation * currentRotation;
    }
}
