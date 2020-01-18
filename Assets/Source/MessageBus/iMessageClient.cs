using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iMessageClient
{
    bool SendMessage(MessageID id);
    bool RecieveMessage(MessageID id);
}
