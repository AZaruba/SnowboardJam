using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPreviewState : iState
{
    CameraPreviewData c_previewData;
    CameraData c_cameraData;
    CameraPreviewActiveData c_activeData;

    public CameraPreviewState(ref CameraData dataIn, ref CameraPreviewData pDataIn, ref CameraPreviewActiveData aDataIn)
    {
        this.c_previewData = pDataIn;
        this.c_cameraData = dataIn;
        this.c_activeData = aDataIn;
    }

    public void Act()
    {
        int i = c_activeData.i_currentPreviewIndex;
        Vector3 currentPosition = c_cameraData.v_currentPosition;

        VelocityCartridge.LerpPosition(ref currentPosition,
            c_previewData.PreviewShots[i].StartPosition,
            c_previewData.PreviewShots[i].EndPosition,
            c_activeData.f_currentShotTime / c_previewData.PreviewShots[i].Time);

        c_activeData.f_currentShotTime += Time.deltaTime;
        c_cameraData.v_currentPosition = currentPosition;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.START_COUNTDOWN)
        {
            return StateRef.TRACKING;
        }
        return StateRef.PREVIEW_TRACKING;
    }

    public void TransitionAct()
    {
        IncrementCartridge.Rotate(ref c_activeData.i_currentPreviewIndex, 1, c_previewData.PreviewShots.Count);
        c_cameraData.q_cameraRotation = Quaternion.Euler(c_previewData.PreviewShots[c_activeData.i_currentPreviewIndex].CameraAngle);
        c_cameraData.v_currentDirection = c_cameraData.q_cameraRotation * Vector3.forward;
        c_cameraData.v_currentPosition = c_previewData.PreviewShots[c_activeData.i_currentPreviewIndex].StartPosition;
        c_activeData.f_currentShotTime = Constants.ZERO_F;
    }
}
