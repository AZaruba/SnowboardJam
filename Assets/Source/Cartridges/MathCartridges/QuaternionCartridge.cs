using System;
using UnityEngine;

public class QuaternionCartridge
{
    public void ApproachQuaternion(ref Quaternion currentOrientation, Quaternion target, float rate)
    {
        currentOrientation = Quaternion.RotateTowards(currentOrientation, target, rate);
    }
}
