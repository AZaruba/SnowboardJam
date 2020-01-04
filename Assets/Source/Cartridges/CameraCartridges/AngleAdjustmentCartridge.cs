using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleAdjustmentCartridge
{
    public void AdjustPositionOnRadius(ref Vector3 subjectPosition, Vector3 groundPosition, float height, float inertia = 0.5f)
    {
        Vector3 desiredCameraPosition = groundPosition;
        desiredCameraPosition.y += height;
        subjectPosition = Vector3.Lerp(subjectPosition, desiredCameraPosition, inertia);
    }

    // TODO: add a funciton to "swing" the camera closer to directly behind the target
}
