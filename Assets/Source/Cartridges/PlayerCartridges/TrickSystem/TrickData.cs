using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickData : MonoBehaviour
{
    [SerializeField] private TrickName DownTrick;
    [SerializeField] private TrickName LeftTrick;
    [SerializeField] private TrickName RightTrick;
    [SerializeField] private TrickName UpTrick;

    private TrickName ActiveTrickName;
    private ControlAction ActiveTrickAction;
    private float CurrentTrickPoints;
    private float ActiveTrickTime;

    public TrickData()
    {
        CurrentTrickPoints = 0;
        ActiveTrickName = TrickName.BLANK_TRICK;
        ActiveTrickTime = Constants.ZERO_F;
        ActiveTrickAction = ControlAction.ERROR_ACTION;
    }

    public TrickName t_activeTrickName
    {
        get { return ActiveTrickName; }
        set { ActiveTrickName = value; }
    }

    public ControlAction k_activeTrickAction
    {
        get { return ActiveTrickAction; }
        set { ActiveTrickAction = value; }
    }

    public float i_trickPoints
    {
        get { return CurrentTrickPoints; }
        set { CurrentTrickPoints = value; }
    }

    public float f_trickTime
    {
        get { return ActiveTrickTime; }
        set { ActiveTrickTime = value; }
    }

    public TrickName trick_down
    {
        get { return DownTrick; }
        set { DownTrick = value; }
    }

    public TrickName trick_left
    {
        get { return LeftTrick; }
        set { LeftTrick = value; }
    }

    public TrickName trick_right
    {
        get { return RightTrick; }
        set { RightTrick = value; }
    }

    public TrickName trick_up
    {
        get { return UpTrick; }
        set { UpTrick = value; }
    }
}
