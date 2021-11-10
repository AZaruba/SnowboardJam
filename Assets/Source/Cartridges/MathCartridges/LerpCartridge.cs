using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpCartridge
{
    public void LerpFloat(ref float subject, float target, float inertia = Constants.LERP_DEFAULT)
    {
        subject = Mathf.Lerp(subject, target, inertia);
    }

    public void LerpVector2(ref Vector2 subject, Vector2 target, float inertia = Constants.LERP_DEFAULT)
    {
        subject = Vector2.Lerp(subject, target, inertia);
    }

    public void LerpVector3(ref Vector3 subject, Vector3 target, float inertia = Constants.LERP_DEFAULT)
    {
        subject = Vector3.Lerp(subject, target, inertia);
    }

    public void LerpColor(ref Color subject, Color target, float inertia = Constants.LERP_DEFAULT)
    {
        subject = Color.Lerp(subject, target, inertia);
    }
}
