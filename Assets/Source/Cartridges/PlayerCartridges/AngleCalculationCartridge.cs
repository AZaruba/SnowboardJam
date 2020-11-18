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
     * Handle zero speed cases
     */ 
    public void AlignToSurfaceByTail(ref Vector3 currentPosition,
                                Vector3 tailPosition,
                                Vector3 nosePosition,
                                Vector3 targetNosePosition,
                                Vector3 noseNormal,
                                ref Quaternion currentRotation,
                                ref Vector3 currentForward,
                                ref Vector3 currentNormal)
    {
        if (noseNormal.normalized == currentNormal.normalized)
        {
            return;
        }

        // targetNosePosition is the point on the line
        Vector3 redLine = targetNosePosition - nosePosition;

        if (redLine.magnitude.Equals(0))
        {
            return;
        }

        Vector3 blueLine = nosePosition - tailPosition;
        Vector3 rotationAxis = Vector3.Cross(currentRotation * Vector3.forward, redLine);
        Vector3 lineDir = Vector3.Cross(noseNormal, rotationAxis); // plane normal X rotation axis gets us direction of the line
        float rpcDot = Vector3.Dot(lineDir, redLine);

        float desiredLengthSq = Mathf.Pow((nosePosition - tailPosition).magnitude, 2);
        float pcMagSq = Mathf.Pow((redLine).magnitude, 2);

        float a = lineDir.magnitude;
        float b = 2 * rpcDot;
        float c = pcMagSq - desiredLengthSq;

        float rootValue = (b * b) - (4 * a * c);
        if (rootValue < 0)
        {
            // how to handle complex numbers?
            return;
        }

        float plusQuad = ((-1 * b) + Mathf.Sqrt(b * b - (4 * a * c))) / (2 * a);
        float minusQuad = ((-1 * b) - Mathf.Sqrt(b * b - (4 * a * c))) / (2 * a);

        Vector3 plusVec = (targetNosePosition + plusQuad * lineDir) - tailPosition;
        Vector3 minusVec = (targetNosePosition + minusQuad * lineDir) - tailPosition;

        Quaternion newForward;
        if (Vector3.SignedAngle(plusVec, blueLine, rotationAxis) < Vector3.SignedAngle(minusVec, blueLine, rotationAxis))
        {
            // update position as well
            newForward = Quaternion.LookRotation(plusVec, Vector3.Cross(plusVec, rotationAxis)).normalized;
            currentPosition = Quaternion.FromToRotation((nosePosition - tailPosition).normalized, plusVec.normalized) * (currentPosition - tailPosition) + tailPosition;
        }
        else
        {
            newForward = Quaternion.LookRotation(minusVec, Vector3.Cross(minusVec, rotationAxis)).normalized;
            currentPosition = Quaternion.FromToRotation((nosePosition - tailPosition).normalized, minusVec.normalized) * (currentPosition - tailPosition) + tailPosition;
        }


        currentRotation = newForward;
        currentNormal = (newForward * Vector3.up).normalized;
        currentForward = (newForward * Vector3.forward).normalized;
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
