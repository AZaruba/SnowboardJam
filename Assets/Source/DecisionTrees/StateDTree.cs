using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDTree
{
    public List<StateDTree> l_children;
    public StateRef s_heldState;

    public StateDTree()
    {
        this.s_heldState = StateRef.ERROR_STATE;
        this.l_children = new List<StateDTree>();
    }

    public StateDTree(StateRef stateIn)
    {
        this.s_heldState = stateIn;
    }

    public bool AssignChild(ref StateDTree nodeIn)
    {
        if (l_children.Contains(nodeIn))
        {
            return false;
        }

        l_children.Add(nodeIn);
        return true;
    }

    public StateRef GetState()
    {
        return s_heldState;
    }

    public StateDTree GetChild(StateRef stateIn)
    {
        foreach (StateDTree child in l_children)
        {
            if (child.GetState() == stateIn)
            {
                return child;
            }
        }

        return null;
    }
}

public class AudioDTree : StateDTree
{
    public AudioRef a_clipOut;

    // non-leaf branch constructor
    public AudioDTree(StateRef stateIn)
    {
        this.s_heldState = stateIn;
        this.a_clipOut = AudioRef.ERROR_CLIP;
        this.l_children = new List<StateDTree>();
    }

    // leaf constructor
    public AudioDTree(StateRef stateIn, AudioRef audioIn)
    {
        this.s_heldState = stateIn;
        this.a_clipOut = audioIn;
    }

    public AudioRef GetLeaf()
    {
        return a_clipOut;
    }

    public bool AssignChild(ref AudioDTree nodeIn)
    {
        if (l_children.Contains(nodeIn))
        {
            return false;
        }

        l_children.Add(nodeIn);
        return true;
    }

    new public AudioDTree GetChild(StateRef stateIn)
    {
        foreach (AudioDTree child in l_children)
        {
            if (child.GetState() == stateIn)
            {
                return child;
            }
        }

        return this;
    }
}
