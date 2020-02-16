using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMessageClient : iMessageClient
{
    // should we just store the message then let classes access that directly?
    ClientID clientID;
    private int currentScore;

    public ScoreMessageClient()
    {
        clientID = ClientID.SCORE_CLIENT;
    }

    public bool SendMessage(MessageID id, Message message)
    {
        return true;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        if (id == MessageID.SCORE_EDIT)
        {
            currentScore += message.getInt();
            return true;
        }
        return false;
    }

    public int getCurrentScore()
    {
        return currentScore;
    }
}
