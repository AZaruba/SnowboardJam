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
        Vector3 targetPosition,
        Vector3 targetOffset,
        Vector3 targetDelta)
    {
        Quaternion intendedOffset = Quaternion.LookRotation(targetDelta, Vector3.up);

        Vector3 projectedAngle = targetPosition - cameraPosition; // + intendedOffset * targetOffset - cameraPosition;
        float currentAngle = Vector3.SignedAngle(projectedAngle, Vector3.ProjectOnPlane(projectedAngle, Vector3.up), axis);

        // TODO: unintuitive min/maxing here, figure out a way to make this more readable
        verticalVelocity = Utils.InterpolateFloat(Utils.GetFloatRatio(currentAngle, maxAngle, minAngle),
                                                   maxVelocity * Constants.NEGATIVE_ONE,
                                                   maxVelocity);
    }

    public static void UpdateVerticalRotation(
        ref Quaternion cameraRotation,
        float rotationalVelocity,
        Vector3 axis)
    {
        Quaternion resultRotation = Quaternion.AngleAxis(rotationalVelocity, axis);
        cameraRotation *= resultRotation;
    }
}
