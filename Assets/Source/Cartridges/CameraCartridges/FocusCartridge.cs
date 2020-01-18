using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCartridge
{
    public void PointVectorAt(ref Vector3 subjectPosition, ref Vector3 targetPosition, ref Vector3 lookVectorOut)
    {
        lookVectorOut = targetPosition - subjectPosition;
    }

    public void PointVectorAtLerp(ref Vector3 lookVectorOut, Vector3 targetLookVector, float inertia = 0.5f)
    {
        lookVectorOut = Vector3.Lerp(lookVectorOut, targetLookVector, inertia);
    }
}
