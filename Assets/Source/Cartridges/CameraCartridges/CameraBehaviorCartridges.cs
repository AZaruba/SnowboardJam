using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraOrientationCartridge
{
    public static void AccelerateRotationalVelocity()
    {

    }

    public static void CalculateVerticalRotation()
    {

    }

    /* NOTE:
     * This behaves incorrectly if we don't take vertical rotation into account, leading to "roll"
     * This could be due to how we unfold and refofld
     */
    public static void CalculateHorizontalRotation(out Quaternion rotationOut, Quaternion currentRotation,
                                                   Quaternion playerRotation, Vector3 playerOffset)
    {
        Vector3 planeDir = playerRotation * Vector3.up;
        Vector3 playerDir = Vector3.ProjectOnPlane(playerOffset, planeDir).normalized;
        Vector3 flatCameraDir = Vector3.ProjectOnPlane(currentRotation * Vector3.forward, planeDir).normalized;

        rotationOut = Quaternion.LookRotation(playerDir, planeDir) * Quaternion.Inverse(Quaternion.LookRotation(flatCameraDir, planeDir)).normalized;
    }

    public static void CalculateHorizontalDistance(out float distance, Vector3 camPosition, Vector3 targetPosition, Quaternion rotation)
    {
        camPosition = Quaternion.Inverse(rotation) * camPosition;
        targetPosition = Quaternion.Inverse(rotation) * targetPosition;
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
    public static void TranslateHorizontalPosition(ref Vector3 position, Quaternion direction, float velocity)
    {
        position += direction * Vector3.forward * velocity * Time.deltaTime;
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
