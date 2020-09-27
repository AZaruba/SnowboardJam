using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreJudge
{
    private iMessageClient c_messageClient;
    private ScoreJudgeData c_judgeData;

    public ScoreJudge()
    {
        InitializeData();
        InitializeMessageClient();
    }

    private void InitializeData()
    {
        c_judgeData = new ScoreJudgeData();
    }

    private void InitializeMessageClient()
    {
        c_messageClient = new ScoreJudgeMessageClient(ref c_judgeData);
        MessageServer.Subscribe(ref c_messageClient, MessageID.TRICK_FINISHED);
    }
}
