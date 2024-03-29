﻿using System.Collections;
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

    private float CurrentSpinDegrees;
    private float CurrentFlipDegrees;

    private float GroundResetRate;
    private int GroundResetDir;
    private float GroundResetTarget;
    private float CurrentGroundResetRotation;
    private float ResetRate;

    private Quaternion StartRotation;
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

    public float f_groundResetRate
    {
        get { return GroundResetRate; }
        set { GroundResetRate = value; }
    }

    public int i_groundResetDir
    {
        get { return GroundResetDir; }
        set { GroundResetDir = value; }
    }

    public float f_groundResetTarget
    {
        get { return GroundResetTarget; }
        set { GroundResetTarget = value; }
    }

    public float f_groundResetRotation
    {
        get { return CurrentGroundResetRotation; }
        set { CurrentGroundResetRotation = value; }
    }

    public float f_resetRate
    {
        get { return ResetRate; }
        set { ResetRate = value; }
    }

    public Quaternion q_startRotation
    {
        get { return StartRotation; }
        set { StartRotation = value; }
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
    #endregion

    #region Constructor
    public TrickPhysicsData(int TrickStat, int MaxTrickStat)
    {
        float TrickStatRatio = (float)TrickStat / (float)MaxTrickStat;

        this.MinimumFlipRate = TrickStatRatio * 1.0f;
        this.MaximumFlipRate = TrickStatRatio * 2.0f;
        this.CurrentFlipRate = 0.0f;
        this.CurrentFlipCharge = 0.0f;
        this.FlipDecay = TrickStatRatio * 0.2f;
        this.FlipIncrement = TrickStatRatio * 0.5f;

        this.MinimumSpinRate = TrickStatRatio * 1.0f;
        this.MaximumSpinRate = TrickStatRatio * 2.5f;
        this.CurrentSpinRate = 0.0f;
        this.CurrentSpinCharge = 0.0f;
        this.SpinDecay = TrickStatRatio * 0.2f;
        this.SpinIncrement = TrickStatRatio * 0.5f;

        this.GroundResetRate = TrickStatRatio * 2f;
        this.ResetRate = TrickStatRatio * 3f;
        this.GroundResetTarget = Constants.ZERO_F;
        this.CurrentGroundResetRotation = Constants.ZERO_F;

        this.f_currentFlipDegrees = Constants.ZERO_F;
        this.f_currentSpinDegrees = Constants.ZERO_F;
        this.i_groundResetDir = Constants.ZERO;

        this.StartRotation = Quaternion.identity;
    }
    #endregion
}
