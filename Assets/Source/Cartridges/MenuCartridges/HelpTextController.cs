using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpTextController : MonoBehaviour
{
    [SerializeField] private Text UIText;
    private iMessageClient c_messageClient;

    // Start is called before the first frame update
    void Start()
    {
        c_messageClient = new HelpTextMessageClient(ref UIText);
        MessageServer.Subscribe(ref c_messageClient, MessageID.MENU_ITEM_CHANGED);
    }

    // Update is called once per frame
    void Update()
    {
        // no other functionality needed
    }
}
