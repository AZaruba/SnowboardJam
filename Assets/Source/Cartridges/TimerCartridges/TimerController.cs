using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    // TODO: how do we grab the time for the current level if different levels have different time limits?
    private TimerData c_timerData;
    private StateData c_stateData;
    private StateMachine sm_timer;

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

        UpdateStateMachine();
        sm_timer.Act();

        float displayTime = c_timerData.f_currentTime;
        int minutes = (int)displayTime / 60;
        int seconds = (int)displayTime % 60;
        int millis = (int)(displayTime * 100) % 100;

        /*
        c_timerData.s_timerString.Clear();
        c_timerData.s_timerString.AppendFormat(Constants.TIME_FORMAT_STRING, minutes, seconds, millis);
        timerText.text = c_timerData.s_timerString.ToString();
        */
    }

    private void SetDefaultTimerData()
    {
        c_timerData = new TimerData();
        c_stateData = new StateData();

        c_stateData.b_updateState = true;
    }

    private void InitializeStateMachine()
    {

        TimeDecreaseState s_timeDecr = new TimeDecreaseState(ref c_timerData);
        TimeIncreaseState s_timeIncr = new TimeIncreaseState(ref c_timerData);

        sm_timer = new StateMachine(StateRef.TIMER_INCR);
        sm_timer.AddState(s_timeIncr, StateRef.TIMER_INCR);
        sm_timer.AddState(s_timeDecr, StateRef.TIMER_DECR);
    }

    private void InitializeMessageClient()
    {
        cl_timer = new TimerMessageClient(ref c_stateData);
        MessageServer.Subscribe(ref cl_timer, MessageID.PAUSE);
        MessageServer.Subscribe(ref cl_timer, MessageID.PLAYER_FINISHED);
        MessageServer.Subscribe(ref cl_timer, MessageID.COUNTDOWN_OVER);
    }

    private void UpdateStateMachine()
    {
        if (c_stateData.b_preStarted == false)
        {
            sm_timer.Execute(Command.COUNTDOWN_OVER);
            c_stateData.b_preStarted = true; // we want to execute this only once
        }
    }
}
