using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityCartridge {

    #region PublicFunctions
    public void UpdatePosition(ref Vector3 translation, ref Vector3 direction, ref float velocity)
    {
        translation = direction.normalized * velocity;
    }

    public void RaycastAdjustment(ref Vector3 surfacePoint, ref Vector3 currentPosition, ref Vector3 currentTranslation)
    {
        // TODO: adjust with respect to "hitching" on a seam
        /*
         * Explanation: when we hit a seam, this operation will
         * "slow down" the delta, potentially even reversing it
         * if going upwards. 
         */ 
        Vector3 raycastDelta = surfacePoint - currentPosition + new Vector3(0, 0.5f, 0);
        currentTranslation += raycastDelta;
    }
    #endregion
}
