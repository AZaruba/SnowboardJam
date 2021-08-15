using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationCartridge {

    #region PublicFunctions
    public static void AccelerateGravity(ref float velocity,
        float gravity,
        float topSpeed,
        ref Quaternion playerRotation,
        ref Quaternion modelRotation)
    {
        Vector3 projectedGravityAcceleration = Vector3.ProjectOnPlane(Vector3.down * gravity * Time.deltaTime, playerRotation * Vector3.up);
        Vector3 playerVec = playerRotation * Vector3.forward * velocity;

        velocity = (playerVec + projectedGravityAcceleration).magnitude;
        Quaternion resultRotation = Quaternion.FromToRotation(playerVec, playerVec + projectedGravityAcceleration);
        playerRotation = resultRotation * playerRotation;
        modelRotation = resultRotation * modelRotation;
        if (velocity > topSpeed)
        {
            velocity = topSpeed;
        }

    }

    public static void DecelerateFriction(ref float velocity,
        float surfaceValue,
        Quaternion playerRotation)
    {
        Vector3 velocityVec = playerRotation * Vector3.forward * velocity;

        Vector3 frictionVec = playerRotation * Vector3.back * surfaceValue * Time.deltaTime;

        velocity = (velocityVec + frictionVec).magnitude;
    }

    public static void AccelerateAbs(ref float velocity, float f_acceleration, float topSpeed, float surfaceFactor = 1.0f)
    {
        if (Mathf.Abs(velocity) > Mathf.Abs(topSpeed))
        {
            velocity = topSpeed * (velocity/Mathf.Abs(velocity));
            return;
        }
        velocity += f_acceleration * surfaceFactor * Time.deltaTime;
    }

    public static void Decelerate(ref float velocity, float deceleration, int switchStance, float surfaceFactor = 1.0f)
    {
        if (velocity * switchStance < 0)
        {
            velocity = 0.0f;
            return;
        }
        velocity -= deceleration * surfaceFactor * Time.deltaTime;
    }
    #endregion
}
