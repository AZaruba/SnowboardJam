using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleCalculationCartridge
{
    public static void AlignToSurfaceByTail(Vector3 surfaceNormal,
                                ref Quaternion currentRotation,
                                ref Vector3 currentNormal,
                                int switchStance)
    {
        Vector3 targetForward = Vector3.ProjectOnPlane(currentRotation * Vector3.forward.normalized * switchStance, surfaceNormal.normalized).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetForward, surfaceNormal).normalized;
        currentRotation = targetRotation;
        currentNormal = (targetRotation * Vector3.up).normalized;
        return;
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
