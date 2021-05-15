using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationCartridge {

    #region PublicFunctions
    public static void Accelerate(ref float velocity, float f_acceleration, float topSpeed, float surfaceFactor = 1.0f)
    {
        if (velocity > topSpeed)
        {
            velocity = topSpeed;
            return;
        }
        velocity += f_acceleration * surfaceFactor * Time.deltaTime;
    }

    // how to manage decelerating or accelerating
    public static void GravityAccelerate(ref float velocity, float gravityValue, Vector3 currentDirection, float topSpeed)
    {

        float gravityAcceleration = currentDirection.normalized.y * gravityValue;
        if (velocity > topSpeed)
        {
            velocity = topSpeed;
            return;
        }
        velocity -= gravityAcceleration * Time.deltaTime;
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

    public static void AccelerateSoftCap(ref float velocity, float acceleration, float topSpeed, float deceleration, float surfaceFactor = Constants.ONE)
    {
        if (velocity > topSpeed)
        {
            velocity -= deceleration * Time.fixedDeltaTime * (1/surfaceFactor);
            return;
        }
        velocity += acceleration * surfaceFactor * Time.fixedDeltaTime;
    }

    /// <summary>
    /// Ensures the current speed of the character does not go above a hard cap
    /// </summary>
    /// <param name="velocity">The current velocity</param>
    /// <param name="maxVelocity">The velocity cap</param>
    public void CapSpeed(ref float velocity, float maxVelocity)
    {
        if (velocity > maxVelocity)
            velocity = maxVelocity;
    }

    /// <summary>
    /// Ensures the speed does not go below a hard cap
    /// </summary>
    /// <param name="velocity">The current velocity</param>
    /// <param name="maxVelocity">The velocity minimum</param>
    public void CapSpeedMin(ref float velocity, float maxVelocity)
    {
        if (velocity < maxVelocity)
            velocity = maxVelocity;
    }

    /// <summary>
    /// If an object needs to move backwards (on an engine level, the use case for this would be the player riding switch)
    /// </summary>
    /// <param name="reversed">Whether the direction is forward or backward relative to the model.</param>
    /// <param name="velocity">The current velocity of the object, used to determine reversal.</param>
    /// <param name="direction">The player's direction of travel.</param>
    public void FlipDirection(ref bool reversed, ref float velocity, ref Vector3 direction)
    {
        bool newReversal = velocity < 0.0f; // true if we are going backwards (reversed is true)

        // TODO: Fix bug in which reversing direction propagates to every frame
        // on direction change (fwd to rev or rev to fwd)
        if (newReversal != reversed)
        {
            direction *= -1;
            velocity *= -1;
            reversed = !reversed;
        }
    }

    /// <summary>
    /// Forces a direction change. A particular use case is spinning such that the player lands switch and velocity needs to be preserved.
    /// </summary>
    /// <param name="reversed">Whether the direction is forward or backward relative to the model.</param>
    /// <param name="direction">The player's direction of travel.</param>
    public void ForceFlipDirection(ref bool reversed, ref float direction)
    {
        reversed = !reversed;
        direction *= -1;
    }
    #endregion
}
