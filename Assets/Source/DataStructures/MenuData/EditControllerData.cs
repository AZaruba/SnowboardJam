using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditControllerData
{
    bool EditorActive;

    bool StoredBool;
    int StoredInt;
    float StoredFloat;
    Resolution StoredResolution;

    private float CurrentMenuTickTime;
    private float CurrentMenuWaitTime;

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

    public float f_currentMenuTickCount
    {
        get { return CurrentMenuTickTime; }
        set { CurrentMenuTickTime = value; }
    }

    public float f_currentMenuWaitCount
    {
        get { return CurrentMenuWaitTime; }
        set { CurrentMenuWaitTime = value; }
    }
}
