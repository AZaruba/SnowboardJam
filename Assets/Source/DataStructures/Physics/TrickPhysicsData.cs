using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class containing the active data and attributes
/// 
/// </summary>
public class TrickPhysicsData
{
    #region Members
    private float MinimumSpinRate;
    private float MaximumSpinRate;
    private float CurrentSpinRate;

    private float CurrentSpinCharge;
    private float SpinIncrement;
    private float SpinDecay;

    private float MinimumFlipRate;
    private float MaximumFlipRate;
    private float CurrentFlipRate;
    private float CurrentFlipCharge;
    private float FlipIncrement;
    private float FlipDecay;

    #endregion

    #region Accessors
    public float f_minSpinRate
    {
        get { return MinimumSpinRate; }
        set { MinimumSpinRate = value; }
    }

    public float f_maxSpinRate
    {
        get { return MaximumSpinRate; }
        set { MaximumSpinRate = value; }
    }

    public float f_currentSpinRate
    {
        get { return CurrentSpinRate; }
        set { CurrentSpinRate = value; }
    }

    public float f_currentSpinCharge
    {
        get { return CurrentSpinCharge; }
        set { CurrentSpinCharge = value; }
    }

    public float f_spinIncrement
    {
        get { return SpinIncrement; }
        set { SpinIncrement = value; }
    }

    public float f_spinDecay
    {
        get { return SpinDecay; }
        set { SpinDecay = value; }
    }

    public float f_minFlipRate
    {
        get { return MinimumFlipRate; }
        set { MinimumFlipRate = value; }
    }

    public float f_maxFlipRate
    {
        get { return MaximumFlipRate; }
        set { MaximumFlipRate = value; }
    }

    public float f_currentFlipRate
    {
        get { return CurrentFlipRate; }
        set { CurrentFlipRate = value; }
    }

    public float f_currentFlipCharge
    {
        get { return CurrentFlipCharge; }
        set { CurrentFlipCharge = value; }
    }

    public float f_flipIncrement
    {
        get { return FlipIncrement; }
        set { FlipIncrement = value; }
    }

    public float f_flipDecay
    {
        get { return FlipDecay; }
        set { FlipDecay = value; }
    }
    #endregion

    #region Constructor
    public TrickPhysicsData(int TrickStat, int MaxTrickStat)
    {
        float TrickStatRatio = (float)TrickStat / (float)MaxTrickStat;

        this.MinimumFlipRate = TrickStatRatio * 50f;
        this.MaximumFlipRate = TrickStatRatio * 100f;
        this.CurrentFlipRate = 0.0f;
        this.CurrentFlipCharge = 0.0f;
        this.FlipDecay = TrickStatRatio;
        this.FlipIncrement = TrickStatRatio * 20f;

        this.MinimumSpinRate = TrickStatRatio * 50f;
        this.MaximumSpinRate = TrickStatRatio * 150f;
        this.CurrentSpinRate = 0.0f;
        this.CurrentSpinCharge = 0.0f;
        this.SpinDecay = TrickStatRatio;
        this.SpinIncrement = TrickStatRatio * 20f;
    }
    #endregion
}
