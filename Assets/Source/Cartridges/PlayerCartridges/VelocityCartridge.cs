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

    public void SurfaceAdjustment(ref Vector3 position, Vector3 point, Quaternion currentRotation)
    {
        position = Vector3.Lerp(position, point + currentRotation * new Vector3(0, 1.1f, 0), 0.5f);
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
    #endregion
}
