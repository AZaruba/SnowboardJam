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
    /// Adds an extra pull to the model rotation 
    /// </summary>
    /// <param name="normal">The surface normal</param>
    /// <param name="modelRotation">The current orientation of the model</param>
    /// <param name="travelRotation">The current direction of travel, expressed by rotation</param>
    public static void AddTurnCorrection(Vector3 normal, ref Quaternion modelRotation, Quaternion travelRotation, int switchStance)
    {
        Vector3 projectedModel = Vector3.ProjectOnPlane(modelRotation * Vector3.forward * switchStance, normal).normalized;
        Vector3 projectedTravel = Vector3.ProjectOnPlane(travelRotation * Vector3.forward, normal).normalized;

        float angleCorrection = Vector3.SignedAngle(projectedModel, projectedTravel, normal) / Constants.SWITCH_ANGLE;
        modelRotation = modelRotation * Quaternion.AngleAxis(angleCorrection * Time.deltaTime * 50, normal);
    }

    public static void SetRotation(ref Quaternion rotationOut, Quaternion rotationIn)
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
    public static void ValidateSpinRotation(float spinDegrees, float flipDegrees, float spinTarget, float flipTarget, ref float spinRate, ref float flipRate)
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
