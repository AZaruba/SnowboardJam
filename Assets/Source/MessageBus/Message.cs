using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MessageID
{
    ERROR_MSG = -1,
    PAUSE,
    TEST_MSG_ONE,
    TEST_MSG_TWO,

    // UI message IDs
    SCORE_EDIT,
    TRICK_FINISHED,
    MENU_ITEM_CHANGED,
    PLAYER_FINISHED,

    // Scene State Message IDs
    CHARACTER_SELECTED,
    CHARACTER_UNSELECTED,
    ALL_CHARACTERS_SELECTED,

    // Nested Menu message IDs
    MENU_FORWARD,
    MENU_BACK,

    // Edit Menu Message IDs
    EDIT_START,
    EDIT_END,

    // Audio IDs
    PLAY_ONE_SHOT,
    PLAY_AUDIO,

    // Countdown ID
    COUNTDOWN_START,
    COUNTDOWN_OVER,

    // Physics and positioning
    PLAYER_POSITION_UPDATED
}

public enum ClientID
{
    ERROR_CLIENT = -1,
    CHARACTER_CLIENT,
    CAMERA_CLIENT,
    SCORE_CLIENT,
    TIMER_CLIENT,
    PAUSE_MENU_CLIENT,
    HELP_TEXT_CLIENT,
}

// compile trick data then send it
public struct TrickMessageData
{
    public float SpinDegrees;
    public float FlipDegrees;
    public float FlipAngle;
    public bool Success;

    public List<TrickName> grabs;
    public List<float> grabTimes;
}

public class Message
{
    private uint u_data;
    private int i_data;
    private float f_data;
    private string s_data;
    private TrickMessageData t_data;
    private AudioRef a_data;
    private Vector3 v_data;
    private Quaternion q_data;

    public Message()
    {

    }

    public Message(int iIn, float fIn = float.MaxValue, string sIn = "")
    {
        this.i_data = iIn;
        this.f_data = fIn;
        this.s_data = sIn;
    }

    public Message(string sIn, int iIn = int.MaxValue, float fIn = float.MaxValue)
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

    public Message(TrickMessageData dataIn)
    {
        t_data = dataIn;
    }

    public Message(AudioRef dataIn)
    {
        a_data = dataIn;
    }

    public Message (Vector3 vDataIn, Quaternion qDataIn)
    {
        v_data = vDataIn;
        q_data = qDataIn;
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

    public TrickMessageData getTrickData()
    {
        return t_data;
    }

    public AudioRef getAudioData()
    {
        return a_data;
    }

    public Vector3 getVector()
    {
        return v_data;
    }

    public Quaternion getQuaternion()
    {
        return q_data;
    }
}
