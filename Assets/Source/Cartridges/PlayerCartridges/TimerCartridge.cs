using System;
public class TimerCartridge
{
    /// <summary>
    /// Increments the current timer
    /// </summary>
    /// <param name="timer">The float storing the timer value</param>
    /// <param name="time">The amount to incremement the timer</param>
    public void IncrementTimer(ref float timer, float deltaTime)
    {
        timer += deltaTime;
    }


    public void ResetTimer(ref float timer)
    {
        timer = 0.0f;
    }
}
