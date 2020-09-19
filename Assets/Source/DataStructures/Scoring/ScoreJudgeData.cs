using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreJudgeData
{
    public List<TrickMessageData> l_trickHistory;

    public ScoreJudgeData()
    {
        l_trickHistory = new List<TrickMessageData>();
    }

    public void AddTrick(TrickMessageData trickIn)
    {
        l_trickHistory.Add(trickIn);
    }
}
