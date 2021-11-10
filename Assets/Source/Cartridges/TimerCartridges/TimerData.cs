using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class TimerData
{
    public TimerData()
    {
        f_currentTime = Constants.ZERO_F;
        s_timerString = new StringBuilder("0:00:00", 8);
    }

    public float f_currentTime { get; set; }
    public StringBuilder s_timerString;
}
