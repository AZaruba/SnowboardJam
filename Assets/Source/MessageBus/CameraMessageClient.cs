using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMessageClient : iMessageClient
{
    ClientID clientID;

    public CameraMessageClient()
    {
        clientID = ClientID.CAMERA_CLIENT;
    }

    public bool SendMessage(MessageID id, Message message)
    {
        MessageServer.SendMessage(id, message);
        return true;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        Debug.Log("Camera Recieved " + id);
        return true;
    }

}
