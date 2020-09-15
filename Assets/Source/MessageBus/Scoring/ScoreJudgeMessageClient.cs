using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreJudgeMessageClient : iMessageClient
{
    private ScoreJudgeData c_judgeData;

    public ScoreJudgeMessageClient(ref ScoreJudgeData dataIn)
    {
        this.c_judgeData = dataIn;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        if (id == MessageID.TRICK_FINISHED)
        {
            c_judgeData.AddTrick(message.getTrickData());
            return true;
        }
        return false;
    }

    public bool SendMessage(MessageID id, Message message)
    {
        return false;
    }
}
