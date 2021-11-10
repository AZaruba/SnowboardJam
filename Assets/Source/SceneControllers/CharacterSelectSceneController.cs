using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                // some kind of feedback here

                /* Send Message to play ready animations
                 * then when animations are done, a message is sent saying "load the next scene"
                 * Then the secene is loaded here
                 */
                LoadCourseSelect();
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

    private void LoadCourseSelect()
    {
        SceneManager.LoadScene((int)Scene.COURSE_SELECT);
    }
}
