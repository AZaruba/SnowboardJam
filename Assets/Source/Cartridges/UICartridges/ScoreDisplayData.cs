using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplayData : MonoBehaviour
{
    [SerializeField] private float ScoreTickRate;

    private int TargetScore;
    private int CurrentScore;
    private int DisplayScore;

    public ScoreDisplayData()
    {

    }

    // the score displayed in the UI, which will tick up over time
    public int i_displayScore
    {
        get { return DisplayScore; }
        set { DisplayScore = value; }
    }

    // whatever score is currently in the "backend," which will stop the incrementation
    public int i_currentScore
    {
        get { return CurrentScore; }
        set { CurrentScore = value; }
    }
}
