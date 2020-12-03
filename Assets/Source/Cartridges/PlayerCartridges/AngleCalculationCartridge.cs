using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleCalculationCartridge
{
    /// <summary>
    /// Aligns the rotation with surface.
    /// </summary>
    /// <param name="targetNormal">Target normal.</param>
    /// <param name="currentNormal">Current normal.</param>
    /// <param name="currentForward">Current forward.</param>
    public void AlignRotationWithSurface(ref Vector3 targetNormal, ref Vector3 currentNormal, ref Vector3 currentForward, ref Quaternion resultRotation, float rotationAngle)
    {
        if (Mathf.Abs(rotationAngle) < 0.05f)
        {
            return;
        }
        Vector3 targetForward = Vector3.ProjectOnPlane(currentForward, targetNormal);
        Quaternion normalRotation = Quaternion.LookRotation(targetForward, targetNormal);

        resultRotation = Quaternion.RotateTowards(resultRotation, normalRotation, Mathf.Abs(rotationAngle)); // Quaternion.AngleAxis(rotationAngle, axis.normalized);
        currentForward = resultRotation * Vector3.forward;
        currentNormal = resultRotation * Vector3.up;
    }

    public static void AlignToSurfaceByTail(ref Vector3 currentPosition,
                                Vector3 surfaceNormal,
                                ref Quaternion currentRotation,
                                ref Vector3 currentForward,
                                ref Vector3 currentNormal)
    {
        Vector3 targetForward = Vector3.ProjectOnPlane(currentForward.normalized, surfaceNormal.normalized).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetForward, surfaceNormal).normalized;
        currentRotation = targetRotation;
        currentNormal = (targetRotation * Vector3.up).normalized;
        currentForward = (targetRotation * Vector3.forward).normalized;
        return;
    }

    public void AlignToSurface2(ref Vector3 currentForward,
                                ref Vector3 currentNormal, 
                                ref Quaternion resultRotation,
                                Quaternion targetRotation)
    {
        resultRotation = resultRotation * targetRotation;

        currentNormal = resultRotation * Vector3.up;
        currentForward = resultRotation * Vector3.forward;
    }

    public void AlignOrientationWithSurface(ref Vector3 currentNormal, ref Vector3 currentForward, ref Quaternion resultRotation, Vector3 targetNormal)
    {
        Vector3 targetForward = Vector3.ProjectOnPlane(currentForward, targetNormal);
        Quaternion normalRotation = Quaternion.LookRotation(targetForward, targetNormal);

        resultRotation = normalRotation;
        currentForward = targetForward.normalized;
        currentNormal = targetNormal;
    }

    /// <summary>
    /// Zeroes out a rotation, useful for when want to update a rotation buffer.
    /// </summary>
    /// <param name="rotation">The rotation to be zeroed. Should be a rotation buffer.</param>
    public void ZeroRotation(ref Quaternion rotation)
    {
        rotation = Quaternion.identity;
    }
}
