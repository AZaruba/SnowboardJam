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
    public static void Turn(Vector3 normal, float handling, ref Quaternion currentRotation, float gripFactor = 1.0f)
    {
        Quaternion newRotation = Quaternion.AngleAxis(handling, normal);
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

    public void SetRotation(ref Quaternion rotationOut, Quaternion rotationIn)
    {
        rotationOut = rotationIn;
    }

    /// <summary>
    /// Takes the current orientation of the player and checks to see if it matches the desired
    /// orientation for finishing a spin/flip combo
    /// </summary>
    /// <param name="currentQ">The current rotation of the plyaer model</param>
    /// <param name="targetFwd">The direction of  travel/direction identifying "not spinning"</param>
    /// <param name="spinRate">The current spin rate</param>
    /// <param name="flipRate">The current flip rate</param>
    public void ValidateSpinRotation(float spinDegrees, float flipDegrees, float spinTarget, float flipTarget, ref float spinRate, ref float flipRate)
    {
        if (Mathf.Abs(spinDegrees) >= Mathf.Abs(spinTarget))
        {
            spinRate = 0.0f;
        }

        if (Mathf.Abs(flipDegrees) >= Mathf.Abs(flipTarget))
        {
            flipRate = 0.0f;
        }
    }
    #endregion
}
