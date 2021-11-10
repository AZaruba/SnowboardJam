using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerMessageClient : iMessageClient
{
    ClientID clientID;
    private StateData c_stateData;

    public TimerMessageClient(ref StateData dataIn)
    {
        clientID = ClientID.TIMER_CLIENT;
        c_stateData = dataIn;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        if (id == MessageID.PAUSE)
        {
            c_stateData.b_updateState = message.getInt() == 0;
            return true;
        }

        // Multiplayer tip: ALL players need to be finished to stop the timer, if a player finishes just send a timestamp to the player
        if (id == MessageID.PLAYER_FINISHED)
        {
            c_stateData.b_updateState = false;
            return true;
        }
        if (id == MessageID.COUNTDOWN_OVER)
        {
            c_stateData.b_preStarted = false;
            return true;
        }
        return false;
    }
}
