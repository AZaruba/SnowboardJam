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

    public bool SendMessage(MessageID id)
    {
        MessageServer.SendMessage(id);
        return true;
    }

    public bool RecieveMessage(MessageID id)
    {
        Debug.Log("Camera Recieved " + id);
        return true;
    }

}
