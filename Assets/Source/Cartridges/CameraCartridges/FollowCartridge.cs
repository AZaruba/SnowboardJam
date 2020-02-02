using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCartridge {

    public void ApproachTarget2(ref Vector3 subjectPos, Vector3 destPos, float inertia = 0.5f)
    {
        subjectPos = Vector3.Lerp(subjectPos, destPos, inertia);
    }

    public void LeaveTarget2(ref Vector3 subjectPos, Vector3 destPos, float inertia = 0.5f)
    {
        subjectPos = Vector3.Lerp(destPos, subjectPos, inertia);
    }

    public void ApproachTarget(ref Vector3 subjectPosition, Vector3 targetPosition, Vector3 offsetVector, float inertia = 0.5f)
    {
        subjectPosition = Vector3.Lerp(subjectPosition, targetPosition + offsetVector, inertia);
    }

    /// <summary>
    /// Adjusts the current subject's position to the closest point that is
    /// a specific distance from the target.
    /// </summary>
    /// <param name="subjectPos"></param>
    /// <param name="targetPos"></param>
    /// <param name="desiredDistance"></param>
    public void AdjustToRadius(Vector3 subjectPos, ref Vector3 targetPos, float desiredDistance)
    {
        float currentDistance = Vector3.Distance(subjectPos, targetPos);

    }
}

/* NOTES:
 *  it is good to adjust based on the angle of travel
 *  keeping a specified distance is also crucial if the camera cannot move fast enough to keep up
 */ 
