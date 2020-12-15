using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iMessageClient
{
    bool RecieveMessage(MessageID id, Message message);
}
