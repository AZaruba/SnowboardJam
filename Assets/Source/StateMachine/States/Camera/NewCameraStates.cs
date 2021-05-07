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
    private NewCameraData c_cameraData;
    private NewCameraPositionData c_positionData;
    private NewCameraTargetData c_targetData;
    private NewCameraLastFrameData c_lastFrameData;

    public CameraFollowState(ref NewCameraData dataIn, 
                             ref NewCameraPositionData posDataIn, 
                             ref NewCameraTargetData targetDataIn,
                             ref NewCameraLastFrameData lastFrameDataIn)
    {
        this.c_cameraData = dataIn;
        this.c_positionData = posDataIn;
        this.c_targetData = targetDataIn;
        this.c_lastFrameData = lastFrameDataIn;
    }
    public void Act()
    {
        Vector3 currentPosition = c_positionData.v_currentPosition;
        Quaternion currentRotation = c_positionData.q_currentRotation;
        float currentTransVelocity = c_positionData.f_currentTranslationVelocity;
        float currentVertVelocity = c_positionData.f_currentVerticalVelocity;

        float distanceToTarget;
        float verticalAngleDiscrepancy;
        float verticalAngleToTarget;

        Quaternion outRotation;

        Vector3 targetTranslationVec = c_targetData.q_currentTargetRotation * Vector3.forward * c_targetData.f_currentTargetVelocity;
        float targetHorizontalVelocity = Mathf.Abs(targetTranslationVec.x + targetTranslationVec.z);


        CameraOrientationCartridge.CalculateHorizontalRotation(out outRotation, 
                                                               currentRotation, 
                                                               c_targetData.q_currentTargetRotation,
                                                               c_targetData.v_currentTargetPosition - c_positionData.v_currentPosition);

        CameraOrientationCartridge.CalculateVerticalRotation(out verticalAngleToTarget,
                                                             currentRotation,
                                                              (c_targetData.v_currentTargetPosition + c_targetData.q_currentTargetRotation * c_cameraData.TargetLookOffset) - c_positionData.v_currentPosition);
;

        CameraOrientationCartridge.ApplyRotation(ref currentRotation,
                                                 outRotation);

        CameraOrientationCartridge.ApplyRotation(ref currentRotation,
                                                 Quaternion.AngleAxis(verticalAngleToTarget, currentRotation * Vector3.right).normalized);

        CameraOrientationCartridge.CalculateHorizontalDistance(out distanceToTarget,
                                                               currentPosition,
                                                               c_targetData.v_currentTargetPosition,
                                                               c_targetData.q_currentTargetRotation);

        CameraOrientationCartridge.AccelerateTranslationalVelocity(ref currentTransVelocity,
                                                                   distanceToTarget,
                                                                   c_cameraData.MaxFollowDistance,
                                                                   c_cameraData.MinFollowDistance,
                                                                   targetHorizontalVelocity);

        CameraOrientationCartridge.CalculateVerticalDifference(out verticalAngleDiscrepancy, 
                                                               (c_targetData.v_currentTargetPosition + c_targetData.q_currentTargetRotation * c_cameraData.TargetLookOffset) - c_positionData.v_currentPosition,
                                                               currentRotation);

        CameraOrientationCartridge.AccelerateVerticalVelocity(ref currentVertVelocity,
                                                             c_cameraData.MaxVerticalVelocity,
                                                             c_cameraData.DesiredVerticalAngle,
                                                             c_cameraData.MaximumVerticalAngle,
                                                             verticalAngleDiscrepancy);

        CameraOrientationCartridge.TranslateHorizontalPosition(ref currentPosition,
                                                     currentRotation * Vector3.forward,
                                                     currentTransVelocity);

        CameraOrientationCartridge.TranslateVerticalPosition(ref currentPosition,
                                                             currentVertVelocity);

        c_positionData.f_currentTranslationVelocity = currentTransVelocity;
        c_positionData.f_currentVerticalVelocity = currentVertVelocity;
        c_positionData.v_currentPosition = currentPosition;
        c_positionData.q_currentRotation = currentRotation;
    }

    public StateRef GetNextState(Command cmd)
    {
        return StateRef.TRACKING;
    }

    public void TransitionAct()
    {
        
    }
}
