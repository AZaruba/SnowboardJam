using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickData : MonoBehaviour
{
    [SerializeField] private TrickName DownTrick;
    [SerializeField] private TrickName LeftTrick;
    [SerializeField] private TrickName RightTrick;
    [SerializeField] private TrickName UpTrick;

    private Trick UpTrickData;
    private Trick DownTrickData;
    private Trick LeftTrickData;
    private Trick RightTrickData;

    private Trick ActiveTrick;
    private float CurrentTrickPoints;

    public TrickData()
    {
        UpTrickData = new Trick(10, 5);
        DownTrickData = new Trick(10, 5);
        LeftTrickData = new Trick(10, 5);
        RightTrickData = new Trick(10, 5);
        CurrentTrickPoints = 0;
    }

    public Trick t_activeTrick
    {
        get { return ActiveTrick; }
        set { ActiveTrick = value; }
    }

    public float i_trickPoints
    {
        get { return CurrentTrickPoints; }
        set { CurrentTrickPoints = value; }
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

    public Trick trick_data_down
    {
        get { return DownTrickData; }
        set { DownTrickData = value; }
    }

    public Trick trick_data_left
    {
        get { return LeftTrickData; }
        set { LeftTrickData = value; }
    }

    public Trick trick_data_right
    {
        get { return RightTrickData; }
        set { RightTrickData = value; }
    }

    public Trick trick_data_up
    {
        get { return UpTrickData; }
        set { UpTrickData = value; }
    }
}
