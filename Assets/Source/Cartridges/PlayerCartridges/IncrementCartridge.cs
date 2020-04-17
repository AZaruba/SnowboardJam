using System;
public class IncrementCartridge
{
    /// <summary>
    /// Increments the current value by a specified amount
    /// </summary>
    /// <param name="timer">The float storing the value</param>
    /// <param name="time">The amount to incremement the value</param>
    /// <param name="max">The optional cap for the increment operation</param>
    public void Increment(ref float value, float delta, float cap = float.MaxValue)
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

    /// <summary>
    /// Decrements the current value by a specified amount
    /// </summary>
    /// <param name="timer">The float storing the value</param>
    /// <param name="time">The amount to decremement the value</param>
    /// <param name="max">The optional cap for the decrement operation</param>
    public void Decrement(ref float value, float delta, float cap = float.MaxValue)
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

    public void Increment(ref int value, int delta, int cap = int.MaxValue)
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


    public void Reset(ref float value)
    {
        value = 0.0f;
    }
}
