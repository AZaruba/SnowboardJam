using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CountdownController : MonoBehaviour
{
    [SerializeField] private GameObject InGameInterface;
    [SerializeField] private GameObject PreviewInterface;
    [SerializeField] private Image CountdownDisplayImage;

    [SerializeField] private Sprite[] CountdownSprites;

    private CountdownData c_countdownData;
    private StateMachine sm_countdown;

    // Start is called before the first frame update
    void Start()
    {
        InGameInterface.SetActive(false);
        PreviewInterface.SetActive(true);
        c_countdownData = new CountdownData(CountdownSprites.Length);

        InitializeStateMachine();
        CountdownDisplayImage.sprite = CountdownSprites[c_countdownData.i_countdownTime - 1];
    }

    private void EnginePush()
    {
        if (c_countdownData.i_countdownTime == c_countdownData.i_targetTime)
        {
            MessageServer.SendMessage(MessageID.COUNTDOWN_OVER, new Message());
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateStateMachine();
        sm_countdown.Act();
        EnginePush();
    }

    private void UpdateStateMachine()
    {
        if (GlobalInputController.GetInputAction(ControlAction.CONFIRM, KeyValue.PRESSED)
            && !CountdownDisplayImage.enabled)
        {
            InGameInterface.SetActive(true);
            PreviewInterface.SetActive(false);
            CountdownDisplayImage.enabled = true;

            MessageServer.SendMessage(MessageID.COUNTDOWN_START, new Message());
            sm_countdown.Execute(Command.START_COUNTDOWN);
        }
        // we want to continuously countdown, tick, then countdown again
        if (c_countdownData.f_currentCountdownTime < c_countdownData.i_countdownTime)
        {
            sm_countdown.Execute(Command.TICK_TIMER);
            sm_countdown.Execute(Command.START_TIMER_DOWN);

            CountdownDisplayImage.sprite = CountdownSprites[c_countdownData.i_countdownTime];
        }
    }

    private void InitializeStateMachine()
    {
        CountdownStartState state_start = new CountdownStartState();
        CountdownStepState state_step = new CountdownStepState(ref c_countdownData);
        DecrementCountdownState state_decr = new DecrementCountdownState(ref c_countdownData);

        sm_countdown = new StateMachine(state_start, StateRef.START_STATE);
        sm_countdown.AddState(state_step, StateRef.TIMER_STEP);
        sm_countdown.AddState(state_decr, StateRef.TIMER_DECR);
    }
}

public class CountdownData
{
    public float f_currentCountdownTime;
    public int   i_countdownTime;
    public int   i_targetTime;

    public CountdownData(int CountdownTime)
    {
        f_currentCountdownTime = CountdownTime;
        i_countdownTime = CountdownTime;
        i_targetTime = Constants.ZERO;
    }
}
