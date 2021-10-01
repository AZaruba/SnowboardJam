using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimestepInterpolator : MonoBehaviour
{
    private float[] f_fixedUpdateTimestamps;
    private int i_alternatingIndex;

    private static float f_interpolationValue;
    public static float FIXED_INTERPOLATION_VALUE
    {
        get { return f_interpolationValue; }
    }

    // Start is called before the first frame update
    void Start()
    {
        f_fixedUpdateTimestamps = new float[2];
        i_alternatingIndex = Constants.ZERO;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        i_alternatingIndex = i_alternatingIndex == Constants.ZERO ? Constants.ONE : Constants.ZERO;
        f_fixedUpdateTimestamps[i_alternatingIndex] = Time.fixedTime;
    }

    void Update()
    {
        float newTime = f_fixedUpdateTimestamps[i_alternatingIndex];
        float oldTime = f_fixedUpdateTimestamps[i_alternatingIndex == Constants.ZERO ? Constants.ONE : Constants.ZERO];

        f_interpolationValue = newTime != oldTime ? (Time.time - newTime) / (newTime - oldTime) : Constants.ONE;
    }
}
