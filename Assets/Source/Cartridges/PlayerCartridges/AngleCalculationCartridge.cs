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

        Vector3 axis = Vector3.Cross(Vector3.up, Vector3.forward); // when the player is "misaligned" with the surface normal, this breaks

        resultRotation = resultRotation * Quaternion.AngleAxis(rotationAngle, axis.normalized);
        currentForward = resultRotation * Vector3.forward;
        currentNormal = resultRotation * Vector3.up;
    }

    // TODO: This needs to be rethought as simply moving to an attach point isn't a one-size-fits-all solution
    public void MoveToAttachPoint(ref Vector3 currentPosition, ref Vector3 attachPoint)
    {
        currentPosition = attachPoint; // Vector3.Lerp(currentPosition,attachPoint, 0.1f);
    }

    public void AlignOrientationWithSurface(ref Vector3 playerPosition, ref Quaternion playerOrientation, Vector3 downwardHitPoint, Vector3 forwardHitPoint, Vector3 surfaceNormal, float lerpValue)
    {
        
    }

    /// <summary>
    /// Zeroes out a rotation, useful for when want to update a rotation buffer.
    /// </summary>
    /// <param name="rotation">The rotation to be zeroed. Should be a rotation buffer.</param>
    public void ZeroRotation(ref Quaternion rotation)
    {
        rotation = Quaternion.identity;
    }
    /*
    public void VerifyDownwardAngle(ref Vector3 referenceVec, ref Vector3 currentDirection, ref bool downward)
    {
        Vector3 crossProduct = Vector3.Cross(currentDirection, referenceVec);


    }
    */
}
