using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMessageClient : iMessageClient
{
    // should we just store the message then let classes access that directly?
    ClientID clientID;
    private ScoreDisplayData data_out;
    private StateData c_stateData;

    public ScoreMessageClient(ref ScoreDisplayData scoreData, ref StateData dataIn)
    {
        clientID = ClientID.SCORE_CLIENT;
        data_out = scoreData;
        c_stateData = dataIn;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        if (id == MessageID.PAUSE)
        {
            c_stateData.b_updateState = message.getInt() == 0;
            return true;
        }
        if (id == MessageID.SCORE_EDIT)
        {
            data_out.i_currentScore += message.getInt();
            return true;
        }
        return false;
    }
}
