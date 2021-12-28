using System;
using System.Collections.Generic;

public class ScoringData
{
    private float SpinTarget;
    private float FlipTarget;

    private float CurrentTrickPoints;

    private bool SendTrick;

    public List<TrickName> l_trickList;
    public List<float> l_timeList;

    public float f_currentTrickPoints
    {
        get { return CurrentTrickPoints; }
        set { CurrentTrickPoints = value; }
    }

    public float f_currentSpinTarget
    {
        get { return SpinTarget; }
        set { SpinTarget = value; }
    }

    public float f_currentFlipTarget
    {
        get { return FlipTarget; }
        set { FlipTarget = value; }
    }

    public bool b_sendTrick
    {
        get { return SendTrick; }
        set { SendTrick = value; }
    }

    public ScoringData()
    {
        this.f_currentFlipTarget = 0.0f;
        this.f_currentSpinTarget = 0.0f;

        this.l_timeList = new List<float>();
        this.l_trickList = new List<TrickName>();

        this.b_sendTrick = false;
    }
}
