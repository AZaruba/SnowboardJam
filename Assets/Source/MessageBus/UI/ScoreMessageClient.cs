﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMessageClient : iMessageClient
{
    // should we just store the message then let classes access that directly?
    ClientID clientID;
    private ScoreDisplayData data_out;

    public ScoreMessageClient(ref ScoreDisplayData scoreData)
    {
        clientID = ClientID.SCORE_CLIENT;
        data_out = scoreData;
    }

    public bool SendMessage(MessageID id, Message message)
    {
        return true;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        if (id == MessageID.SCORE_EDIT)
        {
            data_out.i_currentScore += message.getInt();
            return true;
        }
        return false;
    }
}
