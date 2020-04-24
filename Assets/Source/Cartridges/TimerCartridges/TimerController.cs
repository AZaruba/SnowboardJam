using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    // TODO: how do we grab the time for the current level if different levels have different time limits?
    private TimerData c_timerData;
    private StateData c_stateData;
    private StateMachine sm_timer;

    private IncrementCartridge cart_incr;

    private iMessageClient cl_timer;

    // DEBUG: implement UI-backend messaging if there are reasons to, as it keeps the architecture consistent
    [SerializeField] private Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        SetDefaultTimerData();
        InitializeStateMachine();
        InitializeMessageClient();
    }

    // Update is called once per frame
    void Update()
    {
        if (!c_stateData.b_updateState)
        {
            return;
        }

        sm_timer.Act();
        float displayTime = c_timerData.f_currentTime;
        int minutes = (int)displayTime / 60;
        int seconds = (int)displayTime % 60;
        int millis = (int)(displayTime * 100) % 100;

        timerText.text = string.Format("{0:0}:{1:00}:{2:00}", minutes, seconds, millis);
    }

    private void SetDefaultTimerData()
    {
        c_timerData = new TimerData();
        c_stateData = new StateData();

        c_timerData.f_currentTime = 0.0f;
        c_stateData.b_updateState = true;
    }

    private void InitializeStateMachine()
    {
        cart_incr = new IncrementCartridge();

        TimeDecreaseState s_timeDecr = new TimeDecreaseState(ref c_timerData, ref cart_incr);
        TimeIncreaseState s_timeIncr = new TimeIncreaseState(ref c_timerData, ref cart_incr);

        sm_timer = new StateMachine(s_timeIncr, StateRef.TIMER_INCR);
        sm_timer.AddState(s_timeDecr, StateRef.TIMER_DECR);
    }

    private void InitializeMessageClient()
    {
        cl_timer = new TimerMessageClient(ref c_stateData);
        MessageServer.Subscribe(ref cl_timer, MessageID.PAUSE);
        MessageServer.Subscribe(ref cl_timer, MessageID.PLAYER_FINISHED);
    }

    private void UpdateStateMachine()
    {
        // TODO: consider when this might get updated outside of a pause message
    }
}
