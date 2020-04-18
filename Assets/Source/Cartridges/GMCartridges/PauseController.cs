using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Input system update
        if (Input.GetKeyDown(KeyCode.P))
        {
            Message pauseMessage = new Message((isPaused ? 0 : 1)); // if we want to pause, send 1, otherwise 0
            isPaused = !isPaused;
            MessageServer.SendMessage(MessageID.PAUSE, pauseMessage);
        }
    }
}
