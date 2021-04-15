using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraData : MonoBehaviour
{
    public float MaxFollowDistance;
    public float MinFollowDistance;
    public float FollowHeight;
    public float MaximumVerticalAngle;
    public float RotationalAcceleration;
    public float TranslationAcceleration;
}

public class NewCameraTargetData
{
    public Vector3 v_currentTargetPosition;
    public Quaternion q_currentTargetRotation;
    public float f_currentTargetVelocity;

    public NewCameraTargetData(Vector3 posIn, Quaternion rotIn)
    {
        this.v_currentTargetPosition = posIn;
        this.q_currentTargetRotation = rotIn;
        f_currentTargetVelocity = Constants.ZERO_F;
    }
}

public class NewCameraPositionData
{
    public NewCameraPositionData(Vector3 pos, Quaternion rot, float angle)
    {
        this.v_currentPosition = pos;
        this.q_currentRotation = rot;
        this.f_currentCameraAngle = angle;
    }

    public Vector3 v_currentPosition;
    public Quaternion q_currentRotation;

    public float f_currentCameraAngle;
    public float f_currentTranslationVelocity;
    public float f_currentRotationalVelocity;
}

public class NewCameraLastFrameData
{
    public NewCameraLastFrameData(Vector3 thisPos,
                                  Vector3 tarPos,
                                  Quaternion thisRot,
                                  Quaternion tarRot)
    {
        this.v_lastPosition = thisPos;
        this.q_lastRotation = thisRot;

        this.v_lastTargetPosition = tarPos;
        this.q_lastTargetRotation = tarRot;
    }

    public Vector3 v_lastPosition;
    public Vector3 v_lastTargetPosition;

    public Quaternion q_lastRotation;
    public Quaternion q_lastTargetRotation;
}
