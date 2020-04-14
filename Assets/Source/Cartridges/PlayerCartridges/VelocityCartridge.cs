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

    /* the Raycast Adjustment should rotate the player around an axis point until they are in the right position
     * The reason for a rotation is to ensure that any adjustment keeps the POSITION DELTA the same.
     * 
     * Theoretically, if the player is travelling parallel to the surface, this adjustment should be zero
     * and can either be short circuited for performance OR left as-is
     * 
     */ 
    public void SurfaceAdjustment(ref Vector3 position, Vector3 previousPosition, Vector3 point, Vector3 normal, Vector3 surfaceNormal, Quaternion currentRotation)
    {
        if (normal == surfaceNormal)
        {
            return;
        }

        position = point + currentRotation * new Vector3(0, 1.1f, 0);
    }

    public void RaycastAdjustment(ref Vector3 surfacePoint, ref Vector3 currentPosition, ref Quaternion currentRotation)
    {
        currentPosition = surfacePoint + currentRotation * new Vector3(0,1.1f,0);
    }
    #endregion
}
