using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraOrientationCartridge
{
    public static void AccelerateVerticalVelocity(ref float velocity, float velocityCap, float targetAngle, float maxAngle, float currentAngle)
    {
        velocity = (currentAngle - targetAngle) / (maxAngle - targetAngle) * velocityCap;
    }

    public static void CalculateVerticalDifference(out float currentAngle, Vector3 playerOffset, Quaternion targetRotationIn)
    {
        /* The target has a rotation, multiply that rotation by Vector3.forward to get where the player is going.
         * That, rotated along the right axis ten degrees, is what our desired angle should be
         */
        Vector3 planeDir = targetRotationIn * Vector3.right;
        Vector3 playerDir = Vector3.ProjectOnPlane(playerOffset, planeDir).normalized;
        Vector3 flatPlayerDir = Vector3.ProjectOnPlane(playerDir, Vector3.up).normalized;

        currentAngle = Vector3.SignedAngle(flatPlayerDir, playerDir, planeDir);
    }

    public static void CalculateVerticalRotation(out float angleBetween, Quaternion rotationIn, Vector3 playerOffset)
    {
        Vector3 planeDir = rotationIn * Vector3.right;
        Vector3 playerDir = Vector3.ProjectOnPlane(playerOffset, planeDir).normalized;
        Vector3 flatCameraDir = Vector3.ProjectOnPlane(rotationIn * Vector3.forward, planeDir).normalized;

        angleBetween = Vector3.SignedAngle(flatCameraDir, playerDir, planeDir);
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

    public static void CalculateHorizontalDistance(out float distance, Vector3 playerOffset, Quaternion targetRotationIn)
    {
        Vector3 planeDir = targetRotationIn * Vector3.up;
        Vector3 playerDir = Vector3.ProjectOnPlane(playerOffset, planeDir);

        distance = playerDir.magnitude;
    }

    // slowing down in the air?
    public static void AccelerateTranslationalVelocity(ref float velocityIn, float currentDist, float maxDist, float minDist, float velocityCap)
    {
        float absVelocity = (currentDist - minDist) / (maxDist - minDist) * velocityCap;
        float capDir = absVelocity > Constants.ZERO_F ? 1 : -1;
        velocityIn = Mathf.Abs(absVelocity) > velocityCap ? velocityCap * capDir : absVelocity;
    }

    public static void TranslateHorizontalPosition(ref Vector3 position, Vector3 direction, float velocity)
    {
        direction = Vector3.ProjectOnPlane(direction, Vector3.up);
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
