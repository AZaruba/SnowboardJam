using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MessageID
{
    ERROR_MSG = -1,
    PAUSE,
    TEST_MSG_ONE,
    TEST_MSG_TWO,
    SCORE_EDIT,
    PLAYER_FINISHED,
}

public enum ClientID
{
    ERROR_CLIENT = -1,
    CHARACTER_CLIENT,
    CAMERA_CLIENT,
    SCORE_CLIENT,
    TIMER_CLIENT,
    PAUSE_MENU_CLIENT,
}

public class Message
{
    private uint u_data;
    private int i_data;
    private float f_data;
    private string s_data;

    public Message(int iIn = int.MaxValue, float fIn = float.MaxValue, string sIn = "")
    {
        this.i_data = iIn;
        this.f_data = fIn;
        this.s_data = sIn;
    }

    public Message(uint dataIn)
    {
        u_data = dataIn;
    }
    
    public Message(int dataIn)
    {
        i_data = dataIn;
    }

    public Message(float dataIn)
    {
        f_data = dataIn;
    }

    public Message(string dataIn)
    {
        s_data = dataIn;
    }

    public uint getUint()
    {
        return u_data;
    }

    public int getInt()
    {
        return i_data;
    }

    public float getFloat()
    {
        return f_data;
    }

    public string getString()
    {
        return s_data;
    }
}
