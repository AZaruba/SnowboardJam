using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iMessageClient
{
    bool SendMessage(MessageID id, Message message);
    bool RecieveMessage(MessageID id, Message message);
}
