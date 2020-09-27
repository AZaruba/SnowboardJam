using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMessageClient : iMessageClient
{
    ClientID clientID;
    StateData c_stateData;
    EntityData c_entityData;
    AudioController c_audioController;

    public CharacterMessageClient(ref StateData dataIn, ref EntityData entDataIn, ref AudioController audioIn)
    {
        clientID = ClientID.CHARACTER_CLIENT;
        this.c_stateData = dataIn;
        this.c_entityData = entDataIn;
        this.c_audioController = audioIn;
    }

    public bool SendMessage(MessageID id, Message message)
    {
        MessageServer.SendMessage(id, message);
        return true;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        if (id == MessageID.PAUSE)
        {
            c_stateData.b_updateState = message.getInt() == 0;
        }
        if (id == MessageID.PLAYER_FINISHED && message.getUint() == c_entityData.u_entityID)
        {
            c_stateData.b_courseFinished = true;
        }
        if (id == MessageID.PLAY_AUDIO)
        {
            c_audioController.PlayAudio(message.getAudioData());
        }
        if (id == MessageID.PLAY_ONE_SHOT)
        {
            c_audioController.PlayOneShot(message.getAudioData());
        }

        return true;
    }
}
