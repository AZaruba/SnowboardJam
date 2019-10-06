using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iCameraState {

    /// <summary>
    /// The interface defining a function which gives the state
    /// access to player data. Should be invoked in every state's
    /// constructor.
    /// </summary>
    /// <param name="dataIn">The current object's PlayerData</param>
    // void ProvideData(PlayerData dataIn);

    void Act(ref CameraData c_cameraData);
    StateRef GetNextState(Command cmd);
}