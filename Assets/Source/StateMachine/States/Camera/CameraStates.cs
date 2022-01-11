using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPreviewStateO : iState
{
    CameraPreviewData c_previewData;
    CameraPositionData_Old_Old c_positionData;
    CameraPreviewActiveData c_activeData;

    public CameraPreviewStateO(ref CameraPositionData_Old_Old dataIn, ref CameraPreviewData pDataIn, ref CameraPreviewActiveData aDataIn)
    {
        this.c_previewData = pDataIn;
        this.c_positionData = dataIn;
        this.c_activeData = aDataIn;
    }

    public void Act()
    {
        int i = c_activeData.i_currentPreviewIndex;
        Vector3 currentPosition = c_positionData.v_currentPosition;

        VelocityCartridge.LerpPosition(ref currentPosition,
            c_previewData.PreviewShots[i].StartPosition,
            c_previewData.PreviewShots[i].EndPosition,
            c_activeData.f_currentShotTime / c_previewData.PreviewShots[i].Time);

        c_activeData.f_currentShotTime += Time.deltaTime;
        c_positionData.v_currentPosition = currentPosition;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.START_COUNTDOWN)
        {
            return StateRef.FOLLOWING;
        }
        return StateRef.PREVIEW_TRACKING;
    }

    public void TransitionAct()
    {
        IncrementCartridge.Rotate(ref c_activeData.i_currentPreviewIndex, 1, c_previewData.PreviewShots.Count);
        c_positionData.q_currentRotation = Quaternion.Euler(c_previewData.PreviewShots[c_activeData.i_currentPreviewIndex].CameraAngle);
        c_positionData.v_currentPosition = c_previewData.PreviewShots[c_activeData.i_currentPreviewIndex].StartPosition;
        c_activeData.f_currentShotTime = Constants.ZERO_F;
    }
}

public class CameraFollowTargetState : iState
{

    private CameraData c_cameraData;
    private CameraPositionData_Old_Old c_positionData;

    public CameraFollowTargetState(ref CameraData cameraDataIn, ref CameraPositionData_Old_Old positionDataIn)
    {
        this.c_cameraData = cameraDataIn;
        this.c_positionData = positionDataIn;
    }

    public void Act()
    {
        Vector3 cameraPosition = c_positionData.v_currentPosition;
        Quaternion cameraRotation = c_positionData.q_currentRotation;

        CameraMotionCartridge.HorizontalFollow(ref cameraPosition, c_positionData.v_currentTargetTranslation, c_positionData.v_currentTargetPosition,
                                               c_positionData.q_currentTargetRotation, cameraRotation, c_cameraData.f_followDistance);

        CameraMotionCartridge.VerticalFollow(ref cameraPosition, c_positionData.v_currentTargetTranslation,
                                             ref cameraRotation, c_positionData.v_currentTargetPosition,
                                             c_cameraData.MaxCameraAngle);

        CameraMotionCartridge.KeepAboveGround(ref cameraPosition, c_positionData.q_currentTargetRotation, c_cameraData.f_followHeight, c_positionData.f_distanceToGround);

        CameraMotionCartridge.FocusOnPlayer(ref cameraRotation,
                                            c_positionData.q_currentTargetRotation,
                                            cameraPosition,
                                            c_positionData.v_currentTargetPosition,
                                            c_positionData.v_currentTargetTranslation,
                                            c_cameraData.f_targetOffset,
                                            c_cameraData.f_offsetHeight);

        c_positionData.v_currentPosition = cameraPosition;
        c_positionData.q_currentRotation = cameraRotation;
    }

    public StateRef GetNextState(Command cmd)
    {
        return StateRef.FOLLOWING;
    }

    public void TransitionAct()
    {
        // set position to desired position
        c_positionData.v_currentPosition = c_positionData.v_currentTargetPosition + c_positionData.q_currentTargetRotation * Vector3.forward * (c_cameraData.f_followDistance * -1);

        Quaternion cameraRotation = c_positionData.q_currentRotation;

        CameraMotionCartridge.FocusOnPlayer(ref cameraRotation,
                                            c_positionData.q_currentTargetRotation,
                                            c_positionData.v_currentPosition, 
                                            c_positionData.v_currentTargetPosition, 
                                            c_positionData.v_currentTargetTranslation, 
                                            c_cameraData.f_targetOffset,
                                            c_cameraData.f_offsetHeight);

        c_positionData.q_currentRotation = cameraRotation;
        /*
        // look at player
        Vector3 normal = c_positionData.q_currentTargetRotation * Vector3.up;
        Vector3 dir = c_positionData.v_currentTargetPosition - c_positionData.v_currentPosition;

        c_positionData.q_currentRotation = Quaternion.LookRotation(dir.normalized, normal.normalized);
        */
    }

}
