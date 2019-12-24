using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCartridge {

    public void ApproachTarget(ref Vector3 subjectPos, Vector3 destPos, float inertia = 0.5f)
    {
        subjectPos = Vector3.Lerp(subjectPos, destPos, Time.deltaTime);
    }

    public void LeaveTarget(ref Vector3 subjectPos, Vector3 destPos, float inertia = 0.5f)
    {
        Vector3 relativeDestination = subjectPos - destPos;
        subjectPos = Vector3.Lerp(subjectPos, relativeDestination, Time.deltaTime);
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
