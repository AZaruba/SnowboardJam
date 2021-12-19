using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AccelerationCartridge {

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

        // value incorporates stick direction so negatives work correctly here
        velocity += f_acceleration * surfaceFactor * Time.deltaTime;
    }

    public static void DecelerateAbs(ref float velocity, float f_acceleration, float surfaceFactor = 1.0f)
    {
        bool positiveTurnRate = Mathf.Sign(velocity) > 0;
        velocity -= f_acceleration * Mathf.Sign(velocity) * surfaceFactor * Time.deltaTime;

        // check for sign change, which signals we have "crossed" zero and reset
        if ((positiveTurnRate && Mathf.Sign(velocity) < 0) ||
            (!positiveTurnRate && Mathf.Sign(velocity) > 0))
        {
            velocity = Constants.ZERO_F;
        }
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

    /*
     * max = turn speed
     * min = reverse turn speed
     * current = current turn speed (between -200 and 200)
     * 
     * if currentValue is -200 and our target is -200, accel should be zero
     * if currentValue is -200 and our target is 200, accel should be maxed
     * 
     * vice versa for 200/-200.
     * 
     * 200 - (-200) = 400 / 400 = 1
     * -200 - (-200) = 0 /400 = 0
     */ 
    public static void CalculateInterpolatedAcceleration(out float currentAcceleration, float signedMaxAcceleration, float rangeMin, float rangeMax, float rangeCurrent)
    {
        float t = (rangeCurrent - rangeMin) / (rangeMax - rangeMin);

        currentAcceleration = Mathf.Lerp(signedMaxAcceleration, Constants.ZERO, t);
    }
    #endregion
}
