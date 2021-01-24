using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditControllerData
{
    bool EditorActive;
    public bool b_editConfirmationActive;

    bool StoredBool;

    int MinInt;
    int MaxInt;
    int StoredInt;

    float MinFloat;
    float MaxFloat;
    float StoredFloat;

    KeyCode StoredKey;

    Resolution StoredResolution;

    private float CurrentTickTime;
    private float MaxTickTime;
    private int Increasing;

    public KeyCode k
    {
        get { return StoredKey; }
        set { StoredKey = value; }
    }

    public bool b_editorActive
    {
        get { return EditorActive; }
        set { EditorActive = value; }
    }

    public bool b
    {
        get { return StoredBool; }
        set { StoredBool = value; }
    }

    public int i
    {
        get { return StoredInt; }
        set { StoredInt = value; }
    }
    public int i_max
    {
        get { return MaxInt; }
        set { MaxInt = value; }
    }
    public int i_min
    {
        get { return MinInt; }
        set { MinInt = value; }
    }

    public float f
    {
        get { return StoredFloat; }
        set { StoredFloat = value; }
    }

    public Resolution res
    {
        get { return StoredResolution; }
        set { StoredResolution = value; }
    }

    public float f_currentTickTime
    {
        get { return CurrentTickTime; }
        set { CurrentTickTime = value; }
    }

    public float f_maxTickTime
    {
        get { return MaxTickTime; }
        set { MaxTickTime = value; }
    }

    public int i_increasing
    {
        get { return Increasing; }
        set { Increasing = value; }
    }

    public EditControllerData()
    {
        b_editConfirmationActive = false;
    }
}
