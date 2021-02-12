using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPreviewState : iState
{
    CameraPreviewData c_previewData;
    CameraPositionData c_positionData;
    CameraPreviewActiveData c_activeData;

    public CameraPreviewState(ref CameraPositionData dataIn, ref CameraPreviewData pDataIn, ref CameraPreviewActiveData aDataIn)
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
    private CameraPositionData c_positionData;

    public CameraFollowTargetState(ref CameraData cameraDataIn, ref CameraPositionData positionDataIn)
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
                                            cameraPosition,
                                            c_positionData.v_currentTargetPosition,
                                            c_positionData.v_currentTargetTranslation,
                                            c_cameraData.f_targetOffset);

        c_positionData.v_currentPosition = cameraPosition;
        c_positionData.q_currentRotation = cameraRotation;

        /* Yoann Pignole's camera guide in our case:
         * 
         * 1) Force camera outside of wall surface:
         *    a. Raycast from the camera to the player. If there's something in the way, move in front of it
         *       IMMEDIATELY
         *       
         * 2) Force camera above ground
         *    a. Check directly downard for the surface position, keep a certain distance above it.
         *    b. No ground below? We can leave it as is, as long as we are at the right height for the player
         *    
         * 3) Keep camera angle within a range
         *    a. if above the max range or below max range, translate vertically the same amount to stay within
         *       this constraint
         *       
         * 4) Follow player forward
         *    a. The horizontal motion (relative to the player's direction of travel) should be a composite 
         *       of a translation along the player's direction of travel and a rotation
         *    b. "unfolded" top down view:
         *        i. There's a "z" component to the player's motion, translate this amount
         *        ii. There's an "x" component, rotate to compensate
         *        iii. There's a "z" component, Which should be handled by (2) and (3)
         *        
         * Notes on 4.iii:
         *     -We might not want a max angle if we have vertical or near-vertical drops. At least not in 
         *     A global scope. This should be a function of the player's direction of travel, to at least
         *     ensure that we aren't getting into conflicts between (2) and (3).
         *     
         *     -To keep (2) working, we should be a little lax about the player's distance, in most cases
         *     the diference caused by forcing (2) should be minimal as there won't be terribly rapid changes
         *     in orientation between two surfaces.
         *     
         *     -So the forcing the camera above ground and out off a surface should happen AFTER we apply
         *     Yoann Pignole's basics to our camera motion.
         *   
         * 5) Since the camera shouldn't always be vertical, do an eased and constrained motion from the camera's 
         *    rotation to the player's rotation.
         *    a. Keep a limit on the amount we can rotate per tick
         *    b. Make sure this isn't vomit-inducing
         *    
         * Notes: How do we do easing? Check for error from ideals in CameraData and then move based on that?
         */
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
                                            c_positionData.v_currentPosition, 
                                            c_positionData.v_currentTargetPosition, 
                                            c_positionData.v_currentTargetTranslation, 
                                            c_cameraData.f_targetOffset);

        c_positionData.q_currentRotation = cameraRotation;
        /*
        // look at player
        Vector3 normal = c_positionData.q_currentTargetRotation * Vector3.up;
        Vector3 dir = c_positionData.v_currentTargetPosition - c_positionData.v_currentPosition;

        c_positionData.q_currentRotation = Quaternion.LookRotation(dir.normalized, normal.normalized);
        */
    }

}
