using UnityEngine;

public static class IncrementCartridge
{
    /// <summary>
    /// Increments the current value by a specified amount
    /// </summary>
    /// <param name="timer">The float storing the value</param>
    /// <param name="time">The amount to incremement the value</param>
    /// <param name="max">The optional cap for the increment operation</param>
    public static void Increment(ref float value, float delta, float cap = float.MaxValue, float floor = float.MinValue)
    {
        if (value < floor)
        {
            value = floor;
        }
        if (value >= cap)
        {
            value = cap;
        }
        else
        {
            value += delta;
        }
    }

    /// <summary>
    /// Decrements the current value by a specified amount
    /// </summary>
    /// <param name="timer">The float storing the value</param>
    /// <param name="time">The amount to decremement the value</param>
    /// <param name="max">The optional cap for the decrement operation</param>
    public static void Decrement(ref float value, float delta, float cap = float.MinValue)
    {
        if (value <= cap)
        {
            value = cap;
        }
        else
        {
            value -= delta;
        }
    }

    /// <summary>
    /// Decrements the current value by a specified amount
    /// </summary>
    /// <param name="timer">The int storing the value</param>
    /// <param name="time">The amount to decremement the value</param>
    /// <param name="max">The optional cap for the decrement operation</param>
    public static void Decrement(ref int value, int delta, int cap = int.MinValue)
    {
        if (value <= cap)
        {
            value = cap;
        }
        else
        {
            value -= delta;
        }
    }

    /// <summary>
    /// Decrements and absolute value. Pulls positive and negative values to zero
    /// </summary>
    /// <param name="value"></param>
    /// <param name="delta"></param>
    /// <param name="cap"></param>
    public static void DecrementAbs(ref float value, float delta, float cap = float.MinValue)
    {
        int sign = (value > 0.0f) ? 1 : -1;
        float absVal = Mathf.Abs(value);

        Decrement(ref absVal, Mathf.Abs(delta), cap);
        value = absVal * sign;
    }


    /// <summary>
    /// Increment and absolute value. Pulls positive and negative values to zero
    /// </summary>
    /// <param name="value"></param>
    /// <param name="delta"></param>
    /// <param name="cap"></param>
    public static void IncrementAbs(ref float value, float delta, float cap = float.MaxValue)
    {
        int sign = (delta > 0.0f) ? 1 : -1;
        float absVal = Mathf.Abs(value);

        Increment(ref absVal, Mathf.Abs(delta), cap);
        value = absVal * sign;
    }


    public static void Increment(ref int value, int delta, int cap = int.MaxValue)
    {
        if (value >= cap)
        {
            value = cap;
        }
        else
        {
            value += delta;
        }
    }

    public static void Rotate(ref int value, int delta, int cap = int.MaxValue, int min = 0)
    {
        if (value == min && delta < 0)
        {
            value = cap - 1;
        }
        else if (value == (cap - 1) && delta > 0)
        {
            value = min;
        }
        else
        {
            value += delta;
        }
    }

    /// <summary>
    /// Increments or decrements a value to approach the minimum or maximum
    /// </summary>
    /// <param name="value"></param>
    /// <param name="delta"></param>
    /// <param name="minimum"></param>
    /// <param name="maximum"></param>
    public static void IncrementTethered(ref float value, float delta, float minimum = float.MinValue, float maximum = float.MaxValue)
    {
        if (value <= minimum)
        {
            value = minimum;
        }
        else if (value >= maximum)
        {
            value = maximum;
        }
        else
        {
            value += delta;
        }
    }

    public static void Reset(ref float value)
    {
        value = 0.0f;
    }
}
