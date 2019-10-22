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
    public void AlignRotationWithSurface(ref Vector3 targetNormal, ref Vector3 currentNormal, ref Vector3 currentForward)
    {
        currentForward = Quaternion.FromToRotation(currentNormal, targetNormal) * currentForward;
        currentNormal = targetNormal;
    }

    public void MoveToAttachPoint(ref Vector3 currentPosition, ref Vector3 attachPoint)
    {
        currentPosition = attachPoint; // Vector3.Lerp(currentPosition,attachPoint, 0.1f);
    }

    /*
    public void VerifyDownwardAngle(ref Vector3 referenceVec, ref Vector3 currentDirection, ref bool downward)
    {
        Vector3 crossProduct = Vector3.Cross(currentDirection, referenceVec);


    }
    */
}
