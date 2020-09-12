using System;
using System.Collections.Generic;

public class ScoringData
{
    private float CurrentSpinDegrees;
    private float CurrentFlipDegrees;

    private float SpinTarget;
    private float FlipTarget;

    private float CurrentTrickPoints;

    public List<TrickName> l_trickList;
    public List<float> l_timeList;

    public float f_currentTrickPoints
    {
        get { return CurrentTrickPoints; }
        set { CurrentTrickPoints = value; }
    }

    public float f_currentSpinDegrees
    {
        get { return CurrentSpinDegrees; }
        set { CurrentSpinDegrees = value; }
    }

    public float f_currentFlipDegrees
    {
        get { return CurrentFlipDegrees; }
        set { CurrentFlipDegrees = value; }
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

    public ScoringData()
    {
        this.f_currentFlipDegrees = 0.0f;
        this.f_currentSpinDegrees = 0.0f;

        this.f_currentFlipTarget = 0.0f;
        this.f_currentSpinTarget = 0.0f;

        this.l_timeList = new List<float>();
        this.l_trickList = new List<TrickName>();
    }
}
