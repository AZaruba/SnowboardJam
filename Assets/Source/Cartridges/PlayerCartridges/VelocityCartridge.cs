using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VelocityCartridge {

    #region PublicFunctions
    public static void UpdatePositionTwo(ref Vector3 position, ref Quaternion rotation, ref float velocity)
    {
        Vector3 direction = rotation * Vector3.forward;
        position += direction.normalized * velocity * Time.deltaTime;
    }

    public static void UpdateAerialPosition(ref Vector3 position, Vector3 lateralDir, float verticalVelocity, float lateralVelocity)
    {
        position += lateralDir * lateralVelocity * Time.deltaTime;
        position += Vector3.up * verticalVelocity * Time.deltaTime;
    }

    /// <summary>
    /// Tracks between two positions as defined by lerpRatio.
    /// </summary>
    /// <param name="currentPosition">The current position of the object.</param>
    /// <param name="startPosition">The beginning of the two positions we lerp between.</param>
    /// <param name="endPosition">The target position when lerpRatio is 1.</param>
    /// <param name="lerpRatio">The value between 0 and 1 definding where we want to end up.</param>
    public static void LerpPosition(ref Vector3 currentPosition, Vector3 startPosition, Vector3 endPosition, float lerpRatio)
    {
        currentPosition = Vector3.Lerp(startPosition, endPosition, lerpRatio);
    }
    #endregion
}
