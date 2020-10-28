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

    /* TODO:
     * This doesn't work as intended, though it is CALLED as intended. It's entirely possible
     * that the parameters provided aren't right, but the goal here is:
     * 
     *  currentPosition, via a rotation, realigns
     *  tailPosition, as our pivot, should remain the same
     *  nosePosition should end up at targetNosePosition
     *  
     *  the rotation, forward, and normal are all rotated by the rotation applied to currentPosition
     *  
     *  Gentle reminder to remove "Forward" entirely as the currentRotation should be able to express the orientation
     *  of the object correctly.
     */ 
    public void AlignToSurfaceByTail(ref Vector3 currentPosition,
                                Vector3 tailPosition,
                                Vector3 nosePosition,
                                Vector3 targetNosePosition,
                                ref Quaternion currentRotation,
                                ref Vector3 currentForward,
                                ref Vector3 currentNormal)
    {
        return; 
        /* Two vectors NP - TP
         *            TNP - TP
         * have some rotation between them. 
         */
        Quaternion surfaceRotation = Quaternion.FromToRotation(nosePosition - tailPosition, targetNosePosition - tailPosition).normalized;

        Vector3 rotationPivot = currentPosition - tailPosition;
        Vector3 resultPivot = surfaceRotation * rotationPivot;
        currentPosition = currentPosition - rotationPivot + resultPivot;
        currentRotation *= surfaceRotation;

        currentNormal = currentRotation * Vector3.up;
        currentForward = currentRotation * Vector3.forward;
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

    // TODO: This needs to be rethought as simply moving to an attach point isn't a one-size-fits-all solution
    public void MoveToAttachPoint(ref Vector3 currentPosition, ref Vector3 attachPoint)
    {
        currentPosition = attachPoint; // Vector3.Lerp(currentPosition,attachPoint, 0.1f);
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
    /*
    public void VerifyDownwardAngle(ref Vector3 referenceVec, ref Vector3 currentDirection, ref bool downward)
    {
        Vector3 crossProduct = Vector3.Cross(currentDirection, referenceVec);


    }
    */
}
