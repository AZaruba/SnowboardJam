using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 InterpolateFixedVector(Vector3 lastPosition, Vector3 targetPosition)
    {
        float timeAlpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
        return Vector3.Lerp(lastPosition, targetPosition, timeAlpha);
    }

    public static Quaternion InterpolateFixedQuaternion(Quaternion lastRotation, Quaternion targetRotation)
    {
        float timeAlpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
        return Quaternion.Lerp(lastRotation, targetRotation, timeAlpha);
    }

    public static float InterpolateFixedFloat(float lastFloat, float targetFloat)
    {
        float timeAlpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
        return (lastFloat * (1.0f - timeAlpha)) + (targetFloat * timeAlpha);
    }

    public static Color InterpolateFixedColor(Color lastColor, Color targetColor)
    {
        float timeAlpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
        return Color.Lerp(lastColor, targetColor, timeAlpha);
    }
}
