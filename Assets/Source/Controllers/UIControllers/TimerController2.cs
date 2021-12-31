
using UnityEngine;
using UnityEngine.UI;

public class TimerController2 : MonoBehaviour
{

    [SerializeField] private Text displaySeconds;
    [SerializeField] private Text displayMinutes;
    [SerializeField] private Text displayMilliSeconds;
    private float time = 0;

    private StateMachine sm_timer;
    private iMessageClient cl_timer;
    private StateData c_stateData;

    CacheIntString cacheSeconds = new CacheIntString(
        (seconds) => seconds % 60, //describe how seconds (key) will be translated to useful value (hash)
        (second) => second.ToString("00") //you describe how string is built based on given value (hash)
        , 0, 59, 1 //initialization range and step, so cache will be warmed up and ready
    );

    CacheIntString cacheMinutes = new CacheIntString(
        (seconds) => seconds / 60 % 60, // this translates input seconds to minutes
        (minute) => minute.ToString("00") // this translates minute to string
        , 0, 60, 60 //minutes needs a step of 60 seconds
    );
    CacheDoubleIntString cacheMilliSeconds = new CacheDoubleIntString(
        (seconds) => (int)System.Math.Round(seconds % 1d * 1000d), //extract 3 decimal places
        (second) => second.ToString("000")
        , 0d, 0.999d, 0.001d //1ms step
    );

    // Start is called before the first frame update
    void Start()
    {
        c_stateData = new StateData();
        c_stateData.b_updateState = true;

        TimerActiveState s_active = new TimerActiveState();

        sm_timer = new StateMachine(StateRef.TIMER_STEP);
        sm_timer.AddState(s_active, StateRef.TIMER_STEP);

        InitializeMessageClient();
    }

    // Use update instead of fixed update as we don't really "care" about simulation here
    void Update()
    {
        if (!c_stateData.b_updateState)
        {
            return;
        }

        if (c_stateData.b_preStarted)
        {
            return;
        }


        time += Time.deltaTime;
        int seconds = Mathf.FloorToInt(time);

        displaySeconds.text = cacheSeconds[seconds];
        displayMinutes.text = cacheMinutes[seconds];
        displayMilliSeconds.text = cacheMilliSeconds[time];
    }

    private void InitializeMessageClient()
    {
        cl_timer = new TimerMessageClient(ref c_stateData);
        MessageServer.Subscribe(ref cl_timer, MessageID.PAUSE);
        MessageServer.Subscribe(ref cl_timer, MessageID.PLAYER_FINISHED);
        MessageServer.Subscribe(ref cl_timer, MessageID.COUNTDOWN_OVER);
    }
}