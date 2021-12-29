
using UnityEngine;
using UnityEngine.UI;

public class TimerController2 : MonoBehaviour
{

    [SerializeField] private Text displaySeconds;
    [SerializeField] private Text displayMinutes;
    [SerializeField] private Text displayMilliSeconds;
    private float time = 60000;

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
        
    }

    // Use update instead of fixed update as we don't really "care" about simulation here
    void Update()
    {
        this.time += Time.deltaTime;
        int seconds = Mathf.FloorToInt(time);

        displaySeconds.text = cacheSeconds[seconds];
        displayMinutes.text = cacheMinutes[seconds];
        displayMilliSeconds.text = cacheMilliSeconds[time];
    }
}