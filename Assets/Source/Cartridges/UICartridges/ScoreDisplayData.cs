using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplayData
{
    private int TargetScore;
    private int CurrentScore;

    public ScoreDisplayData()
    {

    }

    public int i_targetScore
    {
        get { return TargetScore; }
        set { TargetScore = value; }
    }

    public int i_currentScore
    {
        get { return CurrentScore; }
        set { CurrentScore = value; }
    }
}
