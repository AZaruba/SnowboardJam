using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPreviewState : iState
{
    CameraPreviewData c_previewData;
    NewCameraPositionData c_positionData;
    CameraPreviewActiveData c_activeData;

    public CameraPreviewState(ref NewCameraPositionData dataIn, ref CameraPreviewData pDataIn, ref CameraPreviewActiveData aDataIn)
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

public class CameraFollowState : iState
{
    /* Goals:
     * Camera moves using the same Piangole principle of "move forward and backward, then rotate"
     * 
     * But rotations should be twin circles a la first person shooter controls
     * - the horizontal motion of the player rotates the camera around the global up axis
     * - the vertical motion of the player rotates the camera around the local "right" or "left" axis
     * 
     * After this:
     * - Maximal vertical angle determines vertical motion
     * - Maximal distance determines horizontal motion
     * 
     * The camera should accelerate and decelerate to the player's current movement
     *    - this can be achieved by checking the difference in position from last to current frame
     *    - this also enables us to not really track maximum speed
     */ 
    public void Act()
    {
        throw new System.NotImplementedException();
    }

    public StateRef GetNextState(Command cmd)
    {
        throw new System.NotImplementedException();
    }

    public void TransitionAct()
    {
        throw new System.NotImplementedException();
    }
}
