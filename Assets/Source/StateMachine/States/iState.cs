using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iState
{
    void Act();
    StateRef GetNextState(Command cmd);
    void TransitionAct();
}

public interface iPhysicsState : iState
{
    void FixedAct();
}
