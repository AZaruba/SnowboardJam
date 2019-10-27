using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iPlayerState {

    /// <summary>
    /// The interface defining a function which gives the state
    /// access to player data. Should be invoked in every state's
    /// constructor.
    /// </summary>
    /// <param name="dataIn">The current object's PlayerData</param>
    // void ProvideData(PlayerData dataIn);

    void Act(ref PlayerData c_playerData);
    StateRef GetNextState(Command cmd);
    void TransitionAct(ref PlayerData c_playerData);
}
