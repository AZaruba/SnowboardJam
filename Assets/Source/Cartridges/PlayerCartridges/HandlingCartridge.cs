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
    public void Turn(ref Vector3 direction, Vector3 normal, ref float handling, ref Quaternion currentRotation, float gripFactor = 1.0f)
    {
        Quaternion newRotation = Quaternion.AngleAxis(handling * Time.deltaTime, normal);
        direction = newRotation * direction;
        currentRotation = currentRotation * newRotation;
    }

    /// <summary>
    /// Applies a pre-calculated rotation to an already rotated object
    /// </summary>
    /// <param name="target">The orientation of the object</param>
    /// <param name="rotation"></param>
    public void ApplyRotation(ref Quaternion target, ref Quaternion rotation)
    {
        rotation = target * rotation;
    }

    // TODO: Include a function for turning the model and NOT the direction of travel.
    #endregion
}
