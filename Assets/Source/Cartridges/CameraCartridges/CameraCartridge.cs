using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraCartridge
{
    public static void AccelerateVerticalVelocity(
        ref float verticalVelocity,
        float maxVelocity,
        float maxAngle,
        float minAngle,
        Vector3 axis,
        Vector3 cameraPosition,
        Vector3 targetPosition)
    {
        Vector3 projectedAngle = targetPosition - cameraPosition;
        float currentAngle = Vector3.SignedAngle(projectedAngle, Vector3.ProjectOnPlane(projectedAngle, Vector3.up), axis);

        verticalVelocity = Utils.InterpolateFloat(Utils.GetFloatRatio(currentAngle, maxAngle, minAngle),
                                                   maxVelocity * Constants.NEGATIVE_ONE,
                                                   maxVelocity);
    }

    public static void AccelerateRotation(
        out float rotVelocity,
        float maxVelocity,
        Quaternion cameraRotation,
        Vector3 cameraPosition,
        Vector3 targetPosition,
        Vector3 targetOffset,
        Vector3 targetDelta,
        Vector3 axis)
    {
        Quaternion intendedOffset = Quaternion.LookRotation(targetDelta, Vector3.up);

        Vector3 targetVector = targetPosition - cameraPosition;
        float diff = Vector3.SignedAngle(Vector3.ProjectOnPlane(cameraRotation * Vector3.forward, axis), Vector3.ProjectOnPlane(targetVector, axis), axis);

        rotVelocity = Utils.InterpolateFloat(Utils.GetFloatRatio(diff, 0, 180f * Mathf.Sign(diff)),
            0,
            maxVelocity * Mathf.Sign(diff));

    }

    public static void RotateCamera(
        ref Quaternion cameraRotation,
        Vector3 axis,
        float velocity)
    {
        cameraRotation = Quaternion.AngleAxis(velocity * Time.deltaTime, axis) * cameraRotation;
    }

    public static void UpdateRotation(
        ref Quaternion cameraRotation,
        float rotationalVelocity,
        Vector3 axis)
    {
        Quaternion resultRotation = Quaternion.AngleAxis(rotationalVelocity, axis);
        cameraRotation *= resultRotation;
    }

    public static void AccelerateHorizontalVelocity(
        ref float horizontalVelocity,
        float maxVelocity,
        float maxDist,
        float minDist,
        Vector3 cameraPosition,
        Vector3 targetPosition)
    {
        float currentDist = Vector3.Distance(Vector3.ProjectOnPlane(cameraPosition, Vector3.up),
                                             Vector3.ProjectOnPlane(targetPosition, Vector3.up));

        horizontalVelocity = Utils.InterpolateFloat(Utils.GetFloatRatio(currentDist, minDist, maxDist),
                                                    Constants.ZERO_F,
                                                    maxVelocity);
    }
}
