using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlingCartridge {

    #region PublicFunctions
    /// <summary>
    /// The turning action, takes the direction and rotates it by an angle defined
    /// by a function of the handling value.
    /// </summary>
    /// <param name="direction">The object's current direction.</param>
    /// <param name="handling">The object's handling value, which should be an axis plus a multiplier.</param>
    /// <param name="gripFactor">Grip should potentially affect handling</param>
    public void Turn(ref Vector3 direction, ref Vector3 normal, ref float handling, ref Quaternion currentRotation, float gripFactor = 1.0f)
    {
        // THIS is the culprit! We only go haywire while turning
        Quaternion newRotation = Quaternion.AngleAxis(handling * Time.deltaTime, Vector3.up);
        direction = newRotation * direction;
        currentRotation *= newRotation;
    }

    // TODO: Include a function for turning the model and NOT the direction of travel.
    #endregion
}
