using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 InterpolateFixedVector(Vector3 lastPosition, Vector3 targetPosition)
    {
        return Vector3.Lerp(lastPosition, targetPosition, TimestepInterpolator.FIXED_INTERPOLATION_VALUE);
    }

    public static Quaternion InterpolateFixedQuaternion(Quaternion lastRotation, Quaternion targetRotation)
    {
        return Quaternion.Lerp(lastRotation, targetRotation, TimestepInterpolator.FIXED_INTERPOLATION_VALUE);
    }

    public static float InterpolateFixedFloat(float lastFloat, float targetFloat)
    {
        return (lastFloat * (1.0f - TimestepInterpolator.FIXED_INTERPOLATION_VALUE)) + (targetFloat * TimestepInterpolator.FIXED_INTERPOLATION_VALUE);
    }

    public static float InterpolateFloatManual(float min, float max, float target)
    {
        return (min * (Constants.ONE - max)) +(target * max);
    }

    public static Color InterpolateFixedColor(Color lastColor, Color targetColor)
    {
        return Color.Lerp(lastColor, targetColor, TimestepInterpolator.FIXED_INTERPOLATION_VALUE);
    }
}
