using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectSceneController : MonoBehaviour
{
    private iMessageClient c_messageClient;
    public Dictionary<PlayerID, bool> playersReady;
    public bool b_messageRecieved;

    private void Awake()
    {
        c_messageClient = new CharacterSelectSceneClient(this);
        MessageServer.Subscribe(ref c_messageClient, MessageID.CHARACTER_SELECTED);
        MessageServer.Subscribe(ref c_messageClient, MessageID.CHARACTER_UNSELECTED);

        b_messageRecieved = false;

        playersReady = new Dictionary<PlayerID, bool>();
        playersReady.Add(PlayerID.PLAYER1, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (b_messageRecieved)
        {
            bool b_allCharactersReady = PollForReadyPlayers();
            if (b_allCharactersReady)
            {
                // go!
            }
            else
            {
                b_messageRecieved = false;
            }
        }
    }

    private bool PollForReadyPlayers()
    {
        foreach (KeyValuePair<PlayerID, bool> pair in playersReady)
        {
            if (!pair.Value)
            {
                // we are not ready until everyone is ready
                return false;
            }
        }
        return true;
    }
}
