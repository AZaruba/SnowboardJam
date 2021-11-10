using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FocusCartridge
{
    public static void PointVectorAt(ref Vector3 subjectPosition, ref Vector3 targetPosition, ref Vector3 lookVectorOut)
    {
        lookVectorOut = targetPosition - subjectPosition;
    }

    public static void PointVectorAtLerp(ref Vector3 lookVectorOut, Vector3 targetLookVector, float inertia = 0.5f)
    {
        lookVectorOut = Vector3.Lerp(lookVectorOut, (lookVectorOut + targetLookVector).normalized, inertia); // normalized composite of camera direction and desired player direction
    }
}
