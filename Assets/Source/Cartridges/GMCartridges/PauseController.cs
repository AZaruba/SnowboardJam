using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PauseControllerData
{
    private bool IsPaused;

    public bool b_isPaused
    {
        get { return IsPaused; }
        set { IsPaused = value; }
    }
}

public class PauseMessageClient : iMessageClient
{
    PauseControllerData c_data;

    public PauseMessageClient(ref PauseControllerData dataIn)
    {
        this.c_data = dataIn;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        if (id == MessageID.PAUSE)
        {
            c_data.b_isPaused = !c_data.b_isPaused;
            return true;
        }
        return false;
    }
}

public class PauseController : MonoBehaviour
{
    PauseControllerData c_data;
    iMessageClient c_client;

    // Start is called before the first frame update
    void Start()
    {
        c_data = new PauseControllerData();
        c_data.b_isPaused = false;
        InitializeClient();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GlobalInputController.GetInputAction(ControlAction.PAUSE, KeyValue.PRESSED))
        {
            Message pauseMessage = new Message((c_data.b_isPaused ? 0 : 1)); // if we want to pause, send 1, otherwise 0
            //isPaused = !isPaused;
            MessageServer.SendMessage(MessageID.PAUSE, pauseMessage);
        }
    }

    private void InitializeClient()
    {
        c_client = new PauseMessageClient(ref c_data);
        MessageServer.Subscribe(ref c_client, MessageID.PAUSE);
    }
}
