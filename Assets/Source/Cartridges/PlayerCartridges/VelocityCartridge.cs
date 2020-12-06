using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityCartridge {

    #region PublicFunctions
    public void UpdatePosition(ref Vector3 position, ref Vector3 direction, ref float velocity)
    {
        position += direction.normalized * velocity * Time.deltaTime;
    }

    // Test function: We update the character's position via the rotation, why not do it here?
    public void UpdatePositionTwo(ref Vector3 position, ref Quaternion rotation, ref float velocity)
    {
        Vector3 direction = rotation * Vector3.forward;
        position += direction.normalized * velocity * Time.deltaTime;
    }

    public void UpdateAerialPosition(ref Vector3 position, Vector3 lateralDir, float verticalVelocity, float lateralVelocity)
    {
        position += lateralDir * lateralVelocity * Time.deltaTime;
        position += Vector3.up * verticalVelocity * Time.deltaTime;
    }

    public void SurfaceAdjustment(ref Vector3 position, Vector3 point, Quaternion currentRotation)
    {
        position = Vector3.Lerp(position, point + currentRotation * new Vector3(0, 1.1f, 0), 0.5f);
    }
     
    public void SurfaceAdjustment(ref Vector3 position, Vector3 offset)
    {
        position += offset;
    }

    public void SurfaceAdjustment(ref Vector3 position, Vector3 point, Vector3 forwardPoint, float angleDifference)
    {
        Vector3 positionVector = position - point;
        Vector3 forwardVector = forwardPoint - point;

    }

    public void RaycastAdjustment(ref Vector3 surfacePoint, ref Vector3 currentPosition, ref Quaternion currentRotation)
    {
        currentPosition = surfacePoint + currentRotation * new Vector3(0,1.1f,0);
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
